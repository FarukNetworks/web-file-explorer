using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.EmploymentHistory
{
    /// <summary>
    /// Repository implementation for EmploymentHistory operations
    /// </summary>
    public class EmploymentHistoryRepository : Repository<sql2code.Models.EmploymentHistory>, IEmploymentHistoryRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the EmploymentHistoryRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public EmploymentHistoryRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for EmploymentHistory if needed
    }
}
