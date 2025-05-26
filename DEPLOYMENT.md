# SQL2CODE Navigator - Deployment Guide

This guide explains how to deploy the SQL2CODE Navigator application to AWS using Terraform and Elastic Beanstalk.

## 🏗️ Architecture Overview

The application is deployed using:

- **AWS Elastic Beanstalk**: Hosts the Next.js application
- **Amazon EC2**: Single t3.micro instance (cost-effective for demo)
- **Amazon S3**: Stores application versions
- **AWS IAM**: Manages permissions and roles
- **Amazon VPC**: Network configuration with public subnets
- **Nginx**: Reverse proxy for static file serving and performance

## 📋 Prerequisites

Before deploying, ensure you have:

1. **AWS Account** with appropriate permissions
2. **AWS CLI** installed and configured
3. **Terraform** (>= 1.0) installed
4. **Node.js** (>= 18) for local development

### AWS Permissions Required

Your AWS user/role needs permissions for:

- Elastic Beanstalk (full access)
- EC2 (VPC, Subnets, Security Groups)
- S3 (bucket creation and management)
- IAM (role and policy management)

## 🚀 Quick Deployment

### Option 1: Using the Deployment Script (Recommended)

```bash
# Make sure you're in the project root
./deploy.sh
```

The script will:

1. Check prerequisites (AWS CLI, Terraform)
2. Initialize Terraform if needed
3. Validate the configuration
4. Show you the deployment plan
5. Deploy the infrastructure
6. Provide the application URL

### Option 2: Manual Terraform Commands

```bash
# Navigate to terraform directory
cd terraform

# Initialize Terraform (first time only)
terraform init

# Review and customize variables
cp terraform.tfvars.example terraform.tfvars
# Edit terraform.tfvars with your preferences

# Plan the deployment
terraform plan

# Apply the configuration
terraform apply
```

## ⚙️ Configuration Options

### Environment Variables

Edit `terraform/terraform.tfvars` to customize:

```hcl
# Basic Configuration
app_name    = "sql2code-navigator"
environment = "demo"
aws_region  = "us-east-1"

# Instance Configuration
instance_type = "t3.micro"  # or t3.small for better performance

# Application Versions
max_app_versions = 3

# Tags
tags = {
  Project     = "sql2code-navigator"
  Environment = "demo"
  ManagedBy   = "terraform"
  Owner       = "your-name"
}
```

### Elastic Beanstalk Configuration

The deployment includes optimized configurations in `.ebextensions/`:

- **Node.js Setup**: Configures Node.js 20 environment
- **Build Process**: Handles npm install, build, and cleanup
- **Nginx Configuration**: Optimized for Next.js with static file serving

## 🌐 Application Features

Once deployed, your application will have:

- **Login Page**: Simple authentication UI (accepts any credentials)
- **Database Navigator**: Lists databases from the `public/output/` folder
- **File Explorer**: Collapsible sidebar with file tree navigation
- **File Viewer**: Displays file contents with syntax highlighting
- **Responsive Design**: Works on desktop and mobile devices

## 📁 File Structure

```
nextweb/
├── src/                          # Next.js application source
│   ├── app/                      # App router pages
│   ├── components/               # React components
│   └── ...
├── public/
│   └── output/                   # Database files (auto-loaded)
│       └── DemoDatabase/         # Example database
├── .ebextensions/                # Elastic Beanstalk configuration
│   ├── 01_nodejs.config         # Node.js environment
│   ├── 02_build.config          # Build process
│   └── 03_nginx.config          # Nginx configuration
├── terraform/                    # Infrastructure as Code
│   ├── main.tf                  # Main Terraform configuration
│   ├── variables.tf             # Input variables
│   ├── outputs.tf               # Output values
│   └── terraform.tfvars         # Variable values
├── deploy.sh                     # Deployment script
├── destroy.sh                    # Cleanup script
└── package.json                  # Node.js dependencies
```

## 🔧 Troubleshooting

### Common Issues

1. **AWS Credentials Not Configured**

   ```bash
   aws configure
   # Enter your Access Key ID, Secret Access Key, and region
   ```

2. **Terraform Not Found**

   ```bash
   # macOS
   brew install terraform

   # Or download from https://terraform.io/downloads
   ```

3. **Deployment Fails**

   - Check AWS permissions
   - Verify region availability
   - Check Elastic Beanstalk service limits

4. **Application Not Loading**
   - Wait 5-10 minutes for full deployment
   - Check Elastic Beanstalk environment health
   - Review application logs in AWS console

### Monitoring

Monitor your deployment:

- **AWS Console**: Elastic Beanstalk → Applications → sql2code-navigator-demo
- **Logs**: Available in the Elastic Beanstalk console
- **Health**: Environment health dashboard

## 💰 Cost Estimation

Approximate monthly costs (us-east-1):

- **t3.micro EC2 instance**: ~$8.50/month
- **Elastic Beanstalk**: Free (no additional charges)
- **S3 storage**: ~$0.10/month (minimal usage)
- **Data transfer**: ~$0.50/month (light usage)

**Total**: ~$9-10/month for a demo environment

## 🧹 Cleanup

To destroy all AWS resources:

```bash
# Using the destroy script (recommended)
./destroy.sh

# Or manually
cd terraform
terraform destroy
```

⚠️ **Warning**: This will permanently delete all resources and data!

## 🔒 Security Considerations

For production deployments, consider:

1. **HTTPS**: Enable SSL/TLS certificates
2. **Authentication**: Implement proper user authentication
3. **VPC**: Use private subnets for enhanced security
4. **IAM**: Follow principle of least privilege
5. **Monitoring**: Enable CloudWatch and AWS Config
6. **Backup**: Implement regular backups

## 📞 Support

If you encounter issues:

1. Check the troubleshooting section above
2. Review AWS CloudWatch logs
3. Verify your AWS permissions
4. Ensure all prerequisites are met

## 🎯 Next Steps

After successful deployment:

1. **Add Your Data**: Upload database files to `public/output/`
2. **Customize UI**: Modify the React components as needed
3. **Scale Up**: Consider upgrading instance type for production
4. **Monitor**: Set up CloudWatch alarms and monitoring
5. **Secure**: Implement proper authentication and HTTPS

---

**Happy Deploying! 🚀**
