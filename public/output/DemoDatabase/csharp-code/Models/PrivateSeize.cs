using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("Private_Seize", Schema = "tSQLt")]
public partial class PrivateSeize
{
    [Key]
    public bool Kaput { get; set; }
}
