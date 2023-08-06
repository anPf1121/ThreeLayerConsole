using BusinessEnum;

namespace Model;
public class Order
{
    public string OrderID { get; set; }
    public DateTime CreateAt { get; set; }
    public Staff Seller { get; set; }
    public Staff Accountant { get; set; }
    public Customer Customer { get; set; }
    public List<PhoneDetail> PhoneDetails { get; set; }
    public OrderEnum.Status OrderStatus { get; set; }
    public List<DiscountPolicy> DiscountPolicies { get; set; }
    public string PaymentMethod { get; set; }
    public decimal TotalDue { get; set; }
    public Order() {}
    public Order(string orderID, DateTime createAt, Staff seller, Staff accountant, Customer customer, List<PhoneDetail> phones, OrderEnum.Status orderStatus, List<DiscountPolicy> discountPolicies, string paymentMethod, decimal totaldue)
    {
        this.OrderID = orderID;
        this.CreateAt = createAt;
        this.Seller = seller;
        this.Accountant = accountant;
        this.Customer = customer;
        this.PhoneDetails = phones;
        this.OrderStatus = orderStatus;
        this.DiscountPolicies = discountPolicies;
        this.PaymentMethod = paymentMethod;
        this.TotalDue = totaldue;
    }
    public decimal GetTotalDue()
    {
        decimal totalDue = 0;
        foreach (var item in PhoneDetails)
        {
            totalDue += item.ListImei.Count() * item.Price;
        }
        return totalDue;
    }
    public decimal GetTotalDueForEachPhone() {
        decimal totalDue = 0;
        foreach (var item in PhoneDetails)
        {
            totalDue = item.Quantity * item.Price;
        }
        return totalDue;
    }
}