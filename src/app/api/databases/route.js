import { NextResponse } from 'next/server';
import fs from 'fs';
import path from 'path';

// Helper function to calculate directory size and file count
function calculateDirectoryStats(dirPath) {
  let size = 0;
  let fileCount = 0;

  function traverse(currentPath) {
    try {
      const items = fs.readdirSync(currentPath, { withFileTypes: true });

      for (const item of items) {
        const itemPath = path.join(currentPath, item.name);

        if (item.isDirectory()) {
          traverse(itemPath);
        } else {
          const stats = fs.statSync(itemPath);
          size += stats.size;
          fileCount++;
        }
      }
    } catch (error) {
      console.warn(`Error reading directory ${currentPath}:`, error);
    }
  }

  traverse(dirPath);
  return { size, fileCount };
}

// Helper function to format bytes
function formatBytes(bytes) {
  if (bytes === 0) return '0 B';

  const k = 1024;
  const sizes = ['B', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));

  return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
}

// Helper function to format date
function formatDate(date) {
  const now = new Date();
  const target = new Date(date);
  const diffTime = Math.abs(now - target);
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

  if (diffDays === 1) {
    return `Today, ${target.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    })}`;
  } else if (diffDays === 2) {
    return `Yesterday, ${target.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    })}`;
  } else {
    return target.toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }
}

export async function GET() {
  try {
    const outputPath = path.join(process.cwd(), 'public', 'output');

    // Check if output directory exists
    if (!fs.existsSync(outputPath)) {
      return NextResponse.json({ databases: [] });
    }

    // Read the output directory
    const items = fs.readdirSync(outputPath, { withFileTypes: true });

    // Filter only directories (databases) and get detailed info
    const databases = items
      .filter(item => item.isDirectory())
      .map(item => {
        const dbPath = path.join(outputPath, item.name);
        const stats = fs.statSync(dbPath);

        // Get database config if it exists
        let config = {};
        const configPath = path.join(dbPath, 'database.json');
        if (fs.existsSync(configPath)) {
          try {
            config = JSON.parse(fs.readFileSync(configPath, 'utf8'));
          } catch (e) {
            console.warn(`Failed to parse config for ${item.name}:`, e);
          }
        }

        // Calculate directory size and file count
        const { size, fileCount } = calculateDirectoryStats(dbPath);

        return {
          name: item.name,
          path: `/output/${item.name}`,
          size: formatBytes(size),
          fileCount,
          environment: config.environment || 'Development',
          lastAccessed: formatDate(stats.mtime),
          type: config.type || 'Unknown',
          createdAt: config.createdAt || stats.birthtime.toISOString()
        };
      });

    return NextResponse.json({ databases });
  } catch (error) {
    console.error('Error reading output directory:', error);
    return NextResponse.json({ databases: [], error: 'Failed to read databases' });
  }
} 