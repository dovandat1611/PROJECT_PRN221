using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
