using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.LoanProduct
{
    /// <summary>
    /// Repository implementation for LoanProduct operations
    /// </summary>
    public class LoanProductRepository : Repository<sql2code.Models.LoanProduct>, ILoanProductRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the LoanProductRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public LoanProductRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for LoanProduct if needed
    }
}
