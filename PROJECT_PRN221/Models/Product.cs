using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? SubDescription { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public double? ListPrice { get; set; }

    public double? Discount { get; set; }

    public int? CategoryId { get; set; }

    public int? BrandId { get; set; }

    public string? Status { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Warranty> Warranties { get; } = new List<Warranty>();
}
