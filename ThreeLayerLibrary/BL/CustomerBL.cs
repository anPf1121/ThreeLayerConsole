using Model;
using System;
using DAL;

namespace BL;
public class CustomerBL
{
    private CustomerDAL customerDAL = new CustomerDAL();

    public List<Customer>? GetCustomerByName(string customerName)
    {
        List<Customer> tempList = customerDAL.GetCustomersByName(customerName);
        if (tempList == null) return null;
        else
            return tempList;
    }
    public Customer? GetCustomerByPhone(string phone)
    {
        return customerDAL.GetCustomerByPhone(phone);
    }
    public bool InsertCustomer(Customer customer)
    {
        return customerDAL.InsertCustomer(customer);
    }
}