using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.Collateral
{
    /// <summary>
    /// Repository implementation for Collateral operations
    /// </summary>
    public class CollateralRepository : Repository<sql2code.Models.Collateral>, ICollateralRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the CollateralRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public CollateralRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for Collateral if needed
    }
}
