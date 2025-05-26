CREATE PROCEDURE usp_PerformLoanUnderwritingAnalysis (
    @LoanApplicationID INT
)
AS
BEGIN
    SET NOCOUNT ON;


    DECLARE @CustomerID INT, @RequestedAmount DECIMAL(18, 2), @LoanPurpose NVARCHAR(100),
            @ProposedLoanProductID INT, @ApplicationStatus NVARCHAR(50), @LoanType NVARCHAR(50),
            @RequiresCollateral BIT, @MinCreditScore INT, @MaxDTI DECIMAL(5, 2), @MaxLTV DECIMAL(5, 2);


    SELECT
        @CustomerID = la.CustomerID,
        @RequestedAmount = la.RequestedAmount,
        @LoanPurpose = la.LoanPurpose,
        @ProposedLoanProductID = la.ProposedLoanProductID,
        @ApplicationStatus = la.Status
    FROM dbo.LoanApplications la
    WHERE la.LoanApplicationID = @LoanApplicationID;

    IF @CustomerID IS NULL
    BEGIN
        THROW 50020, 'Loan Application ID not found.', 1; RETURN;
    END
    IF @ApplicationStatus NOT IN ('Under Review', 'Pending Docs', 'Submitted')
    BEGIN
        THROW 50021, 'Loan Application is not in a state suitable for underwriting analysis.', 1; RETURN;
    END


    SELECT
        @LoanType = lp.ProductType,
        @RequiresCollateral = lp.RequiresCollateral,
        @MinCreditScore = lp.MinCreditScoreRequired,
        @MaxDTI = lp.MaxDTIPercent / 100.0,
        @MaxLTV = lp.MaxLTVPercent / 100.0
    FROM dbo.LoanProducts lp
    WHERE lp.LoanProductID = @ProposedLoanProductID;

    IF @LoanType IS NULL
    BEGIN
        THROW 50022, 'Associated Loan Product details not found or application not linked.', 1; RETURN;
    END


    DECLARE @AnalysisResults TABLE (
        CheckCategory VARCHAR(50),
        CheckName VARCHAR(100),
        ResultCode VARCHAR(20),
        Details NVARCHAR(500),
        NumericValue DECIMAL(18, 4) NULL
    );


    DECLARE @LatestCreditScore INT, @CreditScoreDate DATE, @BankruptciesLast5Yrs INT, @DelinquenciesLast2Yrs INT;


    SELECT TOP 1
        @LatestCreditScore = cs.Score,
        @CreditScoreDate = cs.DateChecked,
        @BankruptciesLast5Yrs = cs.BankruptciesLast5Years,
        @DelinquenciesLast2Yrs = cs.DelinquenciesLast2Years
    FROM dbo.CreditScores cs
    WHERE cs.CustomerID = @CustomerID
    ORDER BY cs.DateChecked DESC;

    INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Credit', 'Credit Score Check Date', IIF(@CreditScoreDate IS NOT NULL, 'INFO', 'FAIL'), 'Latest score checked on: ' + ISNULL(CONVERT(VARCHAR, @CreditScoreDate, 120), 'N/A'), NULL);

    IF @CreditScoreDate IS NULL
    BEGIN
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Credit', 'Credit Score Available', 'FAIL', 'No credit score found for customer.', NULL);
    END
    ELSE
    BEGIN
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Credit', 'Credit Score Available', 'PASS', 'Score: ' + CAST(@LatestCreditScore AS VARCHAR), @LatestCreditScore);
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Credit', 'Min Score Requirement', IIF(@LatestCreditScore >= @MinCreditScore, 'PASS', 'FAIL'), 'Product requires min score of ' + CAST(@MinCreditScore AS VARCHAR), @LatestCreditScore);
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Credit', 'Bankruptcies (Last 5Y)', IIF(@BankruptciesLast5Yrs = 0, 'PASS', 'WARN'), 'Count: ' + CAST(@BankruptciesLast5Yrs AS VARCHAR), @BankruptciesLast5Yrs);
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Credit', 'Delinquencies (Last 2Y)', IIF(@DelinquenciesLast2Yrs <= 1, 'PASS', 'WARN'), 'Count: ' + CAST(@DelinquenciesLast2Yrs AS VARCHAR), @DelinquenciesLast2Yrs);
    END





    DECLARE @VerifiedMonthlyIncome DECIMAL(18, 2), @IncomeVerifiedDate DATE, @IncomeVerificationStatus VARCHAR(20);
    DECLARE @EmploymentStatus VARCHAR(50), @MonthsInCurrentJob INT, @EmploymentVerifiedDate DATE;

    SELECT TOP 1
        @VerifiedMonthlyIncome = iv.MonthlyAmount,
        @IncomeVerifiedDate = iv.VerificationDate,
        @IncomeVerificationStatus = iv.Status
    FROM dbo.IncomeVerifications iv
    WHERE iv.CustomerID = @CustomerID
    ORDER BY iv.VerificationDate DESC;

    SELECT TOP 1
        @EmploymentStatus = eh.EmploymentStatus,
        @MonthsInCurrentJob = DATEDIFF(month, eh.StartDate, GETDATE()),
        @EmploymentVerifiedDate = eh.VerificationDate
    FROM dbo.EmploymentHistory eh
    WHERE eh.CustomerID = @CustomerID AND eh.IsCurrent = 1
    ORDER BY eh.VerificationDate DESC;

    IF @IncomeVerificationStatus IS NULL OR @IncomeVerificationStatus != 'Verified'
    BEGIN
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Income', 'Income Verified', 'FAIL', 'Income status: ' + ISNULL(@IncomeVerificationStatus, 'Not Found'), NULL);
    END
    ELSE
    BEGIN
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Income', 'Income Verified', 'PASS', 'Verified Amount: ' + FORMAT(@VerifiedMonthlyIncome, 'C', 'en-US') + '/month on ' + CONVERT(VARCHAR, @IncomeVerifiedDate, 120), @VerifiedMonthlyIncome);
    END

    IF @EmploymentStatus IS NULL OR @EmploymentVerifiedDate IS NULL
    BEGIN
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Employment', 'Employment Verified', 'FAIL', 'Current employment not verified or verification outdated.', NULL);
    END
    ELSE
    BEGIN
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Employment', 'Employment Verified', 'PASS', 'Status: ' + @EmploymentStatus + ', Verified: ' + CONVERT(VARCHAR, @EmploymentVerifiedDate, 120), NULL);
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Employment', 'Job Stability (Months)', IIF(@MonthsInCurrentJob >= 12, 'PASS', 'WARN'), 'Months in current job: ' + CAST(@MonthsInCurrentJob AS VARCHAR), @MonthsInCurrentJob);
    END





    DECLARE @TotalExistingMonthlyDebt DECIMAL(18, 2) = 0;
    DECLARE @ProposedMonthlyPayment DECIMAL(18, 2);
    DECLARE @CalculatedDTI DECIMAL(5, 4);
    SELECT @TotalExistingMonthlyDebt = SUM(ISNULL(l.MonthlyPaymentAmount, 0))
    FROM dbo.Loans l
    WHERE l.CustomerID = @CustomerID
      AND l.Status = 'Active';
    IF @ProposedLoanProductID IS NOT NULL
    BEGIN
        DECLARE @ProductRate DECIMAL(5,4), @ProductTerm INT
        SELECT @ProductRate = InterestRate / 100.0, @ProductTerm = TermMonths FROM dbo.LoanProducts WHERE LoanProductID = @ProposedLoanProductID
        IF @ProductRate > 0 AND @ProductTerm > 0
             SET @ProposedMonthlyPayment = (@RequestedAmount * (@ProductRate/12.0) * POWER(1 + (@ProductRate/12.0), @ProductTerm)) / (POWER(1 + (@ProductRate/12.0), @ProductTerm) - 1)
        ELSE SET @ProposedMonthlyPayment = NULL;
    END

    INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Debt', 'Existing Monthly Debt', 'INFO', FORMAT(@TotalExistingMonthlyDebt, 'C', 'en-US'), @TotalExistingMonthlyDebt);
    INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Debt', 'Proposed Monthly Payment', IIF(@ProposedMonthlyPayment IS NOT NULL, 'INFO', 'FAIL'), FORMAT(@ProposedMonthlyPayment, 'C', 'en-US'), @ProposedMonthlyPayment);


    IF @VerifiedMonthlyIncome > 0 AND @ProposedMonthlyPayment IS NOT NULL
    BEGIN
        SET @CalculatedDTI = (@TotalExistingMonthlyDebt + @ProposedMonthlyPayment) / @VerifiedMonthlyIncome;
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Debt', 'Debt-to-Income (DTI) Ratio', IIF(@CalculatedDTI <= @MaxDTI, 'PASS', 'FAIL'), 'Calculated DTI: ' + FORMAT(@CalculatedDTI, 'P2'), @CalculatedDTI);
    END
    ELSE
    BEGIN
        SET @CalculatedDTI = NULL;
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Debt', 'Debt-to-Income (DTI) Ratio', 'FAIL', 'Cannot calculate DTI (Missing Income or Proposed Payment)', NULL);
    END





    DECLARE @CollateralValue DECIMAL(18, 2), @CollateralVerifiedDate DATE, @CollateralType VARCHAR(50);
    DECLARE @CalculatedLTV DECIMAL(5, 4);

    IF @RequiresCollateral = 1
    BEGIN
        SELECT TOP 1
            @CollateralValue = c.EstimatedValue,
            @CollateralVerifiedDate = c.VerificationDate,
            @CollateralType = c.Description
        FROM dbo.Collaterals c
        WHERE c.LoanApplicationID = @LoanApplicationID
        ORDER BY c.VerificationDate DESC;

        IF @CollateralValue IS NULL OR @CollateralVerifiedDate IS NULL
        BEGIN
            INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Collateral', 'Collateral Verified', 'FAIL', 'Required collateral not found or not verified recently.', NULL);
            SET @CalculatedLTV = NULL;
        END
        ELSE
        BEGIN
             INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Collateral', 'Collateral Verified', 'PASS', 'Type: ' + @CollateralType + ', Value: ' + FORMAT(@CollateralValue, 'C', 'en-US') + ', Verified: ' + CONVERT(VARCHAR, @CollateralVerifiedDate, 120), @CollateralValue);
             IF @CollateralValue > 0
             BEGIN
                 SET @CalculatedLTV = @RequestedAmount / @CollateralValue;
                 INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Collateral', 'Loan-to-Value (LTV) Ratio', IIF(@CalculatedLTV <= @MaxLTV, 'PASS', 'FAIL'), 'Calculated LTV: ' + FORMAT(@CalculatedLTV, 'P2'), @CalculatedLTV);
             END
             ELSE
             BEGIN
                 SET @CalculatedLTV = NULL;
                 INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Collateral', 'Loan-to-Value (LTV) Ratio', 'FAIL', 'Cannot calculate LTV (Collateral value is zero or missing)', NULL);
             END
        END
    END
    ELSE
    BEGIN
        INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Collateral', 'Collateral Requirement', 'N/A', 'Loan product does not require collateral.', NULL);
    END






    INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details) VALUES ('Policy', 'Loan Purpose Eligibility', IIF(@LoanPurpose IN ('Debt Consolidation', 'Home Improvement', 'Vehicle Purchase'), 'PASS', 'WARN'), 'Purpose: ' + @LoanPurpose);
    INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details) VALUES ('Policy', 'Max Loan Amount', IIF(@RequestedAmount <= 100000, 'PASS', 'WARN'), 'Requested: ' + FORMAT(@RequestedAmount, 'C', 'en-US'));

    DECLARE @FraudFlagCount INT = 0;

    INSERT INTO @AnalysisResults (CheckCategory, CheckName, ResultCode, Details, NumericValue) VALUES ('Fraud', 'Active Fraud Flags', IIF(@FraudFlagCount = 0, 'PASS', 'FAIL'), 'Count: ' + CAST(@FraudFlagCount AS VARCHAR), @FraudFlagCount);

    DECLARE @OverallAssessment VARCHAR(50) = 'Refer to Underwriter';
    DECLARE @FailCount INT = 0;
    SELECT @FailCount = COUNT(*) FROM @AnalysisResults WHERE ResultCode = 'FAIL';

    IF @FailCount = 0
    BEGIN

        SET @OverallAssessment = 'Preliminary Approval Recommended';
    END
    ELSE
    BEGIN
         SET @OverallAssessment = 'Rejection Recommended / Needs Review';
    END


    SELECT
        @LoanApplicationID AS LoanApplicationID,
        @CustomerID AS CustomerID,
        (SELECT FirstName + ' ' + LastName FROM dbo.Customers WHERE CustomerID = @CustomerID) AS CustomerName,
        @RequestedAmount AS RequestedAmount,
        @LoanPurpose AS LoanPurpose,
        @LoanType AS LoanType,
        @OverallAssessment AS OverallAssessment,
        (SELECT COUNT(*) FROM @AnalysisResults WHERE ResultCode = 'FAIL') AS FailCount,
        (SELECT COUNT(*) FROM @AnalysisResults WHERE ResultCode = 'WARN') AS WarnCount
    ;


    SELECT CheckCategory, CheckName, ResultCode, Details, NumericValue FROM @AnalysisResults ORDER BY CheckCategory, CheckName;



END;