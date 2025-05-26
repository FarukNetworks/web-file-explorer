using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sql2code.DTOs.dbo.usp_BuildCustomerCommsProfile;
using sql2code.Services.dbo.usp_BuildCustomerCommsProfile;

namespace sql2code.Controllers.dbo.usp_BuildCustomerCommsProfile
{
    /// <summary>
    /// API Controller providing endpoints for customer communications profiles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UspBuildCustomerCommsProfileController : ControllerBase
    {
        private readonly IUspBuildCustomerCommsProfileService _service;

        /// <summary>
        /// Constructor with service dependency injection
        /// </summary>
        /// <param name="service">Injected service instance</param>
        public UspBuildCustomerCommsProfileController(IUspBuildCustomerCommsProfileService service)
        {
            _service = service;
        }

        /// <summary>
        /// GET endpoint to retrieve customer communications profiles for optional customer
        /// </summary>
        /// <param name="customerId">Optional customer ID filter</param>
        /// <returns>List of customer communication profiles</returns>
        [HttpGet]
        public async Task<ActionResult<List<CustomerCommsProfileDto>>> GetCustomerCommsProfiles([FromQuery] int? customerId = null)
        {
            var result = await _service.GetCustomerCommsProfilesAsync(customerId);
            return Ok(result);
        }
    }
}
