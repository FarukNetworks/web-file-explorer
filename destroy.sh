#!/bin/bash

# SQL2CODE Navigator Destroy Script
# This script safely destroys the AWS infrastructure created by Terraform

set -e  # Exit on any error

echo "🗑️  SQL2CODE Navigator Infrastructure Destroy Script"
echo "=================================================="

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

# Check if Terraform state exists
if [ ! -f "terraform.tfstate" ] && [ ! -f ".terraform/terraform.tfstate" ]; then
    echo "⚠️  No Terraform state found. Nothing to destroy."
    exit 0
fi

echo "📋 Current infrastructure:"
echo "========================="
terraform show -no-color | head -20

echo ""
echo "⚠️  WARNING: This will destroy ALL infrastructure created by Terraform!"
echo "This includes:"
echo "  - Elastic Beanstalk Application and Environment"
echo "  - S3 Bucket and all application versions"
echo "  - IAM Roles and Policies"
echo "  - VPC Subnets and Route Tables"
echo ""
echo "🤔 Are you sure you want to destroy all infrastructure? (y/N)"
read -r response

if [[ ! "$response" =~ ^[Yy]$ ]]; then
    echo "❌ Destroy cancelled"
    exit 0
fi

echo ""
echo "🔄 Planning destruction..."
terraform plan -destroy -out=destroy-plan

echo ""
echo "🤔 Last chance! Proceed with destruction? (y/N)"
read -r response

if [[ ! "$response" =~ ^[Yy]$ ]]; then
    echo "❌ Destroy cancelled"
    rm -f destroy-plan
    exit 0
fi

echo ""
echo "🗑️  Destroying infrastructure..."
terraform apply destroy-plan

echo ""
echo "✅ Infrastructure destroyed successfully!"
echo ""
echo "🧹 Cleaning up local files..."
rm -f destroy-plan
rm -f terraform.tfstate.backup

echo ""
echo "🎉 Cleanup completed!"
echo "All AWS resources have been destroyed and local state cleaned up." 