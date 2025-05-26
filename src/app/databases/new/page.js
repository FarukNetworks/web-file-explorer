'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { ChevronLeft, ChevronDown } from 'lucide-react';

export default function NewDatabase() {
  const router = useRouter();
  const [formData, setFormData] = useState({
    name: '',
    type: 'Microsoft SQL Server',
    environment: 'Development',
    connectionString: '',
    enableSSL: false,
    enableAutoBackup: false
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const databaseTypes = ['Microsoft SQL Server', 'PostgreSQL', 'MySQL', 'Oracle', 'MongoDB'];

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const response = await fetch('/api/databases/create', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      });

      const result = await response.json();

      if (result.success) {
        // Navigate back to databases list
        router.push('/databases');
      } else {
        setError(result.error || 'Failed to create database');
      }
    } catch (err) {
      setError('Failed to create database');
      console.error('Error creating database:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    router.push('/databases');
  };

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-2xl mx-auto">
        {/* Header */}
        <div className="mb-8">
          <button
            onClick={() => router.push('/databases')}
            className="flex items-center text-blue-600 hover:text-blue-700 mb-4 transition-colors"
          >
            <ChevronLeft className="w-4 h-4 mr-1" />
            Back to Databases
          </button>
        </div>

        {/* Form */}
        <div className="bg-white rounded-lg shadow-md p-8">
          <h1 className="text-2xl font-bold text-gray-900 mb-8">Add New Database</h1>

          {error && (
            <div className="bg-red-50 border border-red-200 rounded-lg p-4 mb-6">
              <p className="text-red-600">{error}</p>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Database Name */}
            <div>
              <label htmlFor="name" className="block text-sm font-medium text-gray-700 mb-2">
                Database Name
              </label>
              <input
                type="text"
                id="name"
                name="name"
                value={formData.name}
                onChange={handleInputChange}
                placeholder="Enter database name"
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>

            {/* Database Type */}
            <div>
              <label htmlFor="type" className="block text-sm font-medium text-gray-700 mb-2">
                Database Type
              </label>
              <div className="relative">
                <select
                  id="type"
                  name="type"
                  value={formData.type}
                  onChange={handleInputChange}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent appearance-none bg-white"
                >
                  {databaseTypes.map(type => (
                    <option key={type} value={type}>{type}</option>
                  ))}
                </select>
                <ChevronDown className="absolute right-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400 pointer-events-none" />
              </div>
            </div>

            {/* Environment */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Environment
              </label>
              <div className="flex space-x-6">
                {['Production', 'Development', 'Testing'].map(env => (
                  <label key={env} className="flex items-center">
                    <input
                      type="radio"
                      name="environment"
                      value={env}
                      checked={formData.environment === env}
                      onChange={handleInputChange}
                      className="w-4 h-4 text-blue-600 border-gray-300 focus:ring-blue-500"
                    />
                    <span className="ml-2 text-sm text-gray-700">{env}</span>
                  </label>
                ))}
              </div>
            </div>

            {/* Connection String */}
            <div>
              <label htmlFor="connectionString" className="block text-sm font-medium text-gray-700 mb-2">
                Connection String
              </label>
              <input
                type="text"
                id="connectionString"
                name="connectionString"
                value={formData.connectionString}
                onChange={handleInputChange}
                placeholder="Enter connection string"
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />

            </div>

            {/* Advanced Options */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-3">
                Advanced Options
              </label>
              <div className="space-y-3">
                <label className="flex items-center">
                  <input
                    type="checkbox"
                    name="enableSSL"
                    checked={formData.enableSSL}
                    onChange={handleInputChange}
                    className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                  />
                  <span className="ml-2 text-sm text-gray-700">Enable SSL/TLS</span>
                </label>
                <label className="flex items-center">
                  <input
                    type="checkbox"
                    name="enableAutoBackup"
                    checked={formData.enableAutoBackup}
                    onChange={handleInputChange}
                    className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                  />
                  <span className="ml-2 text-sm text-gray-700">Enable Auto Backup</span>
                </label>
              </div>
            </div>

            {/* Form Actions */}
            <div className="flex justify-end space-x-4 pt-6">
              <button
                type="button"
                onClick={handleCancel}
                className="px-4 py-2 text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200 transition-colors"
              >
                Cancel
              </button>
              <button
                type="submit"
                disabled={loading || !formData.name}
                className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
              >
                {loading ? 'Creating...' : 'Add Database'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
} 