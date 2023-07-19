using MySqlConnector;
using Model;

namespace DAL
{
    public static class ItemFilter
    {
        public const int GET_ALL = 0;
        public const int FILTER_BY_ITEM_INFORMATION = 1;
        public const int FILTER_BY_ITEM_HAVE_DISCOUNT = 2;
    }
    public class PhoneDAL
    {
        private string query = "";
        public MySqlConnection connection = DbConfig.GetConnection();
        public Phone GetItemById(int itemId)
        {
            connection.Open();
            Phone phone = new Phone();
           
            return phone;
        }
        public Phone GetItem(MySqlDataReader reader)
        {
            Phone phone = new Phone();

            return phone;
        }
        public List<Phone> GetItems(int itemFilter, string? input)
        {
            List<Phone> lst = new List<Phone>();
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                switch (itemFilter)
                {
                    case ItemFilter.GET_ALL:
                        query = @"SELECT * FROM phones";
                        break;
                    case ItemFilter.FILTER_BY_ITEM_INFORMATION:

                        query = @"SELECT * FROM phones WHERE Phone_Name LIKE @input
                OR Brand LIKE @input OR CPU LIKE @input OR RAM LIKE @input OR Battery_Capacity LIKE @input OR OS LIKE @input
                OR Sim_Slot LIKE @input OR Screen_Hz LIKE @input OR Screen_Resolution LIKE @input OR ROM LIKE @input OR Mobile_Network LIKE @input 
                OR Phone_Size LIKE @input OR Price LIKE @input OR DiscountPrice LIKE @input;";
                        break;
                    case ItemFilter.FILTER_BY_ITEM_HAVE_DISCOUNT:
                        query = @"SELECT * FROM phones p
                        inner join phonediscount pd on p.phone_id = pd.phone_id
                        inner join discountpolicies dp on dp.policy_id = pd.policy_id
                        where p.DiscountPrice != '0' and current_timestamp() >= dp.from_date and current_timestamp() <= dp.to_date;";
                        break;
                    default:
                        break;
                }
                command.CommandText = query;
                if (itemFilter == ItemFilter.FILTER_BY_ITEM_INFORMATION)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@input", "%" + input + "%");
                }
                MySqlDataReader reader = command.ExecuteReader();
                lst = new List<Phone>();
                while (reader.Read())
                {
                    lst.Add(GetItem(reader));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! " + ex);
            }
            connection.Close();
            return lst;
        }
        public bool CheckImeiExist(string imei, Phone phone)
        {
            bool check = false;
            try
            {
                connection.Open();
                query = "select * from phonedetails where phone_imei = @phoneimei and phone_id = @phoneid and status = 'Not Export';";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phoneimei", imei);
                command.Parameters.AddWithValue("@phoneid", phone.PhoneID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) check = true;
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            connection.Close();
            if (check) return true;
            else return false;
        }
    }
}