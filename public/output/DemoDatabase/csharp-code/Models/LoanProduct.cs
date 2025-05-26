using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

public partial class LoanProduct
{
    [Key]
    [Column("LoanProductID")]
    public int LoanProductId { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string ProductType { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(5, 4)")]
    public decimal InterestRate { get; set; }

    public int TermMonths { get; set; }

    public bool RequiresCollateral { get; set; }

    public int? MinCreditScoreRequired { get; set; }

    [Column("MaxDTIPercent", TypeName = "decimal(5, 2)")]
    public decimal? MaxDtipercent { get; set; }

    [Column("MaxLTVPercent", TypeName = "decimal(5, 2)")]
    public decimal? MaxLtvpercent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MinAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MaxAmount { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("ProposedLoanProduct")]
    public virtual ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();

    [InverseProperty("LoanProduct")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
