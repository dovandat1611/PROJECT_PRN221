using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? ProductId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? CommentDate { get; set; }

    public string? CommentContent { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
