# Terraform variables file for SQL2Code Navigator Demo

# Basic Configuration
app_name    = "sql2code-navigator"
environment = "demo"
aws_region  = "us-east-1"

# Elastic Beanstalk Configuration (Demo - single instance, no load balancer)
solution_stack_name = "64bit Amazon Linux 2023 v6.5.2 running Node.js 20"
instance_type       = "t3.micro"
max_app_versions    = 3

# Additional Tags
tags = {
  Project     = "sql2code-navigator"
  Environment = "demo"
  ManagedBy   = "terraform"
  Application = "DatabaseNavigator"
}