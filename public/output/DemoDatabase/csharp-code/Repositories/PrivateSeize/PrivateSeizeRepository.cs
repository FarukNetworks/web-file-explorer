using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.PrivateSeize
{
    /// <summary>
    /// Repository implementation for PrivateSeize operations
    /// </summary>
    public class PrivateSeizeRepository : Repository<sql2code.Models.PrivateSeize>, IPrivateSeizeRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the PrivateSeizeRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public PrivateSeizeRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for PrivateSeize if needed
    }
}
