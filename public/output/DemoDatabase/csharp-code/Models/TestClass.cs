using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Keyless]
public partial class TestClass
{
    [StringLength(128)]
    public string Name { get; set; } = null!;

    public int SchemaId { get; set; }
}
