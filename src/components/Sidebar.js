'use client';

import { useState, useEffect } from 'react';
import { Search, FolderIcon, ChevronDown, ChevronRight } from 'lucide-react';
import { FileTypeIcon } from '@/components/ui/file-icon';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { cn } from '@/lib/utils';

export default function Sidebar({ database, isCollapsed, onToggle, onFileSelect, selectedFile }) {
  const [expandedDirs, setExpandedDirs] = useState({});
  const [searchQuery, setSearchQuery] = useState('');
  const [fileTree, setFileTree] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (database) {
      fetchFileTree();
    }
  }, [database]);

  const fetchFileTree = async () => {
    try {
      const response = await fetch(`/api/files?database=${database}`);
      const data = await response.json();
      setFileTree(data.tree);

      // Auto-expand first 2 levels
      const autoExpanded = {};
      const expandNode = (node, level = 0) => {
        if (node.type === 'folder' && level < 2) {
          autoExpanded[node.id || node.path] = true;
          if (node.children) {
            node.children.forEach(child => expandNode(child, level + 1));
          }
        }
      };
      if (data.tree) {
        expandNode(data.tree);
      }
      setExpandedDirs(autoExpanded);
    } catch (error) {
      console.error('Error fetching file tree:', error);
    } finally {
      setLoading(false);
    }
  };

  const toggleDirectory = (nodeId) => {
    setExpandedDirs(prev => ({
      ...prev,
      [nodeId]: !prev[nodeId],
    }));
  };

  const filterFiles = (query, nodes) => {
    if (!query) return nodes;
    if (!Array.isArray(nodes)) return [];

    return nodes.filter(node => {
      // Check if the current node matches
      const nodeMatches = node.name.toLowerCase().includes(query.toLowerCase());

      // For directories, also check children
      if (node.type === 'folder' && node.children) {
        const filteredChildren = filterFiles(query, node.children);
        // If any children match, this directory should be included
        if (filteredChildren.length > 0) {
          // Return a new node with only the matching children
          return {
            ...node,
            children: filteredChildren,
          };
        }
      }

      return nodeMatches;
    });
  };

  const renderFileNode = (node, depth = 0) => {
    if (!node) return null;

    const isDirectory = node.type === 'folder';
    const nodeId = node.id || node.path || node.name;
    const isExpanded = expandedDirs[nodeId];
    const isSelected = selectedFile?.path === node.path;

    const paddingLeft = depth * 1 + 'rem';

    return (
      <div key={nodeId}>
        <button
          className={cn(
            'flex items-center w-full py-1.5 px-2 text-sm rounded-md transition-colors text-left',
            isDirectory
              ? 'hover:bg-gray-100 font-medium text-gray-700'
              : 'hover:bg-gray-100 text-gray-600',
            isSelected && !isDirectory && 'bg-blue-50 text-blue-700 border-l-2 border-blue-500'
          )}
          onClick={() => {
            if (isDirectory) {
              toggleDirectory(nodeId);
            } else {
              onFileSelect(node);
            }
          }}
          style={{ paddingLeft }}
        >
          {isDirectory ? (
            <>
              {isExpanded ? (
                <ChevronDown className="h-4 w-4 mr-2 text-gray-500" />
              ) : (
                <ChevronRight className="h-4 w-4 mr-2 text-gray-500" />
              )}
              <FolderIcon className="h-4 w-4 mr-2 text-blue-500" />
            </>
          ) : (
            <FileTypeIcon fileName={node.name} className="mr-2 w-4 h-4 ml-6" />
          )}
          <span className="truncate">{node.name}</span>
        </button>

        {isDirectory && isExpanded && node.children && (
          <div className="w-full overflow-hidden text-left">
            {node.children.map(child => renderFileNode(child, depth + 1))}
          </div>
        )}
      </div>
    );
  };

  const renderFileTree = (nodes) => {
    if (!nodes) return null;
    if (Array.isArray(nodes)) {
      return nodes.map(node => renderFileNode(node));
    }
    return renderFileNode(nodes);
  };

  const filteredFiles = fileTree ? filterFiles(searchQuery, Array.isArray(fileTree) ? fileTree : [fileTree]) : [];

  // Always show expanded sidebar
  if (isCollapsed) {
    // Don't render collapsed state, always show expanded
  }

  return (
    <div className="w-full border-r border-gray-200 bg-white overflow-y-auto h-full flex-shrink-0">
      <div className="p-4">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-semibold text-gray-900">Files</h2>
        </div>

        <div className="relative mb-6">
          <Search className="h-4 w-4 absolute left-3 top-3 text-gray-400" />
          <Input
            placeholder="Search files..."
            value={searchQuery}
            onChange={e => setSearchQuery(e.target.value)}
            className="pl-10 h-10 text-sm w-full border-gray-300 rounded-md focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
          />
        </div>

        <div className="space-y-0.5">
          {loading ? (
            <div className="flex items-center justify-center py-8">
              <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-blue-500"></div>
            </div>
          ) : (
            renderFileTree(filteredFiles)
          )}
        </div>
      </div>
    </div>
  );
} 