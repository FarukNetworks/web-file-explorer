using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("Private_RenamedObjectLog", Schema = "tSQLt")]
public partial class PrivateRenamedObjectLog
{
    [Key]
    public int Id { get; set; }

    public int ObjectId { get; set; }

    public string OriginalName { get; set; } = null!;
}
