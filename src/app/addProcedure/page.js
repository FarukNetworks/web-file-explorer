'use client';

import { useState, useEffect } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import { Clock, Folder, Play, Code } from 'lucide-react';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { oneLight } from 'react-syntax-highlighter/dist/esm/styles/prism';

export default function AddProcedurePage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const database = searchParams.get('database') || 'DemoDatabase';
  const [selectedProcedure, setSelectedProcedure] = useState(null);
  const [procedures, setProcedures] = useState([]);
  const [sqlContent, setSqlContent] = useState('');
  const [loadingSql, setLoadingSql] = useState(false);

  useEffect(() => {
    // Fetch available stored procedures
    fetchProcedures();
  }, [database]);

  const fetchProcedures = async () => {
    try {
      const response = await fetch(`/api/files?database=${database}`);
      const data = await response.json();

      // Extract stored procedure directories from the sql_raw directory
      const rootData = data.tree || data; // Handle both tree and direct data structures
      const sqlRawFolder = rootData.children?.find(child => child.name === 'sql_raw');
      if (sqlRawFolder && sqlRawFolder.children) {
        const procedureFolders = sqlRawFolder.children
          .filter(child => child.type === 'folder' && child.name.startsWith('dbo.usp_'))
          .map(folder => ({
            name: folder.name,
            lastModified: 'May 26, 2025, 02:21 PM', // Mock data for now
            type: 'Stored Procedure',
            path: folder.path,
            sqlPath: `${folder.path}/${folder.name}.sql` // Path to the actual SQL file
          }));
        setProcedures(procedureFolders);
      }
    } catch (error) {
      console.error('Error fetching procedures:', error);
    }
  };

  const handleProcedureSelect = (procedure) => {
    setSelectedProcedure(procedure);
    fetchSqlContent(procedure);
  };

  const fetchSqlContent = async (procedure) => {
    setLoadingSql(true);
    setSqlContent('');
    try {
      // Construct the path to the SQL file
      const sqlFilePath = `${procedure.path}/${procedure.name}.sql`;
      const response = await fetch(sqlFilePath);
      if (response.ok) {
        const content = await response.text();
        setSqlContent(content);
      } else {
        setSqlContent('-- Error: Could not load stored procedure content');
      }
    } catch (error) {
      console.error('Error fetching SQL content:', error);
      setSqlContent('-- Error: Could not load stored procedure content');
    } finally {
      setLoadingSql(false);
    }
  };

  const handleBeginAnalysis = () => {
    if (selectedProcedure) {
      // Navigate back to the database view with the selected procedure
      router.push(`/databases/${database}`);
    }
  };

  const formatProcedureName = (name) => {
    return name.replace('dbo.usp_', '');
  };

  return (
    <div className="h-screen flex flex-col bg-gray-50">
      {/* Top Header */}
      <div className="bg-white border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-4">
            <button
              onClick={() => router.push(`/databases/${database}`)}
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
        </div>
      </div>

      {/* Main Content */}
      <div className="flex-1 flex overflow-hidden">
        {/* Sidebar - Available Stored Procedures */}
        <div className="w-80 bg-white border-r border-gray-200 flex flex-col">
          <div className="p-4 border-b border-gray-200">
            <div className="flex items-center space-x-2 text-blue-600">
              <Folder className="w-5 h-5" />
              <h2 className="text-lg font-semibold">Available Stored Procedures</h2>
            </div>
          </div>

          <div className="flex-1 overflow-y-auto">
            {procedures.length === 0 ? (
              <div className="p-4 text-center text-gray-500">
                <p>Loading procedures...</p>
              </div>
            ) : (
              <div className="p-2">
                {procedures.map((procedure, index) => (
                  <div
                    key={index}
                    onClick={() => handleProcedureSelect(procedure)}
                    className={`p-3 mb-2 rounded-lg cursor-pointer transition-colors duration-150 ${selectedProcedure?.name === procedure.name
                      ? 'bg-blue-50 border border-blue-200'
                      : 'hover:bg-gray-50 border border-transparent'
                      }`}
                  >
                    <div className="flex items-start space-x-3">
                      <Folder className="w-5 h-5 text-blue-500 mt-0.5 flex-shrink-0" />
                      <div className="flex-1 min-w-0">
                        <h3 className="font-medium text-gray-900 truncate">
                          {formatProcedureName(procedure.name)}
                        </h3>
                        <div className="flex flex-col items-start space-x-4 mt-1 text-xs text-gray-500">
                          <div className="flex items-center space-x-1">
                            <Clock className="w-3 h-3" />
                            <span>{procedure.lastModified}</span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

        {/* Main Content Area */}
        <div className="flex-1 flex flex-col">
          {selectedProcedure ? (
            <div className="flex-1 flex flex-col">
              {/* Selected Procedure Header */}
              <div className="bg-white border-b border-gray-200 p-6">
                <div className="flex items-center justify-between">
                  <div>
                    <h1 className="text-2xl font-bold text-gray-900 mb-2">
                      {formatProcedureName(selectedProcedure.name)}
                    </h1>
                    <div className="flex items-center space-x-4 text-sm text-gray-500">
                      <div className="flex items-center space-x-1">
                        <Clock className="w-4 h-4" />
                        <span>Last Modified: {selectedProcedure.lastModified}</span>
                      </div>
                      <div className="flex items-center space-x-1">
                        <Folder className="w-4 h-4" />
                        <span>Type: {selectedProcedure.type}</span>
                      </div>
                    </div>
                    <div className="mt-2 text-sm text-gray-600">
                      <span>Path: {selectedProcedure.path}</span>
                    </div>
                  </div>
                </div>
              </div>

              {/* Analysis Options */}
              <div className="flex-1 bg-gray-50 p-6">
                <div className="w-[70%]">
                  <h2 className="text-lg font-semibold text-gray-900 mb-4">Analysis Options</h2>

                  <div className="bg-white rounded-lg border border-gray-200 p-6">
                    <button
                      onClick={handleBeginAnalysis}
                      className="flex items-center space-x-3 w-full p-4 text-left hover:bg-green-50 rounded-lg border border-green-200 transition-colors duration-150"
                    >
                      <div className="flex-shrink-0">
                        <Play className="w-6 h-6 text-green-600" />
                      </div>
                      <div>
                        <h3 className="font-medium text-gray-900">Begin Analysis</h3>
                        <p className="text-sm text-gray-500 mt-1">
                          Start analyzing the selected stored procedure and view its documentation
                        </p>
                      </div>
                    </button>
                  </div>

                  {/* Stored Procedure Definition */}
                  <div className="mt-6">
                    <div className="flex items-center space-x-2 mb-4">
                      <Code className="w-5 h-5 text-gray-600" />
                      <h2 className="text-lg font-semibold text-gray-900">Stored Procedure Definition</h2>
                    </div>

                    <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
                      {loadingSql ? (
                        <div className="p-6 text-center">
                          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mx-auto"></div>
                          <p className="text-gray-500 mt-2">Loading SQL content...</p>
                        </div>
                      ) : sqlContent ? (
                        <div className="relative">
                          <div className="bg-gray-50 px-4 py-2 border-b border-gray-200">
                            <span className="text-sm font-medium text-gray-700">
                              {selectedProcedure.name}.sql
                            </span>
                          </div>
                          <div className="max-h-96 overflow-y-auto">
                            <SyntaxHighlighter
                              language="sql"
                              style={oneLight}
                              showLineNumbers={true}
                              customStyle={{
                                margin: 0,
                                padding: '1rem',
                                fontSize: '14px',
                                lineHeight: '1.5',
                                background: 'transparent'
                              }}
                            >
                              {sqlContent}
                            </SyntaxHighlighter>
                          </div>
                        </div>
                      ) : (
                        <div className="p-6 text-center text-gray-500">
                          <Code className="w-12 h-12 text-gray-300 mx-auto mb-2" />
                          <p>SQL content will appear here when a procedure is selected</p>
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ) : (
            <div className="flex-1 flex items-center justify-center bg-gray-50">
              <div className="text-center max-w-md">
                <Folder className="w-16 h-16 text-gray-300 mx-auto mb-4" />
                <h2 className="text-xl font-semibold text-gray-900 mb-2">Select a Procedure</h2>
                <p className="text-gray-500">
                  Select a stored procedure to see details and analysis options
                </p>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
} 