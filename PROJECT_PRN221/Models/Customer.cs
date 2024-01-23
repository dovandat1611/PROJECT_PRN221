using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public string? Image { get; set; }

    public bool? Gender { get; set; }

    public string? Address { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Warranty> Warranties { get; } = new List<Warranty>();
}
