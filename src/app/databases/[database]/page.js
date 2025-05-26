'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import Sidebar from '../../../components/Sidebar';
import FileViewer from '../../../components/FileViewer';

export default function DatabasePage() {
  const params = useParams();
  const router = useRouter();
  const database = params.database;
  const [selectedFile, setSelectedFile] = useState(null);

  const handleFileSelect = (file) => {
    setSelectedFile(file);
  };

  return (
    <div className="h-screen flex flex-col bg-gray-50">
      {/* Top Header */}
      <div className="bg-white border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-4">
            <button
              onClick={() => router.push('/databases')}
              className="flex items-center space-x-2 px-3 py-2 text-sm text-gray-600 hover:text-gray-900 hover:bg-gray-100 rounded"
            >
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
              </svg>
              <span>Back</span>
            </button>
            <h1 className="text-2xl font-bold text-gray-900">SQL2CODE</h1>
            <div className="flex items-center space-x-2 text-sm text-gray-500">
              <span>{database}</span>
              <span className="px-2 py-1 bg-blue-100 text-blue-700 rounded text-xs font-medium">
                Development
              </span>
            </div>
          </div>
          <div className="flex items-center space-x-3">
            <button className="flex items-center space-x-2 px-3 py-2 text-sm text-gray-600 hover:text-gray-900 hover:bg-gray-100 rounded">
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
              <span>Refresh</span>
            </button>
            <button
              onClick={() => router.push(`/addProcedure?database=${database}`)}
              className="flex items-center space-x-2 px-3 py-2 text-sm bg-blue-600 text-white hover:bg-blue-700 rounded"
            >
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
              </svg>
              <span>Add New Stored Procedure</span>
            </button>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="flex-1 flex overflow-hidden">
        <div className="sticky top-0 h-screen overflow-y-auto">
          <Sidebar
            database={database}
            isCollapsed={false}
            onToggle={() => { }}
            onFileSelect={handleFileSelect}
            selectedFile={selectedFile}
          />
        </div>
        <div className="flex-1 overflow-y-auto">
          <FileViewer selectedFile={selectedFile} />
        </div>
      </div>
    </div>
  );
} 