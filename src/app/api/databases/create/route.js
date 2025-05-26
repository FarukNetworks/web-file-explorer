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

    // Create database configuration file (minimal metadata only)
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