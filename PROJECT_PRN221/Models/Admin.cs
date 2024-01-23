using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Gmail { get; set; }

    public DateTime? Dob { get; set; }

    public string? Image { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<News> News { get; } = new List<News>();
}
