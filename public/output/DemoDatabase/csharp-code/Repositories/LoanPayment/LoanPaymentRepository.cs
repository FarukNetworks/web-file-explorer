using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.LoanPayment
{
    /// <summary>
    /// Repository implementation for LoanPayment operations
    /// </summary>
    public class LoanPaymentRepository : Repository<sql2code.Models.LoanPayment>, ILoanPaymentRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the LoanPaymentRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public LoanPaymentRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for LoanPayment if needed
    }
}
