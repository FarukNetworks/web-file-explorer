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
            'flex items-center w-full p-1 text-sm rounded-md transition-colors',
            isDirectory
              ? 'hover:bg-primary-100 font-medium mb-1'
              : 'hover:bg-primary-100',
            isSelected && !isDirectory && 'bg-primary-100'
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
                <ChevronDown className="h-4 w-4 mr-1 text-primary-500" />
              ) : (
                <ChevronRight className="h-4 w-4 mr-1 text-primary-500" />
              )}
              <FolderIcon className="h-4 w-4 mr-[.3rem] text-yellow-500" />
            </>
          ) : (
            <FileTypeIcon fileName={node.name} className="mr-[.3rem] w-4 h-4" />
          )}
          <span className="w-full overflow-hidden text-left">{node.name}</span>
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

  if (isCollapsed) {
    return (
      <div className="w-12 bg-gray-50 border-r border-gray-200 flex flex-col">
        <button
          onClick={onToggle}
          className="p-3 hover:bg-gray-100 border-b border-gray-200"
        >
          <svg className="w-5 h-5 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        </button>
      </div>
    );
  }

  return (
    <div className="w-[30%] border-r border-primary-200 bg-white overflow-y-auto h-screen">
      <div className="p-4">
        <div className="flex items-center justify-between mb-2">
          <h2 className="text-sm font-semibold text-primary-900">Files</h2>
          <button
            onClick={onToggle}
            className="p-1 hover:bg-gray-100 rounded"
          >
            <svg className="w-4 h-4 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
            </svg>
          </button>
        </div>

        <div className="relative mb-4">
          <Search className="h-4 w-4 absolute left-2.5 top-2.5 text-primary-500" />
          <Input
            placeholder="Search files..."
            value={searchQuery}
            onChange={e => setSearchQuery(e.target.value)}
            className="pl-8 h-9 text-sm w-full"
          />
        </div>

        <div className="mt-4 space-y-1">
          {loading ? (
            <div className="flex items-center justify-center py-8">
              <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-primary-500"></div>
            </div>
          ) : (
            renderFileTree(filteredFiles)
          )}
        </div>
      </div>
    </div>
  );
} 