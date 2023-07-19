using MySqlConnector;
using Model;
namespace DAL;
public class CustomerDAL
{
    private MySqlConnection connection = DbConfig.GetConnection();
    private string query = "";
    public static class InformationFilter
    {
        public const int GET_BY_PHONE = 0;
        public const int GET_BY_ID = 1;
    }
    public Customer GetCustomer(MySqlDataReader reader)
    {
        Customer customer = new Customer();
        return customer;
    }
    public List<Customer> GetCustomersByName(string name)
    {
        List<Customer> lstCustomer = new List<Customer>();
        return lstCustomer;
    }
    public Customer? GetCustomerByInfo(int informationFilter, string info)
    {
        Customer? customer = null;
        return customer;
    }
    public bool InsertCustomer(Customer newcustomer)
    {
        return false;
    }
}