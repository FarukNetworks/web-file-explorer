using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.CreditScore
{
    /// <summary>
    /// Repository implementation for CreditScore operations
    /// </summary>
    public class CreditScoreRepository : Repository<sql2code.Models.CreditScore>, ICreditScoreRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the CreditScoreRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public CreditScoreRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for CreditScore if needed
    }
}
