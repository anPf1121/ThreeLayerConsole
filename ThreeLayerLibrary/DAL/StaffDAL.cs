using MySqlConnector;
using Model;
using BusinessEnum;
using System.Text;
using System.Security.Cryptography;

namespace DAL
{
    public class StaffDAL
    {
        private MySqlConnection connection = DbConfig.GetConnection();
        private string query = "";
        public Staff GetAccountByUsername(string userName)
        {
            Staff staff = new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active);
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    query = @"select * from staffs where user_name = @username;";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@username", userName);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        staff = GetStaff(reader);
                    }
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            return staff;
        }
        public Staff GetStaff(MySqlDataReader reader)
        {
            Staff staff = new Staff(
                reader.GetInt32("Staff_ID"),
                reader.GetString("Name"),
                reader.GetString("Phone_Number"),
                reader.GetString("user_name"),
                reader.GetString("password"),
                reader.GetString("address"),
                (StaffEnum.Role)Enum.ToObject(typeof(StaffEnum.Role), reader.GetInt32("role")),
                (StaffEnum.Status)Enum.ToObject(typeof(StaffEnum.Status), reader.GetInt32("status"))
            );
            return staff;
        }
        public string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input); // Chuyển đổi chuỗi thành mảng byte

                byte[] hashBytes = md5.ComputeHash(inputBytes); // Tính toán MD5 hash

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Chuyển đổi mỗi byte thành chuỗi hexa và nối vào StringBuilder
                }
                return sb.ToString();
            }
        }
        public Staff GetStaffByID(int id)
        {
            Staff output = new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active);
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from staffs where staff_id = @id;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) output = GetStaff(reader);
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
            return output;
        }
    }
}