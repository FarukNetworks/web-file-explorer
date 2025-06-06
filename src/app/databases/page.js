'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { DatabaseCard } from '@/components/DatabaseCard';

export default function Databases() {
  const router = useRouter();
  const [databases, setDatabases] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchDatabases();
  }, []);



  const fetchDatabases = async () => {
    try {
      const response = await fetch('/api/databases');
      const data = await response.json();

      if (data.error) {
        setError(data.error);
      } else {
        setDatabases(data.databases);
      }
    } catch (err) {
      setError('Failed to fetch databases');
      console.error('Error fetching databases:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDatabaseClick = (database) => {
    // Navigate to the database detail page
    router.push(`/databases/${database.name}`);
  };

  const handleDropDatabase = async (databaseName) => {
    if (window.confirm(`Are you sure you want to delete the database "${databaseName}"? This action cannot be undone.`)) {
      try {
        const response = await fetch(`/api/databases/delete`, {
          method: 'DELETE',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ name: databaseName }),
        });

        const result = await response.json();

        if (result.success) {
          // Refresh the databases list
          fetchDatabases();
        } else {
          setError(result.error || 'Failed to delete database');
        }
      } catch (err) {
        setError('Failed to delete database');
        console.error('Error deleting database:', err);
      }
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 p-8">
        <div className="max-w-6xl mx-auto">
          <div className="mb-8">
            <h1 className="text-3xl font-bold text-gray-900 mb-2">Database Navigator</h1>
            <p className="text-gray-600">Loading your databases...</p>
          </div>
          <div className="flex justify-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-6xl mx-auto">
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">Databases</h1>
        </div>

        {error && (
          <div className="bg-red-50 border border-red-200 rounded-lg p-4 mb-6">
            <p className="text-red-600">{error}</p>
          </div>
        )}

        <div className="bg-white rounded-lg shadow-md p-6">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-xl font-semibold text-gray-800 mb-4">Available Databases</h2>
            <button
              onClick={() => router.push('/databases/new')}
              className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 transition-colors"
            >
              Add Database
            </button>
          </div>


          {databases.length === 0 ? (
            <p className="text-gray-600">No databases found in the output folder.</p>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {databases.map((database, index) => (
                <DatabaseCard
                  key={index}
                  database={database}
                  onDropRequest={handleDropDatabase}
                  onOpen={handleDatabaseClick}
                />
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
} 