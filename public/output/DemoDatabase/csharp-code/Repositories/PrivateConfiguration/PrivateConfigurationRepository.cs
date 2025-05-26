using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.PrivateConfiguration
{
    /// <summary>
    /// Repository implementation for PrivateConfiguration operations
    /// </summary>
    public class PrivateConfigurationRepository : Repository<sql2code.Models.PrivateConfiguration>, IPrivateConfigurationRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the PrivateConfigurationRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public PrivateConfigurationRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for PrivateConfiguration if needed
    }
}
