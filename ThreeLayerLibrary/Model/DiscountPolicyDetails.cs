namespace Model;
public class DiscountPolicyDetails
{
    public int PolicyDetailID { get; set; }
    public DiscountPolicy DiscountPolicy { get; set; }
    public Order Order { get; set; }
    public DiscountPolicyDetails(int policyDetailID, DiscountPolicy discountPolicy, Order order) {
        this.PolicyDetailID = policyDetailID;
        this.DiscountPolicy = discountPolicy;
        this.Order = order;
    }
}