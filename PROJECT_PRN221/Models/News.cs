using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class News
{
    public int NewsId { get; set; }

    public int? NewsgroupId { get; set; }

    public string? Image { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int? Createdby { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public virtual Admin? CreatedbyNavigation { get; set; }

    public virtual NewsGroup? Newsgroup { get; set; }
}
