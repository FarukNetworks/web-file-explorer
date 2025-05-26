import { useEffect, useRef } from 'react';
import mermaid from 'mermaid';

const MermaidDiagram = ({ chart }) => {
  const ref = useRef(null);

  useEffect(() => {
    // Initialize mermaid with configuration
    mermaid.initialize({
      startOnLoad: false,
      theme: 'default',
      securityLevel: 'loose',
      fontFamily: 'ui-sans-serif, system-ui, sans-serif',
      flowchart: {
        useMaxWidth: true,
        htmlLabels: true,
        curve: 'basis',
      },
      sequence: {
        useMaxWidth: true,
        wrap: true,
      },
      gantt: {
        useMaxWidth: true,
      },
      journey: {
        useMaxWidth: true,
      },
      timeline: {
        useMaxWidth: true,
      },
    });

    if (ref.current && chart) {
      // Clear previous content
      ref.current.innerHTML = '';

      // Generate unique ID for this diagram
      const id = `mermaid-${Math.random().toString(36).substr(2, 9)}`;

      try {
        // Render the mermaid diagram
        mermaid.render(id, chart).then(({ svg }) => {
          if (ref.current) {
            ref.current.innerHTML = svg;
          }
        }).catch((error) => {
          console.error('Mermaid rendering error:', error);
          if (ref.current) {
            ref.current.innerHTML = `
              <div class="p-4 bg-red-50 border border-red-200 rounded-md">
                <p class="text-red-700 text-sm font-medium">Error rendering diagram</p>
                <pre class="text-red-600 text-xs mt-2 whitespace-pre-wrap">${error.message}</pre>
              </div>
            `;
          }
        });
      } catch (error) {
        console.error('Mermaid error:', error);
        if (ref.current) {
          ref.current.innerHTML = `
            <div class="p-4 bg-red-50 border border-red-200 rounded-md">
              <p class="text-red-700 text-sm font-medium">Error rendering diagram</p>
              <pre class="text-red-600 text-xs mt-2 whitespace-pre-wrap">${error.message}</pre>
            </div>
          `;
        }
      }
    }
  }, [chart]);

  return (
    <div className="my-6 p-4 bg-gray-50 border border-gray-200 rounded-lg overflow-x-auto">
      <div ref={ref} className="mermaid-diagram flex justify-center" />
    </div>
  );
};

export default MermaidDiagram; 