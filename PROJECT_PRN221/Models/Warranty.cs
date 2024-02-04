using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Warranty
{
    public int WarrantyId { get; set; }

    public int? OrderdetailId { get; set; }

    public int? ProductId { get; set; }

    public int? CustomerId { get; set; }

    public string? ImageProduct { get; set; }

    public string? ProductStatus { get; set; }

    public DateTime? WarrantyDate { get; set; }

    public string? WarrantyStatus { get; set; }

    public int? WarrantyQuantity { get; set; }

    public string? ProductStatusAdmin { get; set; }

    public string? ImageProductAdmin { get; set; }

    public DateTime? WarrantyDateAdmin { get; set; }

    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual OrderDetail? Orderdetail { get; set; }

    public virtual Product? Product { get; set; }
}
