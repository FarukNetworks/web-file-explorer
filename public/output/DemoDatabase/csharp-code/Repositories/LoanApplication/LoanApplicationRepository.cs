using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.LoanApplication
{
    /// <summary>
    /// Repository implementation for LoanApplication operations
    /// </summary>
    public class LoanApplicationRepository : Repository<sql2code.Models.LoanApplication>, ILoanApplicationRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the LoanApplicationRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public LoanApplicationRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for LoanApplication if needed
    }
}
