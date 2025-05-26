using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.Customer
{
    /// <summary>
    /// Repository implementation for Customer operations
    /// </summary>
    public class CustomerRepository : Repository<sql2code.Models.Customer>, ICustomerRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the CustomerRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public CustomerRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for Customer if needed
    }
}
