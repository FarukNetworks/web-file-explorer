using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sql2code.Abstractions.Repositories;
using sql2code.DTOs.dbo.usp_BuildCustomerCommsProfile;
using sql2code.Models;

namespace sql2code.Services.dbo.usp_BuildCustomerCommsProfile
{
    /// <summary>
    /// Service implementation for building customer communications profiles,
    /// migrated from the usp_BuildCustomerCommsProfile stored procedure.
    /// </summary>
    public class UspBuildCustomerCommsProfileService : IUspBuildCustomerCommsProfileService
    {
        private readonly IReadRepository<Customer> _customerRepository;
        private readonly IReadRepository<Address> _addressRepository;
        private readonly IReadRepository<LoanApplication> _loanApplicationRepository;
        private readonly IReadRepository<Loan> _loanRepository;
        private readonly IReadRepository<LoanPayment> _loanPaymentRepository;
        private readonly IReadRepository<CommunicationPreference> _communicationPreferenceRepository;
        private readonly IReadRepository<MarketingEmailsSent> _marketingEmailsSentRepository;

        private static readonly int MarketingOpenLookbackDays = 90;

        /// <summary>
        /// Constructor injecting required repositories
        /// </summary>
        public UspBuildCustomerCommsProfileService(
            IReadRepository<Customer> customerRepository,
            IReadRepository<Address> addressRepository,
            IReadRepository<LoanApplication> loanApplicationRepository,
            IReadRepository<Loan> loanRepository,
            IReadRepository<LoanPayment> loanPaymentRepository,
            IReadRepository<CommunicationPreference> communicationPreferenceRepository,
            IReadRepository<MarketingEmailsSent> marketingEmailsSentRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _loanApplicationRepository = loanApplicationRepository;
            _loanRepository = loanRepository;
            _loanPaymentRepository = loanPaymentRepository;
            _communicationPreferenceRepository = communicationPreferenceRepository;
            _marketingEmailsSentRepository = marketingEmailsSentRepository;
        }

        /// <inheritdoc />
        public async Task<List<CustomerCommsProfileDto>> GetCustomerCommsProfilesAsync(int? customerId = null)
        {
            // Current date for calculations
            var today = DateTime.UtcNow.Date;

            // Base customers per stored procedure - with optional filter by customerId
            var baseCustomersQuery = _customerRepository.GetQueryable()
                .Where(c => !customerId.HasValue || c.CustomerId == customerId.Value)
                .Select(c => new
                {
                    c.CustomerId,
                    c.FirstName,
                    c.LastName,
                    PrimaryEmail = c.Email
                });

            // Primary addresses aggregation
            var primaryAddressesQuery = _addressRepository.GetQueryable()
                .Where(a => !customerId.HasValue || a.CustomerId == customerId.Value)
                .GroupBy(a => a.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    HasMailingAddress = g.Max(a => ((a.AddressType == "Billing" || a.AddressType == "Shipping")
                        && !string.IsNullOrEmpty(a.StreetAddress)
                        && !string.IsNullOrEmpty(a.City)
                        && !string.IsNullOrEmpty(a.PostalCode)
                        && !string.IsNullOrEmpty(a.Country)) ? 1 : 0) == 1
                });

            // Last activity dates combined from three sources
            var loanApplicationsMaxDates = _loanApplicationRepository.GetQueryable()
                .Where(la => !customerId.HasValue || la.CustomerId == customerId.Value)
                .GroupBy(la => la.CustomerId)
                .Select(g => new { CustomerId = g.Key, LastActivityDate = g.Max(la => la.ApplicationDate) });

            var activeLoansMaxDates = _loanRepository.GetQueryable()
                .Where(l => !customerId.HasValue || l.CustomerId == customerId.Value)
                .Where(l => l.Status == "Active")
                .GroupBy(l => l.CustomerId)
                .Select(g => new { CustomerId = g.Key, LastActivityDate = g.Max(l => l.StartDate.ToDateTime(TimeOnly.MinValue)) });

            var loanPaymentsMaxDates = _loanPaymentRepository.GetQueryable()
                .Where(lp => !customerId.HasValue || lp.Loan.CustomerId == customerId.Value)
                .GroupBy(lp => lp.Loan.CustomerId)
                .Select(g => new { CustomerId = g.Key, LastActivityDate = g.Max(lp => lp.PaymentDate) });

            // Combine all activity sources, grouping by CustomerId and getting max date.
            // Rename the resulting field to LastInteraction
            var combinedLastActivityDates = loanApplicationsMaxDates
                .Concat(activeLoansMaxDates)
                .Concat(loanPaymentsMaxDates)
                .GroupBy(x => x.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    LastInteraction = g.Max(x => x.LastActivityDate)
                });

            // Communication preferences
            var communicationPrefsQuery = _communicationPreferenceRepository.GetQueryable()
                .Where(cp => !customerId.HasValue || cp.CustomerId == customerId.Value)
                .GroupBy(cp => cp.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    EmailOptInExplicit = g.Max(cp => (cp.Channel == "Email" && cp.OptInStatus) ? 1 : 0),
                    PostalOptInExplicit = g.Max(cp => (cp.Channel == "Post" && cp.OptInStatus) ? 1 : 0),
                });

            // Recent marketing opens
            var marketingCutoffDate = today.AddDays(-MarketingOpenLookbackDays);
            var recentMarketingOpensQuery = _marketingEmailsSentRepository.GetQueryable()
                .Where(mes => mes.SentDate >= marketingCutoffDate && (!customerId.HasValue || mes.CustomerId == customerId.Value))
                .GroupBy(mes => mes.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    OpenedMarketingEmailRecently = g.Max(mes => mes.WasOpened ? 1 : 0)
                });

            // Active status
            var activeStatusQuery = _loanRepository.GetQueryable()
                .Where(l => !customerId.HasValue || l.CustomerId == customerId.Value)
                .GroupBy(l => l.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    IsActiveCustomer = g.Max(l => l.Status == "Active" ? 1 : 0)
                });

            // Compose final query using left joins (join with DefaultIfEmpty)
            var query = from bc in baseCustomersQuery
                        join pa in primaryAddressesQuery on bc.CustomerId equals pa.CustomerId into paJoin
                        from pa in paJoin.DefaultIfEmpty()
                        join cp in communicationPrefsQuery on bc.CustomerId equals cp.CustomerId into cpJoin
                        from cp in cpJoin.DefaultIfEmpty()
                        join lad in combinedLastActivityDates on bc.CustomerId equals lad.CustomerId into ladJoin
                        from lad in ladJoin.DefaultIfEmpty()
                        join rmo in recentMarketingOpensQuery on bc.CustomerId equals rmo.CustomerId into rmoJoin
                        from rmo in rmoJoin.DefaultIfEmpty()
                        join act in activeStatusQuery on bc.CustomerId equals act.CustomerId into actJoin
                        from act in actJoin.DefaultIfEmpty()
                        orderby bc.LastName, bc.FirstName
                        select new
                        {
                            bc.CustomerId,
                            bc.FirstName,
                            bc.LastName,
                            bc.PrimaryEmail,
                            HasMailingAddress = pa != null && pa.HasMailingAddress,
                            EmailOptInStatus = cp != null && cp.EmailOptInExplicit == 1,
                            PostalOptInStatus = cp != null && cp.PostalOptInExplicit == 1,
                            LastInteractionDate = lad != null ? lad.LastInteraction : (DateTime?)null,
                            DaysSinceLastInteraction = lad != null ? (int)(today - lad.LastInteraction).TotalDays : (int?)null,
                            IsActiveCustomer = act != null && act.IsActiveCustomer == 1,
                            OpenedMarketingEmailRecently = rmo != null && rmo.OpenedMarketingEmailRecently == 1
                        };

            // Materialize the query before applying business logic that cannot be translated.
            var rawResults = await query.ToListAsync();

            // Now, in memory, determine the customer segment and map to DTO.
            var results = rawResults.Select(x => new CustomerCommsProfileDto
            {
                CustomerId = x.CustomerId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PrimaryEmail = x.PrimaryEmail,
                HasMailingAddress = x.HasMailingAddress,
                EmailOptInStatus = x.EmailOptInStatus,
                PostalOptInStatus = x.PostalOptInStatus,
                LastInteractionDate = x.LastInteractionDate,
                DaysSinceLastInteraction = x.DaysSinceLastInteraction,
                IsActiveCustomer = x.IsActiveCustomer,
                OpenedMarketingEmailRecently = x.OpenedMarketingEmailRecently,
                CustomerSegment = DetermineCustomerSegment(x.IsActiveCustomer, x.LastInteractionDate, today)
            }).ToList();

            return results;
        }

        /// <summary>
        /// Determines the customer segment based on active status and last interaction date.
        /// <para>Translated from the stored procedure CASE statement.</para>
        /// </summary>
        /// <param name="isActiveCustomer">Whether the customer has an active loan</param>
        /// <param name="lastInteractionDate">The last interaction date</param>
        /// <param name="today">Current date for comparison</param>
        /// <returns>Customer segment string</returns>
        private static string DetermineCustomerSegment(bool isActiveCustomer, DateTime? lastInteractionDate, DateTime today)
        {
            if (isActiveCustomer)
            {
                return "Active Loan Holder";
            }

            if (lastInteractionDate.HasValue)
            {
                if (lastInteractionDate.Value >= today.AddYears(-1))
                {
                    return "Recent Inactive";
                }
                else
                {
                    return "Long-term Inactive";
                }
            }

            // Prospect/New if no last interaction date is present.
            return "Prospect/New";
        }
    }
}
