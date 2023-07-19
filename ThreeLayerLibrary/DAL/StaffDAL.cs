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
                }
                reader.Close();
            }
            catch { }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            return staff;
        }
        public Staff GetStaff(MySqlDataReader reader)
        {
            Staff staff = new Staff();
            staff.StaffID = reader.GetInt32("Staff_ID");
            staff.StaffName = reader.GetString("Staff_Name");
            staff.Address = reader.GetString("Address");
            staff.UserName = reader.GetString("User_Name");
            staff.Password = reader.GetString("Password");
            staff.Role = (StaffEnum.Role)Enum.Parse(typeof(StaffEnum.Role), reader.GetString("Role"));
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