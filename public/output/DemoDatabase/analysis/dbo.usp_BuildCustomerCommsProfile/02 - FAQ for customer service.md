# Business Functions


## BF-001: Retrieve Base Customer Information

### Description
Extracts basic customer details from the Customers table by optionally filtering on CustomerID.

### Business Purpose
Provides the core customer profile data needed to build the communication profile.

### SQL Snippet
```sql
WITH BaseCustomers AS (
    SELECT
        c.CustomerID,
        c.FirstName,
        c.LastName,
        c.Email AS PrimaryEmail
    FROM dbo.Customers c
    WHERE (@CustomerID IS NULL OR c.CustomerID = @CustomerID)
)
```

### Topics
#### Q: What information does this function retrieve?

A: This function extracts core customer details, including ID, first name, last name, and primary email from the Customers table.

#### Q: Why is the CustomerID parameter optional?

A: Allowing CustomerID to be optional helps the function run either for a specific customer when provided or for all customers when not specified.



## BF-002: Evaluate Mailing Address Presence

### Description
Determines if a customer has a valid mailing address based on address type and completeness of address fields.

### Business Purpose
Ensures that customers have the necessary address information for communications and deliveries.

### SQL Snippet
```sql
WITH PrimaryAddresses AS (
    SELECT
        a.CustomerID,
        MAX(CASE WHEN a.AddressType IN ('Billing', 'Shipping')
                 AND a.StreetAddress IS NOT NULL AND a.City IS NOT NULL
                 AND a.PostalCode IS NOT NULL AND a.Country IS NOT NULL
                 THEN 1 ELSE 0 END) AS HasMailingAddress
    FROM dbo.Addresses a
    WHERE (@CustomerID IS NULL OR a.CustomerID = @CustomerID)
    GROUP BY a.CustomerID
)
```

### Topics
#### Q: What does this function check for regarding mailing addresses?

A: It examines a customer's addresses to determine if they have a complete mailing address based on address type and field completeness.

#### Q: How is the address validity determined?

A: A valid mailing address must be of type Billing or Shipping and include non-null values for street address, city, postal code, and country.



## BF-003: Aggregate Last Interaction Date

### Description
Consolidates different sources of customer activity to obtain the most recent interaction date for each customer.

### Business Purpose
Provides a single metric for customer engagement which is critical for segmentation and re-engagement campaigns.

### SQL Snippet
```sql
WITH LastActivityDates AS (
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
)
```

### Topics
#### Q: How is the last interaction date for a customer determined?

A: It calculates the most recent date when the customer interacted, by aggregating activity dates from loan applications, active loans, and loan payments.

#### Q: Why are multiple sources such as applications, loans, and payments used?

A: Using multiple sources ensures a comprehensive overview of the customer’s interactions to accurately reflect engagement.



## BF-004: Fetch Communication Preferences

### Description
Retrieves customer communication preferences and calculates explicit opt-in statuses for both Email and Postal channels.

### Business Purpose
Determines the channels the customer has agreed to be contacted through, supporting compliance and targeting.

### SQL Snippet
```sql
WITH CommunicationPrefs AS (
    SELECT
        cp.CustomerID,
        MAX(CASE WHEN cp.Channel = 'Email' AND cp.OptInStatus = 1 THEN 1 ELSE 0 END) AS EmailOptInExplicit,
        MAX(CASE WHEN cp.Channel = 'Post' AND cp.OptInStatus = 1 THEN 1 ELSE 0 END) AS PostalOptInExplicit
    FROM dbo.CommunicationPreferences cp
    WHERE (@CustomerID IS NULL OR cp.CustomerID = @CustomerID)
    GROUP BY cp.CustomerID
)
```

### Topics
#### Q: What communication preferences does this function retrieve?

A: It retrieves whether a customer has explicitly opted in for Email and Postal communications.

#### Q: How are the opt-in statuses determined?

A: It checks the CommunicationPreferences table for flags where OptInStatus is set to 1 and converts the result into boolean values.



## BF-005: Assess Recent Marketing Opens

### Description
Evaluates marketing email campaigns to identify if the customer has engaged by opening an email within the defined lookback period.

### Business Purpose
Measures customer engagement with marketing emails which informs further outreach strategies.

### SQL Snippet
```sql
WITH RecentMarketingOpens AS (
    SELECT
        mes.CustomerID,
        MAX(CAST(mes.WasOpened AS INT)) AS OpenedMarketingEmailRecently
    FROM dbo.MarketingEmailsSent mes
    WHERE mes.SentDate >= DATEADD(day, -@MarketingOpenLookbackDays, @Today)
      AND (@CustomerID IS NULL OR mes.CustomerID = @CustomerID)
    GROUP BY mes.CustomerID
)
```

### Topics
#### Q: How does the function determine if a marketing email was recently opened?

A: It verifies if a customer opened a marketing email within a defined period by comparing the email's sent date with the current date adjusted by a lookback period.

#### Q: What role does the lookback period play?

A: The lookback period defines the timeframe (e.g., 90 days) during which email opens are considered recent engagement.



## BF-006: Determine Active Loan Status

### Description
Checks if the customer currently holds an active loan by evaluating the status from the Loans table.

### Business Purpose
Identifies active loan holders to help segment customers for communications and market offerings.

### SQL Snippet
```sql
WITH ActiveStatus AS (
    SELECT
        l.CustomerID,
        MAX(CASE WHEN l.Status = 'Active' THEN 1 ELSE 0 END) AS IsActiveCustomer
    FROM dbo.Loans l
    WHERE (@CustomerID IS NULL OR l.CustomerID = @CustomerID)
    GROUP BY l.CustomerID
)
```

### Topics
#### Q: How is the active loan status of a customer identified?

A: This function checks the Loans table to see if a customer has any loans with the status 'Active', indicating current borrowing activity.

#### Q: Why is it important to know if a customer has an active loan?

A: Identifying active loans helps in segmenting customers for specific communications or offers based on their current loan status.



## BF-007: Compute Customer Segmentation

### Description
Determines the final customer segment label based on active status and recency of interaction.

### Business Purpose
Facilitates tailored communications and marketing strategies by segmenting customers into distinct groups.

### SQL Snippet
```sql
CASE
    WHEN ISNULL(act.IsActiveCustomer, 0) = 1 THEN 'Active Loan Holder'
    WHEN lad.LastInteractionDate >= DATEADD(year, -1, @Today) THEN 'Recent Inactive'
    WHEN lad.LastInteractionDate < DATEADD(year, -1, @Today) THEN 'Long-term Inactive'
    ELSE 'Prospect/New'
END AS CustomerSegment
```

### Topics
#### Q: How is the customer segmentation determined?

A: Segmentation is based on whether a customer has an active loan and how recent their last interaction was. Depending on these factors, they are classified as an 'Active Loan Holder', 'Recent Inactive', 'Long-term Inactive', or 'Prospect/New'.

#### Q: Why is segmentation important in this process?

A: Segmenting customers helps target communications and marketing strategies appropriately based on customer behavior and engagement.



## BF-008: Marketing Open Lookback Configuration

### Description
Defines the lookback period (in days) that determines which marketing email opens are considered recent.

### Business Purpose
Allows dynamic adjustment of the period used to assess recent marketing email engagements.

### Configuration Details
- Parameter: @MarketingOpenLookbackDays
- Value: 90

### Topics
#### Q: What does the Marketing Open Lookback Configuration do?

A: It sets the period (90 days) during which a marketing email open is considered recent, thereby impacting how engagement is measured.

#### Q: Can the lookback period be adjusted?

A: Yes, the lookback period is configurable, allowing the business to update the engagement criteria as needed.


