using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Keyless]
public partial class PrivateSysType
{
    [Column("name")]
    [StringLength(128)]
    public string Name { get; set; } = null!;

    [Column("system_type_id")]
    public byte SystemTypeId { get; set; }

    [Column("user_type_id")]
    public int UserTypeId { get; set; }

    [Column("schema_id")]
    public int SchemaId { get; set; }

    [Column("principal_id")]
    public int? PrincipalId { get; set; }

    [Column("max_length")]
    public short MaxLength { get; set; }

    [Column("precision")]
    public byte Precision { get; set; }

    [Column("scale")]
    public byte Scale { get; set; }

    [Column("collation_name")]
    [StringLength(128)]
    public string? CollationName { get; set; }

    [Column("is_nullable")]
    public bool? IsNullable { get; set; }

    [Column("is_user_defined")]
    public bool IsUserDefined { get; set; }

    [Column("is_assembly_type")]
    public bool IsAssemblyType { get; set; }

    [Column("default_object_id")]
    public int DefaultObjectId { get; set; }

    [Column("rule_object_id")]
    public int RuleObjectId { get; set; }

    [Column("is_table_type")]
    public bool IsTableType { get; set; }
}
