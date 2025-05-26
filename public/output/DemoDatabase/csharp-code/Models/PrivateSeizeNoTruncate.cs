using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Keyless]
[Table("Private_Seize_NoTruncate", Schema = "tSQLt")]
public partial class PrivateSeizeNoTruncate
{
    public bool? NoTruncate { get; set; }

    [ForeignKey("NoTruncate")]
    public virtual PrivateSeize? NoTruncateNavigation { get; set; }
}
