using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("LoanApplicationId", Name = "IX_ApprovalDecisions_LoanApplicationID", IsUnique = true)]
public partial class ApprovalDecision
{
    [Key]
    [Column("ApprovalDecisionID")]
    public int ApprovalDecisionId { get; set; }

    [Column("LoanApplicationID")]
    public int LoanApplicationId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Decision { get; set; } = null!;

    [Precision(3)]
    public DateTime DecisionDate { get; set; }

    [StringLength(128)]
    public string? DecisionBy { get; set; }

    [StringLength(1000)]
    public string? Reason { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ApprovedAmount { get; set; }

    [Column(TypeName = "decimal(5, 4)")]
    public decimal? ApprovedRate { get; set; }

    public int? ApprovedTerm { get; set; }

    [ForeignKey("LoanApplicationId")]
    [InverseProperty("ApprovalDecision")]
    public virtual LoanApplication LoanApplication { get; set; } = null!;
}
