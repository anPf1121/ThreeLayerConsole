using MySqlConnector;
using Model;
using BusinessEnum;

namespace DAL
{
    public class PhoneDetailFilter
    {
        public const int CHANGE_IMEI_STATUS_TO_INORDER = 1;
        public const int CHANGE_IMEI_STATUS_TO_EXPORT = 2;
        public const int CHANGE_IMEI_STATUS_TO_NOTEXPORT = 3;
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
        public PhoneColor GetPhoneColoreByID(int id)
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
                new PhoneDetail(reader.GetInt32("phone_detail_id"), new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "", new DateTime(), "", new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime()),
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
                new Staff(reader.GetInt32("update_by"), "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active),
                reader.GetDateTime("update_at")
            );
            return phoneDetail;
        }
        public PhoneDetail GetPhoneDetailByID(int phonedetailid)
        {
            PhoneDetail output = new PhoneDetail(0, new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "", new DateTime(), "", new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime());
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
            output.PhoneColor = GetPhoneColoreByID(output.PhoneColor.ColorID);
            output.UpdateBy = staffDAL.GetStaffByID(output.UpdateBy.StaffID);
            return output;
        }
        public List<PhoneDetail> GetPhoneDetailsByPhoneID(int phoneID)
        {
            List<PhoneDetail> phoneDetails = new List<PhoneDetail>();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"SELECT * FROM phonedetails where phone_id = @phoneid and phone_detail_id >0;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phoneID", phoneID);
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
        public PhoneDetail GetPhoneDetailByImei(string phoneImei)
        {
            PhoneDetail phoneDetail = new PhoneDetail();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from imeis I
                INNER JOIN phonedetails PD ON PD.phone_detail_id = I.phone_detail_id
                INNER JOIN phones P ON P.phone_id = PD.phone_id
                WHERE I.phone_imei = @phone_imei;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phone_imei", phoneImei);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    phoneDetail = GetPhoneDetail(reader);
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
            phoneDetail.Phone = new PhoneDAL().GetPhoneById(phoneDetail.Phone.PhoneID);
            phoneDetail.PhoneColor = GetPhoneColoreByID(phoneDetail.PhoneColor.ColorID);
            phoneDetail.ROMSize = GetROMSizeByID(phoneDetail.ROMSize.ROMID);
            return phoneDetail;
        }
        public List<Imei> GetImeisByPhoneDetailsID(int phoneDetailID)
        {
            List<Imei> imeis = new List<Imei>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select * from imeis 
                where phone_detail_id = @phone_detail_id and status = 0;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phone_detail_id", phoneDetailID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    imeis.Add(GetImei(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            List<Imei> output = new List<Imei>();
            foreach (var imei in imeis)
            {
                imei.PhoneDetail = GetPhoneDetailByID(imei.PhoneDetail.PhoneDetailID);
                output.Add(imei);
            }
            return output;
        }
        public List<Imei> GetListImeisInOrder(string orderid)
        {
            List<Imei> output = new List<Imei>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select i.* from imeis i 
                inner join orderdetails od on od.phone_imei = i.phone_imei
                where od.order_id = @orderid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@orderid", orderid);
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
            List<Imei> output1 = new List<Imei>();
            foreach (var imei in output)
            {
                imei.PhoneDetail = GetPhoneDetailByID(imei.PhoneDetail.PhoneDetailID);
                output1.Add(imei);
            }

            return output1;
        }
    }
}

