using System.Collections.Generic;
using System.Threading.Tasks;
using sql2code.DTOs.dbo.usp_BuildCustomerCommsProfile;

namespace sql2code.Services.dbo.usp_BuildCustomerCommsProfile
{
    /// <summary>
    /// Interface for the service that provides customer communications profiles.
    /// </summary>
    public interface IUspBuildCustomerCommsProfileService
    {
        /// <summary>
        /// Gets the customer communications profiles for the specified customer or all customers.
        /// </summary>
        /// <param name="customerId">Optional ID of the customer. If null, returns for all customers.</param>
        /// <returns>List of customer communications profiles.</returns>
        Task<List<CustomerCommsProfileDto>> GetCustomerCommsProfilesAsync(int? customerId = null);
    }
}
