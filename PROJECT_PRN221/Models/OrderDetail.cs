using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class OrderDetail
{
    public int OrderdetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public double? ListPrice { get; set; }

    public int? QuantityOrder { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<Warranty> Warranties { get; } = new List<Warranty>();
}
