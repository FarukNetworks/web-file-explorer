import { useState } from 'react';
import { MoreHorizontal, HardDrive, FileText, ChevronRight } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardFooter } from '@/components/ui/card';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Badge } from '@/components/ui/badge';
import { formatDate } from '@/lib/file-helpers';

export function DatabaseCard({ database, onDropRequest, onOpen }) {
  const [menuOpen, setMenuOpen] = useState(false);

  const handleDrop = (e) => {
    e.stopPropagation();
    onDropRequest(database.name);
    setMenuOpen(false);
  };

  const handleOpen = () => {
    onOpen(database);
  };

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

  // Format last accessed date
  const lastAccessedFormatted = formatDate(database.lastAccessed);

  return (
    <Card className="overflow-hidden border border-primary-100 hover:shadow-lg transition-shadow">
      <CardContent className="p-5">
        <div className="flex items-start justify-between">
          <div>
            <h2 className="text-lg font-semibold">{database.name}</h2>
            <p className="text-primary-500 text-sm">Last accessed: {lastAccessedFormatted}</p>
          </div>
          <div className="flex items-center">
            <Badge variant={getBadgeVariant(database.environment)} className="mr-2 capitalize">
              {database.environment}
            </Badge>
            <DropdownMenu open={menuOpen} onOpenChange={setMenuOpen}>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="icon" className="text-primary-400 hover:text-primary-600">
                  <MoreHorizontal className="h-5 w-5" />
                </Button>
              </DropdownMenuTrigger>
              {menuOpen && (
                <DropdownMenuContent align="end">
                  <DropdownMenuItem onClick={handleOpen}>
                    Open
                  </DropdownMenuItem>
                  <DropdownMenuItem onClick={handleDrop} className="text-red-600">
                    Drop Database
                  </DropdownMenuItem>
                </DropdownMenuContent>
              )}
            </DropdownMenu>
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