using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("LoanId", "PaymentDate", Name = "IX_LoanPayments_LoanID_PaymentDate", IsDescending = new[] { false, true })]
[Index("PaymentDate", Name = "IX_LoanPayments_PaymentDate")]
public partial class LoanPayment
{
    [Key]
    [Column("LoanPaymentID")]
    public long LoanPaymentId { get; set; }

    [Column("LoanID")]
    public int LoanId { get; set; }

    [Precision(3)]
    public DateTime PaymentDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentMethod { get; set; }

    [Column("TransactionID")]
    [StringLength(255)]
    public string? TransactionId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PrincipalComponent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal InterestComponent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? EscrowComponent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? FeeComponent { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Precision(3)]
    public DateTime DateRecorded { get; set; }

    [ForeignKey("LoanId")]
    [InverseProperty("LoanPayments")]
    public virtual Loan Loan { get; set; } = null!;
}
