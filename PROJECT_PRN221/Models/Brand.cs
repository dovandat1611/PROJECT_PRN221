using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string? BrandName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
