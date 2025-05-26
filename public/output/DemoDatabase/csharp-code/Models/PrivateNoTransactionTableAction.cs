using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Keyless]
public partial class PrivateNoTransactionTableAction
{
    public string? Name { get; set; }

    public string? Action { get; set; }
}
