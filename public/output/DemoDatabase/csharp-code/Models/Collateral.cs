using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("LoanApplicationId", Name = "IX_Collaterals_LoanApplicationID")]
public partial class Collateral
{
    [Key]
    [Column("CollateralID")]
    public int CollateralId { get; set; }

    [Column("LoanApplicationID")]
    public int LoanApplicationId { get; set; }

    [StringLength(255)]
    public string Description { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CollateralType { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal EstimatedValue { get; set; }

    [StringLength(100)]
    public string? ValueSource { get; set; }

    [Precision(3)]
    public DateTime? VerificationDate { get; set; }

    [Precision(3)]
    public DateTime DateAdded { get; set; }

    [ForeignKey("LoanApplicationId")]
    [InverseProperty("Collaterals")]
    public virtual LoanApplication LoanApplication { get; set; } = null!;
}
