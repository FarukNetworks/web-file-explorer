using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.PrivateRenamedObjectLog
{
    /// <summary>
    /// Repository implementation for PrivateRenamedObjectLog operations
    /// </summary>
    public class PrivateRenamedObjectLogRepository : Repository<sql2code.Models.PrivateRenamedObjectLog>, IPrivateRenamedObjectLogRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the PrivateRenamedObjectLogRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public PrivateRenamedObjectLogRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for PrivateRenamedObjectLog if needed
    }
}
