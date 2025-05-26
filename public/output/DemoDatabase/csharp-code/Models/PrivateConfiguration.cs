using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("Private_Configurations", Schema = "tSQLt")]
public partial class PrivateConfiguration
{
    [Key]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "sql_variant")]
    public object? Value { get; set; }
}
