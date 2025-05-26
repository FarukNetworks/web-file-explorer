using sql2code.Models;

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sql2code.Models;

[Table("MarketingEmailsSent")]
[Index("CampaignId", Name = "IX_MarketingEmailsSent_CampaignID")]
[Index("CustomerId", "SentDate", Name = "IX_MarketingEmailsSent_CustomerID_SentDate", IsDescending = new[] { false, true })]
[Index("SentDate", Name = "IX_MarketingEmailsSent_SentDate")]
public partial class MarketingEmailsSent
{
    [Key]
    [Column("EmailLogID")]
    public long EmailLogId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(255)]
    public string SentToEmailAddress { get; set; } = null!;

    [Column("CampaignID")]
    [StringLength(100)]
    [Unicode(false)]
    public string CampaignId { get; set; } = null!;

    [StringLength(500)]
    public string? SubjectLine { get; set; }

    [Precision(3)]
    public DateTime SentDate { get; set; }

    public bool WasOpened { get; set; }

    [Precision(3)]
    public DateTime? OpenedDate { get; set; }

    public bool WasClicked { get; set; }

    [Precision(3)]
    public DateTime? ClickedDate { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("MarketingEmailsSents")]
    public virtual Customer Customer { get; set; } = null!;
}
