using System;

public class ProductOrderDTO
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public decimal? ItemPrice { get; set; }
    public int? Count { get; set; }
}
