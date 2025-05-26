variable "app_name" {
  description = "Name of the application"
  type        = string
  default     = "sql2code-navigator"
}

variable "environment" {
  description = "Environment name (e.g., dev, staging, prod)"
  type        = string
  default     = "demo"
}

variable "aws_region" {
  description = "AWS region to deploy resources"
  type        = string
  default     = "us-east-1"
}

variable "solution_stack_name" {
  description = "Elastic Beanstalk solution stack name"
  type        = string
  default     = "64bit Amazon Linux 2023 v6.5.2 running Node.js 20"
}

variable "instance_type" {
  description = "EC2 instance type for Elastic Beanstalk environment"
  type        = string
  default     = "t3.micro"
}

variable "max_app_versions" {
  description = "Maximum number of application versions to keep"
  type        = number
  default     = 3
}

variable "tags" {
  description = "Additional tags for resources"
  type        = map(string)
  default     = {}
}