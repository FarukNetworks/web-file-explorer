using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("EmploymentHistory")]
[Index("CustomerId", "IsCurrent", Name = "IX_EmploymentHistory_CustomerID_IsCurrent", IsDescending = new[] { false, true })]
public partial class EmploymentHistory
{
    [Key]
    [Column("EmploymentHistoryID")]
    public int EmploymentHistoryId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(200)]
    public string EmployerName { get; set; } = null!;

    [StringLength(100)]
    public string? JobTitle { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EmploymentStatus { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsCurrent { get; set; }

    [Precision(3)]
    public DateTime? VerificationDate { get; set; }

    [StringLength(100)]
    public string? VerificationSource { get; set; }

    [Precision(3)]
    public DateTime DateAdded { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("EmploymentHistories")]
    public virtual Customer Customer { get; set; } = null!;
}
