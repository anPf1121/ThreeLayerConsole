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
                new Brand(reader.GetInt32("brand_id"), "", ""),
                reader.GetString("camera"),
                reader.GetString("ram"),
                reader.GetString("weight"),
                reader.GetString("processor"),
                reader.GetString("battery_capacity"),
                reader.GetString("sim_slot"),
                reader.GetString("os"),
                reader.GetString("screen"),
                reader.GetString("connection"),
                reader.GetDateTime("release_date"),
                reader.GetString("charge_port"),
                new Staff(reader.GetInt32("create_by"), "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active),
                reader.GetDateTime("create_at"),
                reader.GetString("description")
            );

            return phone;
        }
        public Phone GetPhoneById(int phoneID)
        {
            Phone phone = new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "", new DateTime(), "", new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), "");
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from phones where phone_id = @phoneid;";
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
            phone.CreateBy = new StaffDAL().GetStaffByID(phone.CreateBy.StaffID);
            phone.Brand = new PhoneDetailsDAL().GetBrandByID(phone.Brand.BrandID);

            return phone;
        }
        public List<Phone> GetPhones(int phoneFilter, string? input)
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
                        query = @"SELECT p.*, b.*, s.*, pd.* from phones p
                        inner join brands b on p.brand_id = b.brand_id
                        inner join staffs s on s.staff_id = p.create_by
                        inner join phonedetails pd on p.phone_id = pd.phone_id
                        inner join colors c on c.color_id = pd.color_id
                        inner join romsizes r on r.rom_size_id = pd.rom_size_id where p.phone_id >0 order by p.phone_id asc;";
                        break;
                    case PhoneFilter.FILTER_BY_PHONE_INFORMATION:
                        query = @"SELECT p.*, b.*, s.*, pd.*, c.*, r.*
                        FROM phones p
                        inner join brands b on p.brand_id = b.brand_id
                        inner join staffs s on s.staff_id = p.create_by
                        inner join phonedetails pd on p.phone_id = pd.phone_id
                        inner join colors c on c.color_id = pd.color_id
                        inner join romsizes r on r.rom_size_id = pd.rom_size_id
                        where (concat(p.phone_name,b.brand_name, r.rom_size, p.ram, p.camera, p.battery_capacity, p.os, p.screen, c.color_name) like @input) and p.phone_id > 0 order by p.phone_id asc;";
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

            //Xu li cac phone trung lap:
            List<Phone> output = new List<Phone>();
            foreach (var p in lst)
            {
                int count = 0;
                foreach (var o in output)
                {
                    if (o.PhoneName == p.PhoneName && o.PhoneID == p.PhoneID) count++;
                }
                if (count == 0) output.Add(GetPhoneById(p.PhoneID));
            }
            return output;
        }
    }
}