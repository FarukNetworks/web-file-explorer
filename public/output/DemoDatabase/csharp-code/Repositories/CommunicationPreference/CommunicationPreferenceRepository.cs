using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.CommunicationPreference
{
    /// <summary>
    /// Repository implementation for CommunicationPreference operations
    /// </summary>
    public class CommunicationPreferenceRepository : Repository<sql2code.Models.CommunicationPreference>, ICommunicationPreferenceRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the CommunicationPreferenceRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public CommunicationPreferenceRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for CommunicationPreference if needed
    }
}
