import { NextResponse } from 'next/server';
import fs from 'fs';
import path from 'path';

export async function GET() {
  try {
    const outputPath = path.join(process.cwd(), 'public', 'output');

    // Check if output directory exists
    if (!fs.existsSync(outputPath)) {
      return NextResponse.json({ databases: [] });
    }

    // Read the output directory
    const items = fs.readdirSync(outputPath, { withFileTypes: true });

    // Filter only directories (databases)
    const databases = items
      .filter(item => item.isDirectory())
      .map(item => ({
        name: item.name,
        path: `/output/${item.name}` // Public path for client access
      }));

    return NextResponse.json({ databases });
  } catch (error) {
    console.error('Error reading output directory:', error);
    return NextResponse.json({ databases: [], error: 'Failed to read databases' });
  }
} 