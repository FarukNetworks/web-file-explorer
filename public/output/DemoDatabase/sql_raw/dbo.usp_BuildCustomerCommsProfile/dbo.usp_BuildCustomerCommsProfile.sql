CREATE PROCEDURE usp_BuildCustomerCommsProfile (
    @CustomerID INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Today DATE = CONVERT(DATE, GETDATE());
    DECLARE @MarketingOpenLookbackDays INT = 90;
    WITH BaseCustomers AS (
        SELECT
            c.CustomerID,
            c.FirstName,
            c.LastName,
            c.Email AS PrimaryEmail
        FROM dbo.Customers c
        WHERE (@CustomerID IS NULL OR c.CustomerID = @CustomerID)
    ),


    PrimaryAddresses AS (
        SELECT
            a.CustomerID,
            MAX(CASE WHEN a.AddressType IN ('Billing', 'Shipping')
                     AND a.StreetAddress IS NOT NULL AND a.City IS NOT NULL
                     AND a.PostalCode IS NOT NULL AND a.Country IS NOT NULL
                     THEN 1 ELSE 0 END) AS HasMailingAddress
        FROM dbo.Addresses a
        WHERE (@CustomerID IS NULL OR a.CustomerID = @CustomerID)
        GROUP BY a.CustomerID
    ),


    LastActivityDates AS (
        SELECT
            CustomerID,
            MAX(ActivityDate) AS LastInteractionDate
        FROM (

            SELECT CustomerID, MAX(ApplicationDate) AS ActivityDate FROM dbo.LoanApplications WHERE (@CustomerID IS NULL OR CustomerID = @CustomerID) GROUP BY CustomerID
            UNION ALL

            SELECT CustomerID, MAX(StartDate) AS ActivityDate FROM dbo.Loans WHERE Status = 'Active' AND (@CustomerID IS NULL OR CustomerID = @CustomerID) GROUP BY CustomerID
            UNION ALL

            SELECT l.CustomerID, MAX(lp.PaymentDate) AS ActivityDate FROM dbo.LoanPayments lp JOIN dbo.Loans l ON lp.LoanID = l.LoanID WHERE (@CustomerID IS NULL OR l.CustomerID = @CustomerID) GROUP BY l.CustomerID

        ) AS ActivitySources
        GROUP BY CustomerID
    ),


    CommunicationPrefs AS (
        SELECT
            cp.CustomerID,
            MAX(CASE WHEN cp.Channel = 'Email' AND cp.OptInStatus = 1 THEN 1 ELSE 0 END) AS EmailOptInExplicit,
            MAX(CASE WHEN cp.Channel = 'Post' AND cp.OptInStatus = 1 THEN 1 ELSE 0 END) AS PostalOptInExplicit
        FROM dbo.CommunicationPreferences cp
        WHERE (@CustomerID IS NULL OR cp.CustomerID = @CustomerID)
        GROUP BY cp.CustomerID
    ),


    RecentMarketingOpens AS (
        SELECT
            mes.CustomerID,
            MAX(CAST(mes.WasOpened AS INT)) AS OpenedMarketingEmailRecently
        FROM dbo.MarketingEmailsSent mes
        WHERE mes.SentDate >= DATEADD(day, -@MarketingOpenLookbackDays, @Today)
          AND (@CustomerID IS NULL OR mes.CustomerID = @CustomerID)
        GROUP BY mes.CustomerID
    ),


    ActiveStatus AS (
        SELECT
            l.CustomerID,
            MAX(CASE WHEN l.Status = 'Active' THEN 1 ELSE 0 END) AS IsActiveCustomer
        FROM dbo.Loans l
        WHERE (@CustomerID IS NULL OR l.CustomerID = @CustomerID)
        GROUP BY l.CustomerID
    )


    SELECT
        bc.CustomerID,
        bc.FirstName,
        bc.LastName,
        bc.PrimaryEmail,
        ISNULL(pa.HasMailingAddress, 0) AS HasMailingAddress,


        ISNULL(cp.EmailOptInExplicit, 0) AS EmailOptInStatus,
        ISNULL(cp.PostalOptInExplicit, 0) AS PostalOptInStatus,

        lad.LastInteractionDate,

        CASE
            WHEN lad.LastInteractionDate IS NOT NULL THEN DATEDIFF(day, lad.LastInteractionDate, @Today)
            ELSE NULL
        END AS DaysSinceLastInteraction,

        ISNULL(act.IsActiveCustomer, 0) AS IsActiveCustomer,
        ISNULL(rmo.OpenedMarketingEmailRecently, 0) AS OpenedMarketingEmailRecently,


        CASE
            WHEN ISNULL(act.IsActiveCustomer, 0) = 1 THEN 'Active Loan Holder'
            WHEN lad.LastInteractionDate >= DATEADD(year, -1, @Today) THEN 'Recent Inactive'
            WHEN lad.LastInteractionDate < DATEADD(year, -1, @Today) THEN 'Long-term Inactive'
            ELSE 'Prospect/New'
        END AS CustomerSegment

    FROM BaseCustomers bc
    LEFT JOIN PrimaryAddresses pa ON bc.CustomerID = pa.CustomerID
    LEFT JOIN LastActivityDates lad ON bc.CustomerID = lad.CustomerID
    LEFT JOIN CommunicationPrefs cp ON bc.CustomerID = cp.CustomerID
    LEFT JOIN RecentMarketingOpens rmo ON bc.CustomerID = rmo.CustomerID
    LEFT JOIN ActiveStatus act ON bc.CustomerID = act.CustomerID
    ORDER BY bc.LastName, bc.FirstName;

END;