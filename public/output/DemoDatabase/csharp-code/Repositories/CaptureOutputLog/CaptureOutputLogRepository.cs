using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.CaptureOutputLog
{
    /// <summary>
    /// Repository implementation for CaptureOutputLog operations
    /// </summary>
    public class CaptureOutputLogRepository : Repository<sql2code.Models.CaptureOutputLog>, ICaptureOutputLogRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the CaptureOutputLogRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public CaptureOutputLogRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for CaptureOutputLog if needed
    }
}
