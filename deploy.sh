#!/bin/bash

# SQL2CODE Navigator Deployment Script
# This script deploys the Next.js application to AWS Elastic Beanstalk using Terraform

set -e  # Exit on any error

echo "🚀 Starting deployment of SQL2CODE Navigator..."

# Check if AWS CLI is configured
if ! aws sts get-caller-identity > /dev/null 2>&1; then
    echo "❌ AWS CLI is not configured or credentials are invalid"
    echo "Please run 'aws configure' to set up your credentials"
    exit 1
fi

# Check if Terraform is installed
if ! command -v terraform &> /dev/null; then
    echo "❌ Terraform is not installed"
    echo "Please install Terraform from https://terraform.io/downloads"
    exit 1
fi

# Navigate to terraform directory
cd terraform

echo "📋 Checking Terraform configuration..."

# Initialize Terraform if needed
if [ ! -d ".terraform" ]; then
    echo "🔧 Initializing Terraform..."
    terraform init
fi

# Validate configuration
echo "✅ Validating Terraform configuration..."
terraform validate

# Check if terraform.tfvars exists
if [ ! -f "terraform.tfvars" ]; then
    echo "⚠️  terraform.tfvars not found, copying from example..."
    cp terraform.tfvars.example terraform.tfvars
    echo "📝 Please edit terraform.tfvars with your specific values"
    echo "Press Enter to continue after editing, or Ctrl+C to exit"
    read
fi

# Plan the deployment
echo "📊 Planning deployment..."
terraform plan -out=tfplan

# Ask for confirmation
echo ""
echo "🤔 Do you want to proceed with the deployment? (y/N)"
read -r response
if [[ ! "$response" =~ ^[Yy]$ ]]; then
    echo "❌ Deployment cancelled"
    exit 0
fi

# Apply the configuration
echo "🚀 Deploying application..."
terraform apply tfplan

# Get outputs
echo ""
echo "✅ Deployment completed successfully!"
echo ""
echo "📋 Deployment Information:"
echo "=========================="
terraform output

echo ""
echo "🌐 Your application should be available at:"
terraform output -raw application_url

echo ""
echo "📝 Note: It may take a few minutes for the application to be fully available"
echo "   You can monitor the deployment in the AWS Elastic Beanstalk console"

# Clean up plan file
rm -f tfplan

echo ""
echo "🎉 Deployment script completed!" 