using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using sql2code.Models;

namespace sql2code.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<sql2code.Models.Address> Addresses { get; set; }

    public virtual DbSet<sql2code.Models.ApprovalDecision> ApprovalDecisions { get; set; }

    public virtual DbSet<sql2code.Models.CaptureOutputLog> CaptureOutputLogs { get; set; }

    public virtual DbSet<sql2code.Models.Collateral> Collaterals { get; set; }

    public virtual DbSet<sql2code.Models.CommunicationPreference> CommunicationPreferences { get; set; }

    public virtual DbSet<sql2code.Models.CreditScore> CreditScores { get; set; }

    public virtual DbSet<sql2code.Models.Customer> Customers { get; set; }

    public virtual DbSet<sql2code.Models.EmploymentHistory> EmploymentHistories { get; set; }

    public virtual DbSet<sql2code.Models.FraudFlag> FraudFlags { get; set; }

    public virtual DbSet<sql2code.Models.IncomeVerification> IncomeVerifications { get; set; }

    public virtual DbSet<sql2code.Models.Loan> Loans { get; set; }

    public virtual DbSet<sql2code.Models.LoanApplication> LoanApplications { get; set; }

    public virtual DbSet<sql2code.Models.LoanPayment> LoanPayments { get; set; }

    public virtual DbSet<sql2code.Models.LoanProduct> LoanProducts { get; set; }

    public virtual DbSet<sql2code.Models.MarketingEmailsSent> MarketingEmailsSents { get; set; }

    public virtual DbSet<sql2code.Models.PrivateConfiguration> PrivateConfigurations { get; set; }

    public virtual DbSet<sql2code.Models.PrivateHostPlatform> PrivateHostPlatforms { get; set; }

    public virtual DbSet<sql2code.Models.PrivateNewTestClassList> PrivateNewTestClassLists { get; set; }

    public virtual DbSet<sql2code.Models.PrivateNoTransactionTableAction> PrivateNoTransactionTableActions { get; set; }

    public virtual DbSet<sql2code.Models.PrivateRenamedObjectLog> PrivateRenamedObjectLogs { get; set; }

    public virtual DbSet<sql2code.Models.PrivateResult> PrivateResults { get; set; }

    public virtual DbSet<sql2code.Models.PrivateSeize> PrivateSeizes { get; set; }

    public virtual DbSet<sql2code.Models.PrivateSeizeNoTruncate> PrivateSeizeNoTruncates { get; set; }

    public virtual DbSet<sql2code.Models.PrivateSysIndex> PrivateSysIndexes { get; set; }

    public virtual DbSet<sql2code.Models.PrivateSysType> PrivateSysTypes { get; set; }

    public virtual DbSet<sql2code.Models.RunLastExecution> RunLastExecutions { get; set; }

    public virtual DbSet<sql2code.Models.Test> Tests { get; set; }

    public virtual DbSet<sql2code.Models.TestClass> TestClasses { get; set; }

    public virtual DbSet<sql2code.Models.TestResult> TestResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<sql2code.Models.Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__091C2A1BB115FCF5");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addresses).HasConstraintName("FK_Addresses_Customers");
        });

        modelBuilder.Entity<sql2code.Models.ApprovalDecision>(entity =>
        {
            entity.HasKey(e => e.ApprovalDecisionId).HasName("PK__Approval__F2F287C8B1D741B8");

            entity.Property(e => e.DecisionDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.LoanApplication).WithOne(p => p.ApprovalDecision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalDecisions_LoanApplications");
        });

        modelBuilder.Entity<sql2code.Models.CaptureOutputLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CaptureO__3214EC079E940FA1");
        });

        modelBuilder.Entity<sql2code.Models.Collateral>(entity =>
        {
            entity.HasKey(e => e.CollateralId).HasName("PK__Collater__BB1A1FBCD1DCD2E1");

            entity.Property(e => e.DateAdded).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.Collaterals).HasConstraintName("FK_Collaterals_LoanApplications");
        });

        modelBuilder.Entity<sql2code.Models.CommunicationPreference>(entity =>
        {
            entity.HasKey(e => e.PreferenceId).HasName("PK__Communic__E228490FDB39BB85");

            entity.Property(e => e.LastChangedDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.CommunicationPreferences).HasConstraintName("FK_CommunicationPreferences_Customers");
        });

        modelBuilder.Entity<sql2code.Models.CreditScore>(entity =>
        {
            entity.HasKey(e => e.CreditScoreId).HasName("PK__CreditSc__145DECE5404C2CF4");

            entity.Property(e => e.DateRetrieved).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.CreditScores).HasConstraintName("FK_CreditScores_Customers");
        });

        modelBuilder.Entity<sql2code.Models.Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B898F340F8");

            entity.Property(e => e.DateRegistered).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<sql2code.Models.EmploymentHistory>(entity =>
        {
            entity.HasKey(e => e.EmploymentHistoryId).HasName("PK__Employme__850EEBBE31A65E3B");

            entity.Property(e => e.DateAdded).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsCurrent).HasComputedColumnSql("(CONVERT([bit],case when [EndDate] IS NULL then (1) else (0) end))", false);

            entity.HasOne(d => d.Customer).WithMany(p => p.EmploymentHistories).HasConstraintName("FK_EmploymentHistory_Customers");
        });

        modelBuilder.Entity<sql2code.Models.FraudFlag>(entity =>
        {
            entity.HasKey(e => e.FraudFlagId).HasName("PK__FraudFla__F9CF2FB240393589");

            entity.Property(e => e.DateRaised).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Customer).WithMany(p => p.FraudFlags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FraudFlags_Customers");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.FraudFlags).HasConstraintName("FK_FraudFlags_LoanApplications");
        });

        modelBuilder.Entity<sql2code.Models.IncomeVerification>(entity =>
        {
            entity.HasKey(e => e.IncomeVerificationId).HasName("PK__IncomeVe__FC9B0D1A3613CEB2");

            entity.Property(e => e.DateAdded).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.MonthlyAmount).HasComputedColumnSql("(case [Frequency] when 'Annual' then [VerifiedAmount]/(12.0) when 'Monthly' then [VerifiedAmount] when 'BiWeekly' then [VerifiedAmount]*((26.0)/(12.0)) when 'Weekly' then [VerifiedAmount]*((52.0)/(12.0))  end)", false);

            entity.HasOne(d => d.Customer).WithMany(p => p.IncomeVerifications).HasConstraintName("FK_IncomeVerifications_Customers");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.IncomeVerifications).HasConstraintName("FK_IncomeVerifications_LoanApplications");
        });

        modelBuilder.Entity<sql2code.Models.Loan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("PK__Loans__4F5AD4374B8CA696");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Loans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loans_Customers");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.Loans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loans_LoanApplications");

            entity.HasOne(d => d.LoanProduct).WithMany(p => p.Loans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loans_LoanProducts");
        });

        modelBuilder.Entity<sql2code.Models.LoanApplication>(entity =>
        {
            entity.HasKey(e => e.LoanApplicationId).HasName("PK__LoanAppl__F60027DD3CB24CDE");

            entity.Property(e => e.ApplicationDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.LoanApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoanApplications_Customers");

            entity.HasOne(d => d.ProposedLoanProduct).WithMany(p => p.LoanApplications).HasConstraintName("FK_LoanApplications_LoanProducts");
        });

        modelBuilder.Entity<sql2code.Models.LoanPayment>(entity =>
        {
            entity.HasKey(e => e.LoanPaymentId).HasName("PK__LoanPaym__5BA74D5C5A5A2919");

            entity.Property(e => e.DateRecorded).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Loan).WithMany(p => p.LoanPayments).HasConstraintName("FK_LoanPayments_Loans");
        });

        modelBuilder.Entity<sql2code.Models.LoanProduct>(entity =>
        {
            entity.HasKey(e => e.LoanProductId).HasName("PK__LoanProd__0D22CCE219183859");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<sql2code.Models.MarketingEmailsSent>(entity =>
        {
            entity.HasKey(e => e.EmailLogId).HasName("PK__Marketin__E8CB41EC81548152");

            entity.Property(e => e.SentDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.MarketingEmailsSents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MarketingEmailsSent_Customers");
        });

        modelBuilder.Entity<sql2code.Models.PrivateConfiguration>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Private___737584F7C1F9EC8B");
        });

        modelBuilder.Entity<sql2code.Models.PrivateHostPlatform>(entity =>
        {
            entity.ToView("Private_HostPlatform", "tSQLt");
        });

        modelBuilder.Entity<sql2code.Models.PrivateNewTestClassList>(entity =>
        {
            entity.HasKey(e => e.ClassName).HasName("PK__Private___F8BF561A10DE8253");
        });

        modelBuilder.Entity<sql2code.Models.PrivateNoTransactionTableAction>(entity =>
        {
            entity.ToView("Private_NoTransactionTableAction", "tSQLt");
        });

        modelBuilder.Entity<sql2code.Models.PrivateRenamedObjectLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Private_RenamedObjectLog__Id");
        });

        modelBuilder.Entity<sql2code.Models.PrivateResult>(entity =>
        {
            entity.ToView("Private_Results", "tSQLt");
        });

        modelBuilder.Entity<sql2code.Models.PrivateSeize>(entity =>
        {
            entity.HasKey(e => e.Kaput).HasName("Private_Seize:PK");

            entity.ToTable("Private_Seize", "tSQLt", tb => tb.HasTrigger("Private_Seize_Stop"));
        });

        modelBuilder.Entity<sql2code.Models.PrivateSeizeNoTruncate>(entity =>
        {
            entity.HasOne(d => d.NoTruncateNavigation).WithMany().HasConstraintName("Private_Seize_NoTruncate(NoTruncate):FK");
        });

        modelBuilder.Entity<sql2code.Models.PrivateSysIndex>(entity =>
        {
            entity.ToView("Private_SysIndexes", "tSQLt");

            entity.Property(e => e.TypeDesc).UseCollation("Latin1_General_CI_AS_KS_WS");
        });

        modelBuilder.Entity<sql2code.Models.PrivateSysType>(entity =>
        {
            entity.ToView("Private_SysTypes", "tSQLt");
        });

        modelBuilder.Entity<sql2code.Models.Test>(entity =>
        {
            entity.ToView("Tests", "tSQLt");
        });

        modelBuilder.Entity<sql2code.Models.TestClass>(entity =>
        {
            entity.ToView("TestClasses", "tSQLt");
        });

        modelBuilder.Entity<sql2code.Models.TestResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK:tSQLt.TestResult");

            entity.Property(e => e.Name).HasComputedColumnSql("((quotename([Class])+'.')+quotename([TestCase]))", false);
            entity.Property(e => e.TestStartTime).HasDefaultValueSql("(sysdatetime())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
