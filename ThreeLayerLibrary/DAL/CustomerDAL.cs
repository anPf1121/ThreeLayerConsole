using MySqlConnector;
using Model;
using CustomerDTO;
namespace DAL;
public class CustomerDAL
{

    private MySqlConnection connection = DbConfig.GetConnection();
    private string query = "";

    public Customer GetCustomer(MySqlDataReader reader)
    {
        Customer customer = new Customer(
            reader.GetInt32("customer_id"),
            reader.GetString("name"),
            reader.GetString("phone_number"),
            reader.GetString("address")
        );
        return customer;
    }
    public List<Customer> GetCustomersByName(string name)
    {
        List<Customer> lstCustomer = new List<Customer>();
        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            query = @"select * from customers where name like @name;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@name", "%" + name + "%");
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lstCustomer.Add(GetCustomer(reader));
            }
            reader.Close();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
        return lstCustomer;
    }
    public Customer GetCustomerByID(int id)
    {
        Customer customer = new Customer(0, "", "", "");
        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();

            }
            query = @"select * from customers where customer_id = @id;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                customer = GetCustomer(reader);
            }
            reader.Close();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();

        }
        return customer;
    }
    public int CheckCustomerIsExist(Customer customer)
    {
        int result = 0;
            
       if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();

            }

            using (MySqlCommand command = new MySqlCommand("sp_createCustomer", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@customerPhone", customer.PhoneNumber);
                command.Parameters["@customerPhone"].Direction = System.Data.ParameterDirection.Input;
                command.Parameters.AddWithValue("@customerID", MySqlDbType.Int32);
                command.Parameters["@customerID"].Direction = System.Data.ParameterDirection.Output;
                command.ExecuteNonQuery();


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = reader.GetInt32("customer_id");
                    }
                }
            }

        return result;
    }
}