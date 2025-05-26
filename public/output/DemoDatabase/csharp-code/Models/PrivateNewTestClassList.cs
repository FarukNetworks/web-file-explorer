using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("Private_NewTestClassList", Schema = "tSQLt")]
public partial class PrivateNewTestClassList
{
    [Key]
    public string ClassName { get; set; } = null!;
}
