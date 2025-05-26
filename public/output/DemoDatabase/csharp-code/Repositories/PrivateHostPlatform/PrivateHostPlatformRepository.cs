using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.PrivateHostPlatform
{
    /// <summary>
    /// Repository implementation for PrivateHostPlatform operations
    /// </summary>
    public class PrivateHostPlatformRepository : KeylessRepository<sql2code.Models.PrivateHostPlatform>, IPrivateHostPlatformRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        // This is a keyless entity, so some operations that require a primary key may not work
        // Method overrides have been implemented to handle the keyless nature of this entity
        
        
        /// <summary>
        /// Initializes a new instance of the PrivateHostPlatformRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public PrivateHostPlatformRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for PrivateHostPlatform if needed
    }
}
