using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("CustomerId", "IsActive", Name = "IX_FraudFlags_CustomerID_IsActive")]
public partial class FraudFlag
{
    [Key]
    [Column("FraudFlagID")]
    public int FraudFlagId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [Column("LoanApplicationID")]
    public int? LoanApplicationId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string FlagType { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Severity { get; set; } = null!;

    [StringLength(1000)]
    public string? Details { get; set; }

    public bool IsActive { get; set; }

    [Precision(3)]
    public DateTime DateRaised { get; set; }

    [Precision(3)]
    public DateTime? ResolvedDate { get; set; }

    [StringLength(128)]
    public string? ResolvedBy { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("FraudFlags")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("LoanApplicationId")]
    [InverseProperty("FraudFlags")]
    public virtual LoanApplication? LoanApplication { get; set; }
}
