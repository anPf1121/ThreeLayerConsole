using MySqlConnector;
using Model;
using BusinessEnum;

namespace DAL
{
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
        public ROMSize GetROMSize(MySqlDataReader reader)
        {
            ROMSize romsize = new ROMSize(
                    reader.GetInt32("rom_size_id"),
                    reader.GetString("rom_size")
                );
            return romsize;
        }
        public PhoneColor GetPhoneColor(MySqlDataReader reader) {
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
                phoneDAL.GetPhone(reader),
                GetROMSize(reader),
                GetPhoneColor(reader),
                (PhoneEnum.Status)Enum.ToObject(typeof(PhoneEnum.Status), reader.GetInt32("phone_status_type")),
                new List<Imei>(),
                staffDAL.GetStaff(reader),
                reader.GetDateTime("update_at")
            );
            return phoneDetail;
        }
        public List<PhoneDetail> GetPhoneDetailsByPhoneID(int phoneID)
        {
            List<PhoneDetail> phoneDetails = new List<PhoneDetail>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"SELECT * FROM phones P
                            INNER JOIN phonedetails PD ON P.phone_id = PD.phone_id
                            INNER JOIN brands B ON P.brand_id = B.brand_id
                            INNER JOIN romsizes RS ON PD.rom_size_id = RS.rom_size_id
                            INNER JOIN colors C ON PD.color_id = C.color_id
                            INNER JOIN staffs S ON P.create_by = S.staff_id
                            WHERE PD.phone_id = @phoneID";
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

            // foreach (PhoneDetail item in phoneDetails)
            // {
            //     try
            //     {
            //         if (connection.State == System.Data.ConnectionState.Closed)
            //         {
            //             connection.Open();
            //         }
            //         query = @"SELECT phone_imei, status FROM imeis WHERE phone_detail_id=@phoneDetailID";
            //         MySqlCommand command = new MySqlCommand(query, connection);
            //         command.Parameters.Clear();
            //         command.Parameters.AddWithValue("@phoneDetailID", item.PhoneDetailID);
            //         MySqlDataReader reader = command.ExecuteReader();
            //         while (reader.Read())
            //         {
            //             item.ListImei.Add(GetImei(reader));
            //         }
            //         reader.Close();
            //     }
            //     catch (MySqlException ex)
            //     {
            //         Console.WriteLine(ex.Message);
            //     }
            // }
            // if (connection.State == System.Data.ConnectionState.Open)
            // {
            //     connection.Close();
            // }

            return phoneDetails;
        }
    }
}


