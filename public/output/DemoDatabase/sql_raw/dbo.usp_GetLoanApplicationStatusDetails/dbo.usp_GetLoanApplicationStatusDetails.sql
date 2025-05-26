CREATE PROCEDURE usp_GetLoanApplicationStatusDetails
    @LoanApplicationID INT
AS
BEGIN
    SET NOCOUNT ON;


    IF NOT EXISTS (SELECT 1 FROM dbo.LoanApplications WHERE LoanApplicationID = @LoanApplicationID)
    BEGIN
        THROW 50010, 'Loan Application ID not found.', 1;
        RETURN;
    END


    SELECT

        app.LoanApplicationID,
        app.ApplicationDate,
        app.RequestedAmount,
        app.LoanPurpose,
        app.Status AS ApplicationStatus,
        app.SubmittedDate,
        app.LastUpdatedDate,


        cust.CustomerID,
        cust.FirstName,
        cust.LastName,
        cust.Email AS CustomerEmail,


        prod.LoanProductID,
        prod.Name AS LoanProductName,
        prod.InterestRate AS StandardRate,
        prod.TermMonths AS StandardTerm,
        cs.Score AS LatestCreditScore,
        cs.Provider AS CreditScoreProvider,
        cs.DateChecked AS CreditScoreDateChecked,


        inc.Source AS IncomeSource,
        inc.VerifiedAmount AS VerifiedIncomeAmount,
        inc.VerificationDate AS IncomeVerificationDate,
        inc.Status AS IncomeVerificationStatus,
        inc.MonthlyAmount AS VerifiedMonthlyIncome,
        dec.Decision AS FinalDecision,
        dec.DecisionDate,
        dec.Reason AS DecisionReason,
        dec.ApprovedAmount,
        dec.ApprovedRate,
        dec.ApprovedTerm,
        dec.DecisionBy AS DecisionProcessorUserID

    FROM
        dbo.LoanApplications app
    INNER JOIN
        dbo.Customers cust ON app.CustomerID = cust.CustomerID
    LEFT JOIN
        dbo.LoanProducts prod ON app.ProposedLoanProductID = prod.LoanProductID
    LEFT JOIN
        dbo.CreditScores cs ON app.CustomerID = cs.CustomerID AND cs.CreditScoreID = (SELECT MAX(CreditScoreID) FROM dbo.CreditScores WHERE CustomerID = app.CustomerID)
    LEFT JOIN
        dbo.IncomeVerifications inc ON app.CustomerID = inc.CustomerID AND inc.IncomeVerificationID = (SELECT MAX(IncomeVerificationID) FROM dbo.IncomeVerifications WHERE CustomerID = app.CustomerID)
    LEFT JOIN
        dbo.ApprovalDecisions dec ON app.LoanApplicationID = dec.LoanApplicationID
    WHERE
        app.LoanApplicationID = @LoanApplicationID;

END;