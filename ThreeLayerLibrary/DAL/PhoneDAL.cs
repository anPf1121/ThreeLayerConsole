using MySqlConnector;
using Model;
using BusinessEnum;
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
            phone.PhoneID = reader.GetInt32("phone_id");
            phone.PhoneName = reader.GetString("phone_name");
            phone.Brand.BrandName = reader.GetString("brand_name");
            phone.Camera = reader.GetString("camera");
            phone.RAM = reader.GetString("ram");
            phone.Weight = reader.GetString("weight");
            phone.Processor = reader.GetString("processor");
            phone.BatteryCapacity = reader.GetString("battery_capacity");
            phone.SimSlot = reader.GetString("sim_slot");
            phone.OS = reader.GetString("os");
            phone.Screen = reader.GetString("screen");
            phone.ChargePort = reader.GetString("charge_port");
            phone.ReleaseDate = reader.GetDateTime("release_date");
            phone.Connection = reader.GetString("connection");
            phone.Description = reader.GetString("description");
            phone.CreateAt = reader.GetDateTime("create_at");
            phone.CreateBy.StaffID = reader.GetInt32("create_by");
            phone.ROM.ROM = reader.GetString("rom_size");
            phone.Color.Color = reader.GetString("color_name");
            phone.Price = reader.GetDecimal("price");
            phone.Quantity = reader.GetInt32("quantity");
            phone.PhoneStatusType = (PhoneEnum.Status)Enum.ToObject(typeof(PhoneEnum.Status), reader.GetInt32("phone_status_type"));
            phone.UpdateAt = reader.GetDateTime("update_at");
            phone.UpdateBy.StaffID = reader.GetInt32("update_by");
            return phone;
        }
        public List<Phone> GetItems(int itemFilter, string? input)
        {
            List<Phone> lst = new List<Phone>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
                
            }
                MySqlCommand command = new MySqlCommand("", connection);
                switch (itemFilter)
                {
                    case ItemFilter.GET_ALL:
                        query = @"SELECT p.phone_id, p.phone_name, b.brand_name, p.camera, p.ram, p.weight, p.processor, p.battery_capacity,
                        p.sim_slot, p.os, p.screen,  p.charge_port, p.release_date, p.connection, p.description, p.create_at, p.create_by, r.rom_size,
                        c.color_name,  pd.price, pd.quantity, pd.phone_status_type, pd.update_at, pd.update_by
                        FROM phones p
                        inner join brands b on p.brand_id = b.brand_id
                        inner join phonedetails pd on p.phone_id = pd.phone_id
                        inner join colors c on c.color_id = pd.color_id
                        inner join romsizes r on r.rom_size_id = pd.rom_size_id;";
                        break;
                    case ItemFilter.FILTER_BY_ITEM_INFORMATION:
                        query = @"SELECT p.phone_id, p.phone_name, b.brand_name, p.camera, p.ram, p.weight, p.processor, p.battery_capacity,
                        p.sim_slot, p.os, p.screen,  p.charge_port, p.release_date, p.connection, p.description, p.create_at, p.create_by, r.rom_size,
                        c.color_name,  pd.price, pd.quantity, pd.phone_status_type, pd.update_at, pd.update_by
                        FROM phones p
                        inner join brands b on p.brand_id = b.brand_id
                        inner join phonedetails pd on p.phone_id = pd.phone_id
                        inner join colors c on c.color_id = pd.color_id
                        inner join romsizes r on r.rom_size_id = pd.rom_size_id
                        where p.phone_name like @input or b.brand_name like @input or p.camera like @input or 
                        p.ram like @input or p.weight like @input or p.processor like @input or 
                        p.battery_capacity like @input or p.sim_slot like @input or p.os like @input or p.screen like @input or
                        p.charge_port like @input or p.release_date like @input or p.connection like @input or 
                        p.description like @input or r.rom_size like @input or c.color_name like @input or pd.price like @input or
                        pd.phone_status_type like @input;";
                        break;
                    case ItemFilter.FILTER_BY_ITEM_HAVE_DISCOUNT:
                        query = @"SELECT p.phone_id, p.phone_name, b.brand_name, p.camera, p.ram, p.weight, p.processor, p.battery_capacity,
                        p.sim_slot, p.os, p.screen,  p.charge_port, p.release_date, p.connection, p.description, p.create_at, p.create_by, r.rom_size,
                        c.color_name,  pd.price, pd.quantity, pd.phone_status_type, pd.update_at, pd.update_by
                        FROM phones p
                        inner join brands b on p.brand_id = b.brand_id
                        inner join phonedetails pd on p.phone_id = pd.phone_id
                        inner join colors c on c.color_id = pd.color_id
                        inner join romsizes r on r.rom_size_id = pd.rom_size_id
                        inner join discountpolicies dp on dp.phone_detail_id = pd.phone_detail_id
                        where dp.discount_price != '0';";
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
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                
            }
            return lst;
        }
        public bool CheckImeiExist(string imei, Phone phone)
        {
            bool check = false;
            try
            {
                connection.Open();
                query = "select * from phonedetails where phone_imei = @phoneimei or phone_id = @phoneid or status = 'Not Export';";
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