using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.PrivateSysType
{
    /// <summary>
    /// Repository implementation for PrivateSysType operations
    /// </summary>
    public class PrivateSysTypeRepository : KeylessRepository<sql2code.Models.PrivateSysType>, IPrivateSysTypeRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        // This is a keyless entity, so some operations that require a primary key may not work
        // Method overrides have been implemented to handle the keyless nature of this entity
        
        
        /// <summary>
        /// Initializes a new instance of the PrivateSysTypeRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public PrivateSysTypeRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for PrivateSysType if needed
    }
}
