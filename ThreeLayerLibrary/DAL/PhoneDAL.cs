using MySqlConnector;
using Model;
using BusinessEnum;
namespace DAL
{
    public static class PhoneFilter
    {
        public const int GET_ALL = 0;
        public const int FILTER_BY_PHONE_INFORMATION = 1;
        public const int FILTER_BY_PHONE_HAVE_DISCOUNT = 2;
    }
    public class PhoneDAL
    {
        private string query = "";
        public MySqlConnection connection = DbConfig.GetConnection();
        public Phone GetPhone(MySqlDataReader reader)
        {
            StaffDAL staffDAL = new StaffDAL();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            Phone phone = new Phone(
                reader.GetInt32("phone_id"),
                reader.GetString("phone_name"),
                phoneDetailsDAL.GetBrand(reader),
                reader.GetString("camera"),
                reader.GetString("ram"),
                reader.GetString("weight"),
                reader.GetString("processor"),
                reader.GetString("battery_capacity"),
                reader.GetString("sim_slot"),
                reader.GetString("os"),
                reader.GetString("screen"),
                reader.GetString("connection"),
                new List<PhoneDetail>(),
                reader.GetDateTime("release_date"),
                reader.GetString("charge_port"),
                staffDAL.GetStaff(reader),
                reader.GetDateTime("create_at"),
                reader.GetString("description")
            );
            return phone;
        }
        public Phone? GetPhoneById(int phoneID)
        {
            Phone? phone = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from phones P
                        INNER JOIN brands B ON P.brand_id = B.brand_id
                        INNER JOIN staffs S ON P.create_by = S.staff_id
                        INNER JOIN phonedetails PD ON P.phone_id = PD.phone_id;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phoneID", phoneID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) phone = GetPhone(reader);
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

            if (phone != null)
            {
                PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
                phone.PhoneDetails = phoneDetailsDAL.GetPhoneDetailsByPhoneID(phone.PhoneID);
            }

            return phone;
        }
        public List<Phone>? GetPhones(int phoneFilter, string? input)
        {
            List<Phone> lst = new List<Phone>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                MySqlCommand command = new MySqlCommand("", connection);
                switch (phoneFilter)
                {
                    case PhoneFilter.GET_ALL:
                        query = @"SELECT p.phone_id, p.phone_name, b.brand_name, p.camera, p.ram, p.weight, p.processor, p.battery_capacity,
                        p.sim_slot, p.os, p.screen,  p.charge_port, p.release_date, p.connection, p.description, p.create_at, p.create_by, r.rom_size,
                        c.color_name,  pd.price, pd.quantity, pd.phone_status_type, pd.update_at, pd.update_by
                        FROM phones p
                        inner join brands b on p.brand_id = b.brand_id
                        inner join phonedetails pd on p.phone_id = pd.phone_id
                        inner join colors c on c.color_id = pd.color_id
                        inner join romsizes r on r.rom_size_id = pd.rom_size_id;";
                        break;
                    case PhoneFilter.FILTER_BY_PHONE_INFORMATION:
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
                    case PhoneFilter.FILTER_BY_PHONE_HAVE_DISCOUNT:
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
                if (phoneFilter == PhoneFilter.FILTER_BY_PHONE_INFORMATION)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@input", "%" + input + "%");
                }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(GetPhone(reader));
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
            if (lst.Count() == 0) return null;
            return lst;
        }
    }
}

