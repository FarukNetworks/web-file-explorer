import { useState, useRef, useEffect } from 'react';
import { MoreHorizontal, HardDrive, FileText, ChevronRight } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardFooter } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';

export function DatabaseCard({ database, onDropRequest, onOpen }) {
  const [menuOpen, setMenuOpen] = useState(false);
  const dropdownRef = useRef(null);

  const handleDrop = (e) => {
    e.stopPropagation();
    onDropRequest(database.name);
    setMenuOpen(false);
  };

  const handleOpen = () => {
    onOpen(database);
    setMenuOpen(false);
  };

  const toggleMenu = (e) => {
    e.stopPropagation();
    setMenuOpen(!menuOpen);
  };

  // Close dropdown when clicking outside
  useEffect(() => {
    function handleClickOutside(event) {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setMenuOpen(false);
      }
    }

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Determine environment badge color
  const getBadgeVariant = (env) => {
    switch (env?.toLowerCase()) {
      case 'production':
        return 'success';
      case 'development':
        return 'default';
      case 'testing':
        return 'warning';
      default:
        return 'secondary';
    }
  };

  return (
    <Card className="overflow-hidden border border-primary-100 hover:shadow-lg transition-shadow">
      <CardContent className="p-5">
        <div className="flex items-start justify-between">
          <div>
            <h2 className="text-lg font-semibold">{database.name}</h2>
          </div>
          <div className="flex items-center">
            <Badge variant={getBadgeVariant(database.environment)} className="mr-2 capitalize">
              {database.environment}
            </Badge>
            <div className="relative" ref={dropdownRef}>
              <Button
                variant="ghost"
                size="icon"
                className="text-primary-400 hover:text-primary-600"
                onClick={toggleMenu}
              >
                <MoreHorizontal className="h-5 w-5" />
              </Button>
              {menuOpen && (
                <div className="absolute right-0 top-full mt-1 z-50 min-w-[8rem] overflow-hidden rounded-md bg-white p-1 shadow-md transition-all duration-300">
                  <div
                    className="relative flex cursor-pointer select-none items-center rounded-sm px-2 py-1.5 text-sm outline-none transition-colors hover:bg-gray-100"
                    onClick={handleOpen}
                  >
                    Open
                  </div>
                  <div
                    className="relative flex cursor-pointer select-none items-center rounded-sm px-2 py-1.5 text-sm outline-none transition-colors hover:bg-gray-100 text-red-600"
                    onClick={handleDrop}
                  >
                    Drop Database
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
        <div className="mt-4">
          <div className="flex items-center text-sm text-primary-600">
            <HardDrive className="h-4 w-4 mr-2" />
            <span>Size: {database.size}</span>
          </div>
          <div className="flex items-center text-sm text-primary-600 mt-1">
            <FileText className="h-4 w-4 mr-2" />
            <span>Files: {database.fileCount}</span>
          </div>
        </div>
      </CardContent>
      <CardFooter className="bg-primary-50 px-5 py-3 border-t border-primary-100">
        <Button
          variant="link"
          className="p-0 text-blue-500 hover:text-blue-700 text-sm font-medium flex items-center"
          onClick={handleOpen}
        >
          View Contents
          <ChevronRight className="h-4 w-4 ml-1" />
        </Button>
      </CardFooter>
    </Card>
  );
} 