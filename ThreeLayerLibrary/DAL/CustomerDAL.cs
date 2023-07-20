using MySqlConnector;
using Model;
using System.Collections.Generic;
namespace DAL;
public class CustomerDAL
{
    private MySqlConnection connection = DbConfig.GetConnection();
    private string query = "";

    public Customer GetCustomer(MySqlDataReader reader)
    {
        Customer customer = new Customer();
        customer.CustomerID = reader.GetInt32("cus_id");
        customer.CustomerName = reader.GetString("name");
        customer.PhoneNumber = reader.GetString("phone_number");
        customer.Address = reader.GetString("address");
        return customer;
    }
    public List<Customer> GetCustomersByName(string name)
    {
        List<Customer> lstCustomer = new List<Customer>();
        try{
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
                
            }
            query = @"select * from customers where name like @name;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@name", "%"+name+"%");
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read()){
                lstCustomer.Add(GetCustomer(reader));
            }
        }
        catch(MySqlException ex){
            Console.WriteLine(ex.Message);
        }
        if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                
            }
        return lstCustomer;
    }
    public Customer GetCustomerByPhone(string phonenumber)
    {
        Customer customer = new Customer();
        try{
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
                
            }
            query = @"select * from customers where phone_number = @phonenumber;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@phonenumber", phonenumber);
            MySqlDataReader reader = command.ExecuteReader();
            if(reader.Read()){
                customer = GetCustomer(reader);
            }
        }
        catch(MySqlException ex){
            Console.WriteLine(ex.Message);
        }
        if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                
            }
        return customer;
    }
    public bool InsertCustomer(Customer newcustomer)
    {
        try{
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
                
            }
            query = @"select * from customers where phone_number = @phonenumber limit 1;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@phonenumber", newcustomer.PhoneNumber);
            MySqlDataReader reader = command.ExecuteReader();
            if(reader.Read()){
                return false;
            }
            reader.Close();
            query = @"insert into customers(name, phone_number, address) value(@name, @phonenumber, @address);";
            command = new MySqlCommand(query, connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@name", newcustomer.CustomerName);
            command.Parameters.AddWithValue("@phonenumber", newcustomer.PhoneNumber);
            command.Parameters.AddWithValue("@address", newcustomer.Address);
            command.ExecuteNonQuery();
        }
        catch(MySqlException ex){
            Console.WriteLine(ex.Message);
        }
        if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                
            }
        return true;
    }
}