using System;
using System.Collections.Generic;

namespace ShopDbLibrary.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly DateOrder { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int CodeReceive { get; set; }

    public int StatusId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<CountProduct> CountProducts { get; set; } = new List<CountProduct>();

    public virtual Status Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
