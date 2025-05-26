import { NextResponse } from 'next/server';
import fs from 'fs';
import path from 'path';

export async function DELETE(request) {
  try {
    const { name } = await request.json();

    // Validate required fields
    if (!name) {
      return NextResponse.json({ error: 'Database name is required' }, { status: 400 });
    }

    // Sanitize database name
    const sanitizedName = name.replace(/[^a-zA-Z0-9-_]/g, '_');

    // Create the database folder path
    const outputDir = path.join(process.cwd(), 'public', 'output');
    const databaseDir = path.join(outputDir, sanitizedName);

    // Check if database exists
    if (!fs.existsSync(databaseDir)) {
      return NextResponse.json({ error: 'Database not found' }, { status: 404 });
    }

    // Recursively delete the database directory
    function deleteDirectory(dirPath) {
      if (fs.existsSync(dirPath)) {
        const files = fs.readdirSync(dirPath);

        files.forEach(file => {
          const filePath = path.join(dirPath, file);
          const stat = fs.statSync(filePath);

          if (stat.isDirectory()) {
            deleteDirectory(filePath);
          } else {
            fs.unlinkSync(filePath);
          }
        });

        fs.rmdirSync(dirPath);
      }
    }

    deleteDirectory(databaseDir);

    return NextResponse.json({
      success: true,
      message: 'Database deleted successfully',
      database: {
        name: sanitizedName,
        originalName: name
      }
    });

  } catch (error) {
    console.error('Error deleting database:', error);
    return NextResponse.json({
      error: 'Failed to delete database'
    }, { status: 500 });
  }
} 