using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("CustomerId", "Channel", Name = "UK_CommunicationPreferences_CustomerChannel", IsUnique = true)]
public partial class CommunicationPreference
{
    [Key]
    [Column("PreferenceID")]
    public int PreferenceId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Channel { get; set; } = null!;

    public bool OptInStatus { get; set; }

    [Precision(3)]
    public DateTime LastChangedDate { get; set; }

    [StringLength(100)]
    public string? Source { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CommunicationPreferences")]
    public virtual Customer Customer { get; set; } = null!;
}
