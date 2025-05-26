using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.PrivateResult
{
    /// <summary>
    /// Repository implementation for PrivateResult operations
    /// </summary>
    public class PrivateResultRepository : KeylessRepository<sql2code.Models.PrivateResult>, IPrivateResultRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        // This is a keyless entity, so some operations that require a primary key may not work
        // Method overrides have been implemented to handle the keyless nature of this entity
        
        
        /// <summary>
        /// Initializes a new instance of the PrivateResultRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public PrivateResultRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for PrivateResult if needed
    }
}
