using System.Diagnostics.Contracts;

namespace Model;
public class Customer
{
    public int CustomerID { get; set; }
    public string CustomerName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public Customer() {}
    public Customer(int customerID, string customerName, string phoneNumber, string address)
    {
        this.CustomerID = customerID;
        this.CustomerName = customerName;
        this.PhoneNumber = phoneNumber;
        this.Address = address;
    }
}