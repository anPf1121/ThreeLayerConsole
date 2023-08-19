using MySqlConnector;
using Model;
using BusinessEnum;

namespace DAL
{
    public class PhoneDetailFilter
    {
        public const int NULL_PARAMETER = 0;
        public const int CHANGE_IMEI_STATUS_TO_INORDER = 1;
        public const int CHANGE_IMEI_STATUS_TO_EXPORT = 2;
        public const int CHANGE_IMEI_STATUS_TO_NOTEXPORT = 3;
        public const int GET_PHONE_DETAIL_BY_PHONE_ID = 4;
        public const int GET_PHONE_DETAIL_IN_ORDER = 5;
        public const int GET_PHONE_DETAIL_FOR_TRADEIN = 6;
        public const int GET_IMEIS_BY_PHONE_DETAIL_ID = 7;
        public const int GET_IMEIS_IN_ORDER = 8;
    }
    public class PhoneDetailsDAL
    {
        private string query = "";
        public MySqlConnection connection = DbConfig.GetConnection();
        public Brand GetBrand(MySqlDataReader reader)
        {
            Brand brand = new Brand(
                reader.GetInt32("brand_id"),
                reader.GetString("brand_name"),
                reader.GetString("website")
            );
            return brand;
        }
        public bool UpdateImeiStatus(string imei, int imeiFilter)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                switch (imeiFilter)
                {
                    case PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_INORDER:
                        query = $@"UPDATE imeis SET status = 2 WHERE phone_imei = '{imei}';";
                        break;
                    case PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_EXPORT:
                        query = $@"UPDATE imeis SET status = 1 WHERE phone_imei = '{imei}';";
                        break;
                    case PhoneDetailFilter.CHANGE_IMEI_STATUS_TO_NOTEXPORT:
                        query = $@"UPDATE imeis SET status = 0 WHERE phone_imei = '{imei}';";
                        break;
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return true;
        }
        public Brand GetBrandByID(int id)
        {
            Brand output = new Brand(0, "", "");
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from brands where brand_id = @brandid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@brandid", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) output = GetBrand(reader);
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
        public ROMSize GetROMSize(MySqlDataReader reader)
        {
            ROMSize romsize = new ROMSize(
                    reader.GetInt32("rom_size_id"),
                    reader.GetString("rom_size")
                );
            return romsize;
        }
        public ROMSize GetROMSizeByID(int id)
        {
            ROMSize output = new ROMSize(0, "");
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from romsizes where rom_size_id = @romsizeid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@romsizeid", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) output = GetROMSize(reader);
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
        public PhoneColor GetPhoneColorByID(int id)
        {
            PhoneColor output = new PhoneColor(0, "");
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from colors where color_id = @colorid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@colorid", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) output = GetPhoneColor(reader);
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
        public PhoneColor GetPhoneColor(MySqlDataReader reader)
        {
            PhoneColor phoneColor = new PhoneColor(
                    reader.GetInt32("color_id"),
                    reader.GetString("color_name")
                );
            return phoneColor;
        }
        public Imei GetImei(MySqlDataReader reader)
        {
            Imei imei = new Imei(
                reader.GetString("phone_imei"),
                (PhoneEnum.ImeiStatus)Enum.ToObject(typeof(PhoneEnum.ImeiStatus), reader.GetInt32("status"))
            );
            return imei;
        }
        public PhoneDetail GetPhoneDetail(MySqlDataReader reader)
        {
            PhoneDAL phoneDAL = new PhoneDAL();
            StaffDAL staffDAL = new StaffDAL();
            PhoneDetail phoneDetail = new PhoneDetail(
                reader.GetInt32("phone_detail_id"),
                new Phone(reader.GetInt32("phone_id"), "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "", new DateTime(), "", new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""),
                new ROMSize(reader.GetInt32("rom_size_id"), ""),
                new PhoneColor(reader.GetInt32("color_id"), ""),
                reader.GetDecimal("price"),
                reader.GetInt32("quantity"),
                (PhoneEnum.Status)Enum.ToObject(typeof(PhoneEnum.Status), reader.GetInt32("phone_status_type")),
                new List<Imei>(),
                new Staff(reader.GetInt32("update_by"), "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active),
                reader.GetDateTime("update_at")
            );
            return phoneDetail;
        }
        public PhoneDetail GetPhoneDetailByID(int phonedetailid)
        {
            PhoneDetail output = new PhoneDetail(0, new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "", new DateTime(), "", new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new List<Imei>(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime());
            PhoneDAL phoneDAL = new PhoneDAL();
            StaffDAL staffDAL = new StaffDAL();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from phonedetails where phone_detail_id = @phonedetailid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phonedetailid", phonedetailid);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    output = GetPhoneDetail(reader);
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
            output.Phone = phoneDAL.GetPhoneById(output.Phone.PhoneID);
            output.ROMSize = GetROMSizeByID(output.ROMSize.ROMID);
            output.PhoneColor = GetPhoneColorByID(output.PhoneColor.ColorID);
            output.UpdateBy = staffDAL.GetStaffByID(output.UpdateBy.StaffID);
            output.ListImei = GetImeis(phonedetailid);
            return output;
        }
        public List<PhoneDetail> GetPhoneDetails(string id, int phoneDetailsFilter)
        {
            List<PhoneDetail> phoneDetails = new List<PhoneDetail>();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                MySqlCommand command = new MySqlCommand("", connection);
                switch (phoneDetailsFilter)
                {
                    case PhoneDetailFilter.GET_PHONE_DETAIL_BY_PHONE_ID:
                        command.CommandText = @"SELECT * FROM phonedetails where phone_id = @phoneid and phone_detail_id >0;";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@phoneID", int.Parse(id));
                        break;
                    case PhoneDetailFilter.GET_PHONE_DETAIL_FOR_TRADEIN:
                        command.CommandText = @"select * from phonedetails where phone_status_type != 0;";
                        command.Parameters.Clear();
                        break;
                    case PhoneDetailFilter.GET_PHONE_DETAIL_IN_ORDER:
                        command.CommandText = @"select pd.* from orders o 
                inner join orderdetails os on o.order_id = os.order_id
                inner join imeis i on i.phone_imei = os.phone_imei
                inner join phonedetails pd on pd.phone_detail_id = i.phone_detail_id
                where o.order_id = @orderid;";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@orderid", id);
                        break;
                    default:
                        break;
                }

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    phoneDetails.Add(GetPhoneDetail(reader));
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
            List<PhoneDetail> output = new List<PhoneDetail>();
            foreach (var p in phoneDetails)
            {
                output.Add(GetPhoneDetailByID(p.PhoneDetailID));
            }

            return output;
        }
        public List<Imei> GetImeis(int phonedetailid)
        {
            List<Imei> output = new List<Imei>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                MySqlCommand command = new MySqlCommand("", connection);
                command.CommandText = @"select * from imeis 
                where phone_detail_id = @phone_detail_id and status = 0;";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phone_detail_id", phonedetailid);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    output.Add(GetImei(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return output;
        }
    }
}


