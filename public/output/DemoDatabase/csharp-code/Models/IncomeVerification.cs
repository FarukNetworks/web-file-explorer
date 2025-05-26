using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("CustomerId", "VerificationDate", Name = "IX_IncomeVerifications_CustomerID_VerificationDate", IsDescending = new[] { false, true })]
public partial class IncomeVerification
{
    [Key]
    [Column("IncomeVerificationID")]
    public int IncomeVerificationId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column("LoanApplicationID")]
    public int? LoanApplicationId { get; set; }

    [StringLength(100)]
    public string Source { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ReportedAmount { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Frequency { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? VerifiedAmount { get; set; }

    [Column(TypeName = "numeric(28, 8)")]
    public decimal? MonthlyAmount { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Precision(3)]
    public DateTime? VerificationDate { get; set; }

    [StringLength(128)]
    public string? VerifiedBy { get; set; }

    [Precision(3)]
    public DateTime DateAdded { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("IncomeVerifications")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("LoanApplicationId")]
    [InverseProperty("IncomeVerifications")]
    public virtual LoanApplication? LoanApplication { get; set; }
}
