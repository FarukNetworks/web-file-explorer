using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("TestResult", Schema = "tSQLt")]
public partial class TestResult
{
    [Key]
    public int Id { get; set; }

    public string Class { get; set; } = null!;

    public string TestCase { get; set; } = null!;

    [StringLength(517)]
    public string? Name { get; set; }

    public string? TranName { get; set; }

    public string? Result { get; set; }

    public string? Msg { get; set; }

    public DateTime TestStartTime { get; set; }

    public DateTime? TestEndTime { get; set; }
}
