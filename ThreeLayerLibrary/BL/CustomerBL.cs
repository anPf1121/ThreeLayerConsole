using Model;
using System;
using DAL;
using CustomerDTO;

namespace BL;
public class CustomerBL
{
    private CustomerDAL customerDAL = new CustomerDAL();

    public int CheckCustomerIsExist(Customer customer)
    {
        return customerDAL.CheckCustomerIsExist(customer);
    }
    public Customer GetCustomerByID(int iD) {
        return customerDAL.GetCustomerByID(iD);
    }
}