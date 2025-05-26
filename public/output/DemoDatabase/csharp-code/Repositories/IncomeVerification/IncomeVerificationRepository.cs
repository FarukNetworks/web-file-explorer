using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.IncomeVerification
{
    /// <summary>
    /// Repository implementation for IncomeVerification operations
    /// </summary>
    public class IncomeVerificationRepository : Repository<sql2code.Models.IncomeVerification>, IIncomeVerificationRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the IncomeVerificationRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public IncomeVerificationRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for IncomeVerification if needed
    }
}
