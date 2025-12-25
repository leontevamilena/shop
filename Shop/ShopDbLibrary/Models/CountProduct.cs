using System;
using System.Collections.Generic;

namespace ShopDbLibrary.Models;

public partial class CountProduct
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public byte Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
