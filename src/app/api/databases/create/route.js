import { NextResponse } from 'next/server';
import fs from 'fs';
import path from 'path';

export async function POST(request) {
  try {
    const { name, type, environment, connectionString, enableSSL, enableAutoBackup } = await request.json();

    // Validate required fields
    if (!name) {
      return NextResponse.json({ error: 'Database name is required' }, { status: 400 });
    }

    // Sanitize database name for folder creation
    const sanitizedName = name.replace(/[^a-zA-Z0-9-_]/g, '_');

    // Create the database folder path
    const outputDir = path.join(process.cwd(), 'public', 'output');
    const databaseDir = path.join(outputDir, sanitizedName);

    // Check if database already exists
    if (fs.existsSync(databaseDir)) {
      return NextResponse.json({ error: 'Database with this name already exists' }, { status: 400 });
    }

    // Create the database directory structure
    fs.mkdirSync(databaseDir, { recursive: true });

    // Create subdirectories
    const subdirs = ['models', 'sql', 'docs', 'src'];
    subdirs.forEach(subdir => {
      fs.mkdirSync(path.join(databaseDir, subdir), { recursive: true });
    });

    // Create database configuration file
    const config = {
      name,
      type,
      environment,
      connectionString: connectionString || '',
      enableSSL,
      enableAutoBackup,
      createdAt: new Date().toISOString(),
      version: '1.0.0'
    };

    fs.writeFileSync(
      path.join(databaseDir, 'database.json'),
      JSON.stringify(config, null, 2)
    );

    // Create a README file
    const readmeContent = `# ${name} Database

## Database Information
- **Type**: ${type}
- **Environment**: ${environment}
- **Created**: ${new Date().toLocaleDateString()}

## Directory Structure
- \`models/\` - Database models and schemas
- \`sql/\` - SQL scripts and queries
- \`docs/\` - Documentation files
- \`src/\` - Source code files

## Configuration
${enableSSL ? '- SSL/TLS enabled' : '- SSL/TLS disabled'}
${enableAutoBackup ? '- Auto backup enabled' : '- Auto backup disabled'}

## Getting Started
This database was created using the SQL2CODE Navigator tool.
`;

    fs.writeFileSync(path.join(databaseDir, 'README.md'), readmeContent);

    // Create sample files in subdirectories

    // Models directory - sample schema file
    const sampleSchema = `-- ${name} Database Schema
-- Generated on ${new Date().toLocaleDateString()}

-- Example table structure
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Add more tables as needed
`;
    fs.writeFileSync(path.join(databaseDir, 'models', 'schema.sql'), sampleSchema);

    // SQL directory - sample queries
    const sampleQueries = `-- Sample queries for ${name} database

-- Get all users
SELECT * FROM users ORDER BY created_at DESC;

-- Count total users
SELECT COUNT(*) as total_users FROM users;

-- Find user by email
SELECT * FROM users WHERE email = $1;
`;
    fs.writeFileSync(path.join(databaseDir, 'sql', 'queries.sql'), sampleQueries);

    // Docs directory - API documentation
    const apiDocs = `# ${name} API Documentation

## Overview
This document describes the API endpoints for the ${name} database.

## Endpoints

### Users
- \`GET /api/users\` - Get all users
- \`POST /api/users\` - Create a new user
- \`GET /api/users/:id\` - Get user by ID
- \`PUT /api/users/:id\` - Update user
- \`DELETE /api/users/:id\` - Delete user

## Authentication
${enableSSL ? 'This API uses SSL/TLS encryption for secure communication.' : 'Consider enabling SSL/TLS for production use.'}

## Backup
${enableAutoBackup ? 'Automatic backups are enabled for this database.' : 'Manual backup procedures should be implemented.'}
`;
    fs.writeFileSync(path.join(databaseDir, 'docs', 'api.md'), apiDocs);

    // Src directory - sample configuration
    const srcConfig = `{
  "database": {
    "name": "${name}",
    "type": "${type}",
    "environment": "${environment}",
    "ssl": ${enableSSL},
    "autoBackup": ${enableAutoBackup}
  },
  "connection": {
    "host": "localhost",
    "port": ${type === 'PostgreSQL' ? 5432 : type === 'MySQL' ? 3306 : 1433},
    "database": "${sanitizedName}",
    "ssl": ${enableSSL}
  },
  "features": {
    "migrations": true,
    "seeding": true,
    "logging": true
  }
}`;
    fs.writeFileSync(path.join(databaseDir, 'src', 'config.json'), srcConfig);

    return NextResponse.json({
      success: true,
      message: 'Database created successfully',
      database: {
        name: sanitizedName,
        originalName: name,
        path: `/output/${sanitizedName}`
      }
    });

  } catch (error) {
    console.error('Error creating database:', error);
    return NextResponse.json({
      error: 'Failed to create database folder structure'
    }, { status: 500 });
  }
} 