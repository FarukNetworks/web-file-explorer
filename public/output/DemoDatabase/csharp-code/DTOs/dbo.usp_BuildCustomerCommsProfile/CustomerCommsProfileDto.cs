using System;

namespace sql2code.DTOs.dbo.usp_BuildCustomerCommsProfile
{
    /// <summary>
    /// DTO representing the customer communications profile, migrated from the usp_BuildCustomerCommsProfile stored procedure.
    /// </summary>
    public class CustomerCommsProfileDto
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PrimaryEmail { get; set; } = null!;

        public bool HasMailingAddress { get; set; }

        public bool EmailOptInStatus { get; set; }

        public bool PostalOptInStatus { get; set; }

        public DateTime? LastInteractionDate { get; set; }

        public int? DaysSinceLastInteraction { get; set; }

        public bool IsActiveCustomer { get; set; }

        public bool OpenedMarketingEmailRecently { get; set; }

        public string CustomerSegment { get; set; } = null!;
    }
}
