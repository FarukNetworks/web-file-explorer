using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Index("CustomerId", Name = "IX_Addresses_CustomerID")]
public partial class Address
{
    [Key]
    [Column("AddressID")]
    public int AddressId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(50)]
    public string AddressType { get; set; } = null!;

    [StringLength(255)]
    public string StreetAddress { get; set; } = null!;

    [StringLength(100)]
    public string City { get; set; } = null!;

    [StringLength(100)]
    public string? StateProvince { get; set; }

    [StringLength(20)]
    public string PostalCode { get; set; } = null!;

    [StringLength(100)]
    public string Country { get; set; } = null!;

    public bool IsPrimary { get; set; }

    [Precision(3)]
    public DateTime DateCreated { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Addresses")]
    public virtual Customer Customer { get; set; } = null!;
}
