using BusinessEnum;

namespace Model;
public class Order
{
    public int OrderID { get; set; }
    public DateTime CreateAt { get; set; }
    public Staff Seller { get; set; } = default!; 
    public Staff Accountant { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
    public OrderEnum.Status OrderStatus { get; set; } 
    public DiscountPolicy DiscountPolicy { get; set; } = default!;
    public decimal TotalPrice { get; set; }
}