using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("CustomerId", "DateChecked", Name = "IX_CreditScores_CustomerID_DateChecked", IsDescending = new[] { false, true })]
public partial class CreditScore
{
    [Key]
    [Column("CreditScoreID")]
    public int CreditScoreId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(50)]
    public string Provider { get; set; } = null!;

    public int Score { get; set; }

    public DateOnly DateChecked { get; set; }

    [Column(TypeName = "xml")]
    public string? ReportData { get; set; }

    public int? BankruptciesLast5Years { get; set; }

    public int? DelinquenciesLast2Years { get; set; }

    [Precision(3)]
    public DateTime DateRetrieved { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CreditScores")]
    public virtual Customer Customer { get; set; } = null!;
}
