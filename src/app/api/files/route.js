import { NextResponse } from 'next/server';
import fs from 'fs';
import path from 'path';

function buildFileTree(dirPath, basePath = '') {
  try {
    const items = fs.readdirSync(dirPath, { withFileTypes: true });

    const tree = {
      name: path.basename(dirPath),
      type: 'folder',
      path: basePath,
      children: []
    };

    items.forEach(item => {
      const itemPath = path.join(dirPath, item.name);
      const relativePath = path.join(basePath, item.name);

      if (item.isDirectory()) {
        // Recursively build tree for subdirectories
        const subTree = buildFileTree(itemPath, relativePath);
        tree.children.push(subTree);
      } else {
        // Add file to tree
        tree.children.push({
          name: item.name,
          type: 'file',
          path: relativePath
        });
      }
    });

    // Sort children: folders first, then files, both alphabetically
    tree.children.sort((a, b) => {
      if (a.type !== b.type) {
        return a.type === 'folder' ? -1 : 1;
      }
      return a.name.localeCompare(b.name);
    });

    return tree;
  } catch (error) {
    console.error('Error building file tree:', error);
    return null;
  }
}

export async function GET(request) {
  try {
    const { searchParams } = new URL(request.url);
    const database = searchParams.get('database');

    if (!database) {
      return NextResponse.json({ error: 'Database parameter is required' }, { status: 400 });
    }

    const databasePath = path.join(process.cwd(), 'public', 'output', database);

    // Check if database directory exists
    if (!fs.existsSync(databasePath)) {
      return NextResponse.json({ error: 'Database not found' }, { status: 404 });
    }

    const tree = buildFileTree(databasePath, `/output/${database}`);

    return NextResponse.json({ tree });
  } catch (error) {
    console.error('Error fetching file tree:', error);
    return NextResponse.json({ error: 'Failed to fetch file tree' }, { status: 500 });
  }
} 