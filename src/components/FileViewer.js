import { useState, useEffect } from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { oneLight } from 'react-syntax-highlighter/dist/esm/styles/prism';
import { Copy, Download, Eye, Monitor, SquarePen, FileText, Maximize } from 'lucide-react';
import MermaidDiagram from './MermaidDiagram';

const FileViewer = ({ selectedFile }) => {
  const [fileContent, setFileContent] = useState('');
  const [loading, setLoading] = useState(false);
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    if (selectedFile) {
      fetchFileContent();
    }
  }, [selectedFile]);

  const fetchFileContent = async () => {
    setLoading(true);
    try {
      const response = await fetch(selectedFile.path);
      const content = await response.text();
      setFileContent(content);
    } catch (error) {
      console.error('Error fetching file content:', error);
      setFileContent('Error loading file content');
    } finally {
      setLoading(false);
    }
  };

  const getLanguage = (filename) => {
    if (filename.endsWith('.md')) return 'markdown';
    if (filename.endsWith('.json')) return 'json';
    if (filename.endsWith('.sql')) return 'sql';
    if (filename.endsWith('.cs')) return 'csharp';
    if (filename.endsWith('.js')) return 'javascript';
    if (filename.endsWith('.ts')) return 'typescript';
    if (filename.endsWith('.py')) return 'python';
    if (filename.endsWith('.xml')) return 'xml';
    if (filename.endsWith('.html')) return 'html';
    if (filename.endsWith('.css')) return 'css';
    return 'text';
  };

  const getFileSize = (content) => {
    const bytes = new Blob([content]).size;
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  };

  const getLineCount = (content) => {
    return content.split('\n').length;
  };

  const copyToClipboard = async () => {
    try {
      await navigator.clipboard.writeText(fileContent);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error('Failed to copy text: ', err);
    }
  };

  const downloadFile = () => {
    const blob = new Blob([fileContent], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = selectedFile.name;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  };

  const isMarkdown = (filename) => filename.endsWith('.md');

  if (!selectedFile) {
    return (
      <div className="flex-1 flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <svg className="w-16 h-16 text-gray-300 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
          <h3 className="text-lg font-medium text-gray-900 mb-2">Select a file to view its contents</h3>
          <p className="text-gray-500">Choose a file from the sidebar to display its contents here with beautiful syntax highlighting and enhanced markdown rendering</p>
        </div>
      </div>
    );
  }

  return (
    <div className="w-full py-5 flex flex-col bg-white">
      {/* File Header */}
      <div className="bg-card text-card-foreground mb-4 bg-gradient-to-r from-white via-blue-50/30 to-purple-50/30 rounded-xl shadow-lg border-0 backdrop-blur-sm mx-5">
        <div className="flex space-y-1.5 p-4 flex-row items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="relative">
              <div className="absolute inset-0 bg-gradient-to-r from-blue-400 to-purple-400 rounded-lg blur-sm opacity-20"></div>
              <FileText className="text-purple-500 relative z-10 w-4 h-4" />
            </div>
            <div className="flex flex-col">
              <span className="text-sm font-bold text-gray-800">{selectedFile.name}</span>
              <span className="text-xs text-gray-500">{selectedFile.path}</span>
            </div>
            {isMarkdown(selectedFile.name) && (
              <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 bg-secondary hover:bg-secondary/80 ml-2 bg-gradient-to-r from-purple-100 to-blue-100 text-purple-800 border-purple-200">
                MARKDOWN
              </div>
            )}
          </div>
          <div className="flex items-center space-x-2">
            <button className="inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0 h-9 rounded-md px-3 text-primary-600 hover:text-primary-700 hover:bg-primary-100 transition-all duration-200">
              <Monitor className="h-4 w-4 mr-1" />
              Dark
            </button>
            <button
              onClick={downloadFile}
              className="inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0 h-9 rounded-md px-3 text-primary-600 hover:text-primary-700 hover:bg-primary-100 transition-all duration-200"
            >
              <Download className="h-4 w-4 mr-1" />
              Download
            </button>
            <button className="inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0 h-9 rounded-md px-3 text-primary-600 hover:text-primary-700 hover:bg-primary-100 transition-all duration-200">
              <SquarePen className="h-4 w-4 mr-1" />
              Edit
            </button>
          </div>
        </div>
      </div>

      {/* File Content Header */}
      <div className="flex space-y-1.5 p-6 border-b border-gradient-to-r px-9 py-3 bg-gradient-to-r from-blue-50/50 via-purple-50/50 to-pink-50/50 flex-row justify-between items-center">
        <div className="flex items-center gap-3">
          <div className="text-lg font-bold text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-purple-600">File Content</div>
          {!loading && fileContent && (
            <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 bg-white/50 text-gray-700 border-gray-300">
              {getLineCount(fileContent)} lines • {getFileSize(fileContent)}
            </div>
          )}
        </div>
        <button className="inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0 rounded-md text-primary-600 hover:text-primary-700 hover:bg-white/80 h-8 w-8 p-0 transition-all duration-200">
          <Maximize className="h-4 w-4" />
        </button>
      </div>

      {/* File Content */}
      <div className="flex-1 overflow-auto">
        {loading ? (
          <div className="flex items-center justify-center h-64">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-purple-600"></div>
          </div>
        ) : (
          <div className="relative">
            {isMarkdown(selectedFile.name) && (
              <div className="sticky top-0 z-10 flex items-center justify-between p-3 bg-gradient-to-r from-purple-50 via-blue-50 to-indigo-50 border-b border-purple-200 backdrop-blur-sm shadow-sm px-9">
                <div className="flex items-center gap-2">
                  <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 bg-secondary hover:bg-secondary/80 bg-gradient-to-r from-purple-100 to-blue-100 text-purple-800 border-purple-200 shadow-sm">
                    📝 Markdown
                  </div>
                  <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 bg-white/70 text-xs text-green-700 border-green-300">
                    ✓ GitHub Features
                  </div>
                  <span className="text-sm text-gray-600 font-medium">{getLineCount(fileContent)} lines</span>
                </div>
                <div className="flex items-center gap-2">
                  <button
                    onClick={copyToClipboard}
                    className="inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0 rounded-md h-8 px-3 text-purple-600 hover:text-purple-700 hover:bg-purple-100 transition-all duration-200"
                  >
                    <Copy className="h-4 w-4 mr-1" />
                    {copied ? 'Copied!' : 'Copy'}
                  </button>

                </div>
              </div>
            )}
            <div className="p-6">
              {isMarkdown(selectedFile.name) ? (
                <div className="prose prose-lg max-w-none">
                  <ReactMarkdown
                    remarkPlugins={[remarkGfm]}
                    components={{
                      code({ node, inline, className, children, ...props }) {
                        const match = /language-(\w+)/.exec(className || '');
                        const language = match ? match[1] : '';
                        const code = String(children).replace(/\n$/, '');

                        // Check if it's a Mermaid diagram
                        if (!inline && (language === 'mermaid' || code.trim().startsWith('flowchart') || code.trim().startsWith('graph') || code.trim().startsWith('sequenceDiagram') || code.trim().startsWith('gantt') || code.trim().startsWith('pie') || code.trim().startsWith('journey') || code.trim().startsWith('gitGraph'))) {
                          return <MermaidDiagram chart={code} />;
                        }

                        return !inline && match ? (
                          <SyntaxHighlighter
                            style={oneLight}
                            language={language}
                            PreTag="div"
                            className="rounded-md"
                            {...props}
                          >
                            {code}
                          </SyntaxHighlighter>
                        ) : (
                          <code className={className} {...props}>
                            {children}
                          </code>
                        );
                      },
                      h1: ({ children }) => (
                        <h1 className="text-3xl font-bold text-purple-600 mb-6 pb-2 border-b border-gray-200">
                          {children}
                        </h1>
                      ),
                      h2: ({ children }) => (
                        <h2 className="text-2xl font-bold text-purple-600 mt-8 mb-4">
                          {children}
                        </h2>
                      ),
                      h3: ({ children }) => (
                        <h3 className="text-xl font-semibold text-gray-800 mt-6 mb-3">
                          {children}
                        </h3>
                      ),
                      p: ({ children }) => (
                        <p className="text-gray-700 leading-relaxed mb-4">
                          {children}
                        </p>
                      ),
                      ul: ({ children }) => (
                        <ul className="list-disc list-inside text-gray-700 mb-4 space-y-1">
                          {children}
                        </ul>
                      ),
                      ol: ({ children }) => (
                        <ol className="list-decimal list-inside text-gray-700 mb-4 space-y-1">
                          {children}
                        </ol>
                      ),
                      blockquote: ({ children }) => (
                        <blockquote className="border-l-4 border-purple-200 pl-4 italic text-gray-600 my-4">
                          {children}
                        </blockquote>
                      ),
                      table: ({ children }) => (
                        <div className="overflow-x-auto mb-6">
                          <table className="min-w-full border border-purple-200 rounded-lg shadow-sm">
                            {children}
                          </table>
                        </div>
                      ),
                      thead: ({ children }) => (
                        <thead className="bg-purple-100 ">
                          {children}
                        </thead>
                      ),
                      tbody: ({ children }) => (
                        <tbody className="bg-gray-50 divide-y divide-gray-100">
                          {children}
                        </tbody>
                      ),
                      tr: ({ children, ...props }) => {
                        // Check if this is a header row by looking at the parent
                        const isHeaderRow = props.node?.tagName === 'tr' && props.node?.parent?.tagName === 'thead';
                        if (isHeaderRow) {
                          return <tr className="border-b border-purple-500">{children}</tr>;
                        }
                        return <tr className=" transition-colors duration-150">{children}</tr>;
                      },
                      th: ({ children }) => (
                        <th className="px-6 py-4 text-left font-semibold text-gray-800 text-sm uppercase tracking-wide">
                          {children}
                        </th>
                      ),
                      td: ({ children }) => (
                        <td className="px-6 py-4 text-sm text-gray-700 border-b border-gray-100">
                          {children}
                        </td>
                      ),
                    }}
                  >
                    {fileContent}
                  </ReactMarkdown>
                </div>
              ) : (
                <SyntaxHighlighter
                  language={getLanguage(selectedFile.name)}
                  style={oneLight}
                  showLineNumbers={true}
                  className="rounded-md"
                  customStyle={{
                    margin: 0,
                    padding: '1rem',
                    fontSize: '14px',
                    lineHeight: '1.5',
                  }}
                >
                  {fileContent}
                </SyntaxHighlighter>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default FileViewer; 