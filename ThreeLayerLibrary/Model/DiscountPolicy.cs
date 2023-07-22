namespace Model;
public class DiscountPolicy
{
    public int PolicyID { get; set; }
    public string Title { get; set; } 
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Staff CreateBy { get; set; }
    public DateTime CreateAt { get; set; }
    public decimal MinimumPurchaseAmount { get; set; }
    public decimal MaximumPurchaseAmount { get; set; }
    public decimal MoneySupported { get; set; }
    public PhoneDetail PhoneDetail { get; set; }
    public DateTime UpdateAt { get; set; }
    public Staff UpdateBy { get; set; } = default!;
    public decimal DiscountPrice { get; set; }
    public string Description { get; set; } 
    public DiscountPolicy(int policyID, string title, DateTime fromDate, DateTime toDate, Staff createBy, DateTime createAt, decimal minimumPurchaseAmount, decimal maximumPurchaseAmount, decimal moneySupported, PhoneDetail phoneDetail, DateTime updateAt, Staff updateBy, decimal discountPrice, string description){
        this.PolicyID = policyID;
        this.Title = title;
        this.FromDate = fromDate;
        this.ToDate = toDate;
        this.CreateBy = createBy;
        this.CreateAt = createAt;
        this.MinimumPurchaseAmount = minimumPurchaseAmount;
        this.MaximumPurchaseAmount = maximumPurchaseAmount;
        this.MoneySupported = moneySupported;
        this.PhoneDetail = phoneDetail;
        this.UpdateAt = updateAt;
        this.UpdateBy = updateBy;
        this.DiscountPrice = discountPrice;
        this.Description = description;
    }
}