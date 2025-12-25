using System;
using System.Collections.Generic;

namespace ShopDbLibrary.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Article { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string UnitMeasurement { get; set; } = null!;

    public int Price { get; set; }

    public byte Discount { get; set; }

    public byte Quantity { get; set; }

    public string? Discription { get; set; }

    public byte? Size { get; set; }

    public string? Color { get; set; }

    public string? Image { get; set; }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<CountProduct> CountProducts { get; set; } = new List<CountProduct>();

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
