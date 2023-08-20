using MySqlConnector;
using Model;
using BusinessEnum;

namespace DAL
{
    public class PhoneDetailFilter {
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
        public bool UpdateImeiStatus(string imei, int imeiFilter) {
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
            } finally {
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
            output.PhoneColor = GetPhoneColoreByID(output.PhoneColor.ColorID);
            output.UpdateBy = staffDAL.GetStaffByID(output.UpdateBy.StaffID);
            output.ListImei = GetImeisByPhoneDetailsID(phonedetailid);
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
            return imeis;
        }
        public List<Imei> GetListImeisInOrder(int phonedetailid, string orderid)
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
                where od.order_id = @orderid and i.phone_detail_id = @phonedetailid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@orderid", orderid);
                command.Parameters.AddWithValue("@phonedetailid", phonedetailid);
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
        public List<PhoneDetail> GetListPhoneDetailInOrder(string orderid)
        { // Ham nay lay ra list<phonedetail> co trong order chi tiet den ca quantity 
            List<PhoneDetail> lst = new List<PhoneDetail>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select pd.* from orders o 
                inner join orderdetails os on o.order_id = os.order_id
                inner join imeis i on i.phone_imei = os.phone_imei
                inner join phonedetails pd on pd.phone_detail_id = i.phone_detail_id
                where o.order_id = @orderid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@orderid", orderid);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(GetPhoneDetail(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            List<PhoneDetail> lst2 = new List<PhoneDetail>();
            foreach (var i in lst)
            {
                lst2.Add(GetPhoneDetailByID(i.PhoneDetailID));
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            List<PhoneDetail> output = new List<PhoneDetail>();
            foreach (var p in lst2)
            {
                int count = 0;
                foreach (var o in output)
                {
                    if (o.PhoneDetailID == p.PhoneDetailID) count++;
                }
                if (count == 0) output.Add(p);
            }
            List<PhoneDetail> output1 = new List<PhoneDetail>();
            foreach (var o in output)
            {
                int count = 0;
                foreach (var i in lst2)
                {
                    if (o.PhoneDetailID == i.PhoneDetailID) count++;
                }
                o.Quantity = count;
                o.ListImei = GetListImeisInOrder(o.PhoneDetailID, orderid);
                output1.Add(o);
            }
            return output1;
        }
        public Dictionary<PhoneDetail, decimal> GetListPhoneDetailHaveDiscountByID(int phonedetailid)
        {
            Dictionary<PhoneDetail, decimal> dic = new Dictionary<PhoneDetail, decimal>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"select pd.*, dp.discount_price from discountpolicies dp
                        inner join phonedetails pd on pd.phone_detail_id = dp.phone_detail_id
                        where dp.discount_price != 0 and dp.policy_id = @policyid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@policyid", phonedetailid);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dic.Add(GetPhoneDetail(reader), reader.GetDecimal("discount_price"));
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
            Dictionary<PhoneDetail, decimal> output = new Dictionary<PhoneDetail, decimal>();
            foreach (var d in dic)
            {
                output.Add(GetPhoneDetailByID(d.Key.PhoneDetailID), d.Value);
            }
            return output;
        }
        public decimal GetPhoneDiscountPrice(int phonedetailid){
            decimal output = 0;
            try{
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                connection.Open();
                }
                query = @"select dp.discount_price from phonedetails pd
                inner join discountpolicies dp on pd.phone_detail_id = dp.phone_detail_id
                where pd.phone_detail_id = @phonedetailid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@phonedetailid", phonedetailid);
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read()){
                    output = reader.GetDecimal("discount_price");
                }
            }catch(MySqlException ex){
                Console.WriteLine(ex.Message);
            }
            if (connection.State == System.Data.ConnectionState.Open)
                {
                connection.Close();
                }
                return output;
        }
        public List<PhoneDetail> GetListPhoneDetailCanTradeIn(){
        List<PhoneDetail> lst = new List<PhoneDetail>();
        try{
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            query = @"select * from phonedetails where phone_status_type != 0;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Clear();
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read()){
                lst.Add(GetPhoneDetail(reader));
            }
            reader.Close();
        }catch(MySqlException ex){
            Console.WriteLine(ex.Message);
        }
        if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        List<PhoneDetail> output = new List<PhoneDetail>();
        foreach(var p in lst){
            output.Add(GetPhoneDetailByID(p.PhoneDetailID));
        }
            return output;
    }
    }
}

