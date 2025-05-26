using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("ApplicationDate", Name = "IX_LoanApplications_ApplicationDate")]
[Index("CustomerId", Name = "IX_LoanApplications_CustomerID")]
[Index("Status", Name = "IX_LoanApplications_Status")]
public partial class LoanApplication
{
    [Key]
    [Column("LoanApplicationID")]
    public int LoanApplicationId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column("ProposedLoanProductID")]
    public int? ProposedLoanProductId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal RequestedAmount { get; set; }

    [StringLength(200)]
    public string LoanPurpose { get; set; } = null!;

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Precision(3)]
    public DateTime ApplicationDate { get; set; }

    [Precision(3)]
    public DateTime? SubmittedDate { get; set; }

    [Precision(3)]
    public DateTime? LastUpdatedDate { get; set; }

    [InverseProperty("LoanApplication")]
    public virtual ApprovalDecision? ApprovalDecision { get; set; }

    [InverseProperty("LoanApplication")]
    public virtual ICollection<Collateral> Collaterals { get; set; } = new List<Collateral>();

    [ForeignKey("CustomerId")]
    [InverseProperty("LoanApplications")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("LoanApplication")]
    public virtual ICollection<FraudFlag> FraudFlags { get; set; } = new List<FraudFlag>();

    [InverseProperty("LoanApplication")]
    public virtual ICollection<IncomeVerification> IncomeVerifications { get; set; } = new List<IncomeVerification>();

    [InverseProperty("LoanApplication")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    [ForeignKey("ProposedLoanProductId")]
    [InverseProperty("LoanApplications")]
    public virtual LoanProduct? ProposedLoanProduct { get; set; }
}
