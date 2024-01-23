using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string? NameReceiver { get; set; }

    public string? PhoneReceiver { get; set; }

    public string? AddressReceiver { get; set; }

    public double? TotalPrice { get; set; }

    public DateTime? OderDate { get; set; }

    public string? Payment { get; set; }

    public string? Status { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
