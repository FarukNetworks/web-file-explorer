using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.Loan
{
    /// <summary>
    /// Repository implementation for Loan operations
    /// </summary>
    public class LoanRepository : Repository<sql2code.Models.Loan>, ILoanRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the LoanRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public LoanRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for Loan if needed
    }
}
