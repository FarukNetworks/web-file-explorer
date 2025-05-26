using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.FraudFlag
{
    /// <summary>
    /// Repository implementation for FraudFlag operations
    /// </summary>
    public class FraudFlagRepository : Repository<sql2code.Models.FraudFlag>, IFraudFlagRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the FraudFlagRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public FraudFlagRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for FraudFlag if needed
    }
}
