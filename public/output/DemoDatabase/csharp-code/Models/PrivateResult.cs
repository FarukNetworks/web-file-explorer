using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Keyless]
public partial class PrivateResult
{
    public int? Severity { get; set; }

    public string? Result { get; set; }
}
