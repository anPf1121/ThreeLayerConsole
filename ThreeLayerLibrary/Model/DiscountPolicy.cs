namespace Model;
public class DiscountPolicy
{
    public int PolicyID { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Staff CreateBy { get; set; } = default!;
    public DateTime CreateAt { get; set; }
    public decimal MinimumPurchaseAmount { get; set; }
    public decimal MaximumPurchaseAmount { get; set; }
    public decimal MoneySupported { get; set; }
    public DateTime UpdateAt { get; set; }
    public Staff UpdateBy { get; set; } = default!;
    public decimal DiscountPrice { get; set; }
    public string Description { get; set; } = string.Empty;

}