using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("Email", Name = "IX_Customers_Email", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(30)]
    public string? Phone { get; set; }

    [Precision(3)]
    public DateTime DateRegistered { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [InverseProperty("Customer")]
    public virtual ICollection<CommunicationPreference> CommunicationPreferences { get; set; } = new List<CommunicationPreference>();

    [InverseProperty("Customer")]
    public virtual ICollection<CreditScore> CreditScores { get; set; } = new List<CreditScore>();

    [InverseProperty("Customer")]
    public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();

    [InverseProperty("Customer")]
    public virtual ICollection<FraudFlag> FraudFlags { get; set; } = new List<FraudFlag>();

    [InverseProperty("Customer")]
    public virtual ICollection<IncomeVerification> IncomeVerifications { get; set; } = new List<IncomeVerification>();

    [InverseProperty("Customer")]
    public virtual ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();

    [InverseProperty("Customer")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    [InverseProperty("Customer")]
    public virtual ICollection<MarketingEmailsSent> MarketingEmailsSents { get; set; } = new List<MarketingEmailsSent>();
}
