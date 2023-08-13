using Model;
using System;
using DAL;
using CustomerDTO;

namespace BL;
public class CustomerBL
{
    private CustomerDAL customerDAL = new CustomerDAL();

    public List<Customer> GetCustomerByName(string customerName)
    {
        List<Customer> tempList = customerDAL.GetCustomersByName(customerName);

            return tempList;
    }

    public CustomerResultDTO AddCustomer(Customer customer)
    {
        return customerDAL.AddCustomer(customer);
    }
    public Customer GetCustomerByID(int iD) {
        return customerDAL.GetCustomerByID(iD);
    }
}