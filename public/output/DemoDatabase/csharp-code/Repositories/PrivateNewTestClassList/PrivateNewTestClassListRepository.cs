using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.PrivateNewTestClassList
{
    /// <summary>
    /// Repository implementation for PrivateNewTestClassList operations
    /// </summary>
    public class PrivateNewTestClassListRepository : Repository<sql2code.Models.PrivateNewTestClassList>, IPrivateNewTestClassListRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the PrivateNewTestClassListRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public PrivateNewTestClassListRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for PrivateNewTestClassList if needed
    }
}
