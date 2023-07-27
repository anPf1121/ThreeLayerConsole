using Model;
using System;
using DAL;

namespace BL;
public class CustomerBL
{
    private CustomerDAL customerDAL = new CustomerDAL();

    public List<Customer> GetCustomerByName(string customerName)
    {
        List<Customer> tempList = customerDAL.GetCustomersByName(customerName);

            return tempList;
    }

    public bool InsertCustomer(Customer customer)
    {
        return customerDAL.InsertCustomer(customer);
    }
}