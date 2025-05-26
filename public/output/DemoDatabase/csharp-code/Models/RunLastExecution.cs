using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Keyless]
[Table("Run_LastExecution", Schema = "tSQLt")]
public partial class RunLastExecution
{
    public string? TestName { get; set; }

    public int? SessionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LoginTime { get; set; }
}
