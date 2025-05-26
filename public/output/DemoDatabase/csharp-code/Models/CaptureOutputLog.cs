using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("CaptureOutputLog", Schema = "tSQLt")]
public partial class CaptureOutputLog
{
    [Key]
    public int Id { get; set; }

    public string? OutputText { get; set; }
}
