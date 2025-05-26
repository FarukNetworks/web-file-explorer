using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.MarketingEmailsSent
{
    /// <summary>
    /// Repository implementation for MarketingEmailsSent operations
    /// </summary>
    public class MarketingEmailsSentRepository : Repository<sql2code.Models.MarketingEmailsSent>, IMarketingEmailsSentRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the MarketingEmailsSentRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public MarketingEmailsSentRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for MarketingEmailsSent if needed
    }
}
