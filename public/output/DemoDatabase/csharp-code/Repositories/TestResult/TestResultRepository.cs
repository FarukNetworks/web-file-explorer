using sql2code.Abstractions.Repositories;
using sql2code.Data;
using sql2code.Models;

namespace sql2code.Repositories.TestResult
{
    /// <summary>
    /// Repository implementation for TestResult operations
    /// </summary>
    public class TestResultRepository : Repository<sql2code.Models.TestResult>, ITestResultRepository
    {
        // No need to redefine _context as it's already available from base class (ReadRepository)
        
        /// <summary>
        /// Initializes a new instance of the TestResultRepository class
        /// </summary>
        /// <param name="context">Database context</param>
        public TestResultRepository(AppDbContext context) : base(context)
        {
            // Base constructor handles context initialization
        }
        
        // Add specific implementation methods for TestResult if needed
    }
}
