using BusinessEnum;

namespace Model;
public class Order
{
    public string OrderID { get; set; }
    public DateTime CreateAt { get; set; }
    public Staff Seller { get; set; }
    public Staff Accountant { get; set; }
    public Customer Customer { get; set; }
    public List<Imei> ListImeiInOrder { get; set; }
    public OrderEnum.Status OrderStatus { get; set; }
    public List<DiscountPolicy> DiscountPolicies { get; set; }
    public string PaymentMethod { get; set; }
    public decimal TotalDue { get; set; }
    public Order() { }
    //     Phone: “Iphone 11”
    // ROM: “512GB”
    // Color: “Pink”
    // Quantity: 1
    // Imei: “378541545434122”
    // Customer Name: "Nguyen Xuan Sinh"
    // Phone: "0904949176"
    // Address: “Hanoi”
    public Order(string orderID, DateTime createAt, Staff seller, Staff accountant, Customer customer, List<Imei> imeis, OrderEnum.Status orderStatus, List<DiscountPolicy> discountPolicies, string paymentMethod, decimal totaldue)
    {
        this.OrderID = orderID;
        this.CreateAt = createAt;
        this.Seller = seller;
        this.Accountant = accountant;
        this.Customer = customer;
        this.ListImeiInOrder = imeis;
        this.OrderStatus = orderStatus;
        this.DiscountPolicies = discountPolicies;
        this.PaymentMethod = paymentMethod;
        this.TotalDue = totaldue;
    }
    public decimal GetTotalDue()
    {
        decimal totalDue = 0;
        foreach (var item in ListImeiInOrder)
        {
            totalDue += item.PhoneDetail.Price;
        }
        return totalDue;
    }
    public decimal GetTotalDueForEachPhone(int phoneID)
    {
        decimal totalDue = 0;
        foreach (var item in this.ListImeiInOrder)
        {
            if (phoneID == item.PhoneDetail.PhoneDetailID)
                totalDue += item.PhoneDetail.Price;
        }
        return totalDue;
    }
}