terraform {
  required_version = ">= 1.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
    archive = {
      source  = "hashicorp/archive"
      version = "~> 2.0"
    }
  }
}

# Configure AWS Provider
provider "aws" {
  region = var.aws_region
}

# Get default VPC
data "aws_vpc" "default" {
  default = true
}

# Create subnets in the default VPC (since none exist)
resource "aws_subnet" "public_1" {
  vpc_id                  = data.aws_vpc.default.id
  cidr_block              = "172.31.1.0/24"
  availability_zone       = "us-east-1a"
  map_public_ip_on_launch = true

  tags = {
    Name = "Public Subnet 1 (us-east-1a)"
  }
}

resource "aws_subnet" "public_2" {
  vpc_id                  = data.aws_vpc.default.id
  cidr_block              = "172.31.2.0/24"
  availability_zone       = "us-east-1b"
  map_public_ip_on_launch = true

  tags = {
    Name = "Public Subnet 2 (us-east-1b)"
  }
}

# Get the internet gateway for the default VPC
data "aws_internet_gateway" "default" {
  filter {
    name   = "attachment.vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# Create route table for public subnets
resource "aws_route_table" "public" {
  vpc_id = data.aws_vpc.default.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = data.aws_internet_gateway.default.id
  }

  tags = {
    Name = "Public Route Table"
  }
}

# Associate route table with subnets
resource "aws_route_table_association" "public_1" {
  subnet_id      = aws_subnet.public_1.id
  route_table_id = aws_route_table.public.id
}

resource "aws_route_table_association" "public_2" {
  subnet_id      = aws_subnet.public_2.id
  route_table_id = aws_route_table.public.id
}

# Create deployment package from parent directory (actual app location)
data "archive_file" "app_zip" {
  type        = "zip"
  source_dir  = "../"
  output_path = "${path.module}/database-navigator-app.zip"
  excludes = [
    "node_modules",
    ".git",
    "*.log",
    ".DS_Store",
    "terraform",
    "*.cache",
    "*.tmp",
    ".terraform*",
    "*.tfstate*",
    ".replit",
    "replit.md",
    "file-tree",
    ".next"
  ]
}

# Create S3 bucket for storing application versions
resource "aws_s3_bucket" "app_bucket" {
  bucket_prefix = "${var.app_name}-${var.environment}-versions-"
  force_destroy = true  # Allow deletion for demo
}

# Configure S3 bucket ownership controls
resource "aws_s3_bucket_ownership_controls" "app_bucket_ownership" {
  bucket = aws_s3_bucket.app_bucket.id
  rule {
    object_ownership = "BucketOwnerPreferred"
  }
}

# Configure S3 bucket ACL
resource "aws_s3_bucket_acl" "app_bucket_acl" {
  depends_on = [aws_s3_bucket_ownership_controls.app_bucket_ownership]
  bucket     = aws_s3_bucket.app_bucket.id
  acl        = "private"
}

# Upload application version to S3
resource "aws_s3_object" "app_version" {
  bucket = aws_s3_bucket.app_bucket.bucket
  key    = "${var.app_name}-${var.environment}-${formatdate("YYYY-MM-DD-hhmmss", timestamp())}.zip"
  source = data.archive_file.app_zip.output_path
  etag   = data.archive_file.app_zip.output_md5
}

# Create Elastic Beanstalk Application
resource "aws_elastic_beanstalk_application" "app" {
  name        = "${var.app_name}-${var.environment}"
  description = "SQL2Code Navigator Web Application - ${var.environment}"

  appversion_lifecycle {
    service_role          = aws_iam_role.beanstalk_service.arn
    max_count             = var.max_app_versions
    delete_source_from_s3 = true  # Clean up for demo
  }
}

# Create Application Version
resource "aws_elastic_beanstalk_application_version" "app_version" {
  name        = "${var.app_name}-${var.environment}-${formatdate("YYYY-MM-DD-hhmmss", timestamp())}"
  application = aws_elastic_beanstalk_application.app.name
  description = "Application version created by terraform"
  bucket      = aws_s3_bucket.app_bucket.bucket
  key         = aws_s3_object.app_version.key
}

# IAM role for Elastic Beanstalk service
resource "aws_iam_role" "beanstalk_service" {
  name = "${var.app_name}-${var.environment}-beanstalk-service-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "elasticbeanstalk.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "beanstalk_service" {
  role       = aws_iam_role.beanstalk_service.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSElasticBeanstalkService"
}

# IAM role for EC2 instances
resource "aws_iam_role" "beanstalk_ec2" {
  name = "${var.app_name}-${var.environment}-beanstalk-ec2-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ec2.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "beanstalk_ec2_web" {
  role       = aws_iam_role.beanstalk_ec2.name
  policy_arn = "arn:aws:iam::aws:policy/AWSElasticBeanstalkWebTier"
}

resource "aws_iam_role_policy_attachment" "beanstalk_ec2_worker" {
  role       = aws_iam_role.beanstalk_ec2.name
  policy_arn = "arn:aws:iam::aws:policy/AWSElasticBeanstalkWorkerTier"
}

resource "aws_iam_role_policy_attachment" "beanstalk_ec2_container" {
  role       = aws_iam_role.beanstalk_ec2.name
  policy_arn = "arn:aws:iam::aws:policy/AWSElasticBeanstalkMulticontainerDocker"
}

# Create instance profile
resource "aws_iam_instance_profile" "beanstalk_ec2" {
  name = "${var.app_name}-${var.environment}-beanstalk-ec2-profile"
  role = aws_iam_role.beanstalk_ec2.name
}

# Create Elastic Beanstalk Environment
resource "aws_elastic_beanstalk_environment" "app_env" {
  name                = "${var.app_name}-${var.environment}-env"
  application         = aws_elastic_beanstalk_application.app.name
  solution_stack_name = var.solution_stack_name
  version_label       = aws_elastic_beanstalk_application_version.app_version.name

  # Settings for Single Instance (no load balancer)
  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "EnvironmentType"
    value     = "SingleInstance"
  }

  # VPC Configuration - use our created subnets
  setting {
    namespace = "aws:ec2:vpc"
    name      = "VPCId"
    value     = data.aws_vpc.default.id
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "Subnets"
    value     = aws_subnet.public_1.id
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "AssociatePublicIpAddress"
    value     = "true"
  }

  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "ServiceRole"
    value     = aws_iam_role.beanstalk_service.arn
  }

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "IamInstanceProfile"
    value     = aws_iam_instance_profile.beanstalk_ec2.name
  }

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "InstanceType"
    value     = var.instance_type
  }

  # Static files configuration (set in .ebextensions)
  # setting {
  #   namespace = "aws:elasticbeanstalk:environment:proxy:staticfiles"
  #   name      = "/public"
  #   value     = "client/dist"
  # }

  setting {
    namespace = "aws:elasticbeanstalk:environment:proxy"
    name      = "ProxyServer"
    value     = "nginx"
  }

  # Environment variables
  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "NODE_ENV"
    value     = "production"
  }

  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "PORT"
    value     = "8080"
  }

  # Health check settings
  setting {
    namespace = "aws:elasticbeanstalk:environment:process:default"
    name      = "HealthCheckPath"
    value     = "/"
  }

  setting {
    namespace = "aws:elasticbeanstalk:environment:process:default"
    name      = "MatcherHTTPCode"
    value     = "200"
  }

  # Deployment settings
  setting {
    namespace = "aws:elasticbeanstalk:command"
    name      = "DeploymentPolicy"
    value     = "AllAtOnce"
  }

  setting {
    namespace = "aws:elasticbeanstalk:command"
    name      = "BatchSizeType"
    value     = "Fixed"
  }

  setting {
    namespace = "aws:elasticbeanstalk:command"
    name      = "BatchSize"
    value     = "1"
  }

  tags = {
    Name        = "${var.app_name}-${var.environment}-env"
    Environment = var.environment
    ManagedBy   = "terraform"
  }
}
