using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.Address
{
    /// <summary>
    /// Repository implementation for Address operations
    /// </summary>
    public class AddressRepository : Repository<sql2code.Models.Address>, IAddressRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the AddressRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public AddressRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for Address if needed
    }
}
