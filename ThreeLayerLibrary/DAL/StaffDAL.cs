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
        public Staff? GetAccountByUsername(string userName)
        {
            Staff? staff = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = @"select * from staffs where user_name = @username;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@username", userName);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    staff = GetStaff(reader);
                    Console.WriteLine(staff.Password);
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
            return staff;
        }
        public Staff GetStaff(MySqlDataReader reader)
        {
            Staff staff = new Staff(
                reader.GetInt32("Staff_ID"),
                reader.GetString("Name"),
                reader.GetString("Phone_Number"),
                reader.GetString("Address"),
                reader.GetString("User_Name"),
                reader.GetString("Password"),
                (StaffEnum.Status)Enum.ToObject(typeof(StaffEnum.Status), reader.GetInt32("Status")),
                (StaffEnum.Role)Enum.ToObject(typeof(StaffEnum.Role), reader.GetInt32("Role"))
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
    }
}