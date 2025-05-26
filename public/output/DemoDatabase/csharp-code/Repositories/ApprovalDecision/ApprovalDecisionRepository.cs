using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.ApprovalDecision
{
    /// <summary>
    /// Repository implementation for ApprovalDecision operations
    /// </summary>
    public class ApprovalDecisionRepository : Repository<sql2code.Models.ApprovalDecision>, IApprovalDecisionRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the ApprovalDecisionRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public ApprovalDecisionRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for ApprovalDecision if needed
    }
}
