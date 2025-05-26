using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("CustomerId", "Status", Name = "IX_Loans_CustomerID_Status")]
[Index("Status", "StartDate", Name = "IX_Loans_Status_StartDate")]
public partial class Loan
{
    [Key]
    [Column("LoanID")]
    public int LoanId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column("LoanApplicationID")]
    public int LoanApplicationId { get; set; }

    [Column("LoanProductID")]
    public int LoanProductId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PrincipalAmount { get; set; }

    [Column(TypeName = "decimal(5, 4)")]
    public decimal InterestRate { get; set; }

    public int TermMonths { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MonthlyPaymentAmount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly ExpectedEndDate { get; set; }

    public DateOnly? PaidOffDate { get; set; }

    [Precision(3)]
    public DateTime DateCreated { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Loans")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("LoanApplicationId")]
    [InverseProperty("Loans")]
    public virtual LoanApplication LoanApplication { get; set; } = null!;

    [InverseProperty("Loan")]
    public virtual ICollection<LoanPayment> LoanPayments { get; set; } = new List<LoanPayment>();

    [ForeignKey("LoanProductId")]
    [InverseProperty("Loans")]
    public virtual LoanProduct LoanProduct { get; set; } = null!;
}
