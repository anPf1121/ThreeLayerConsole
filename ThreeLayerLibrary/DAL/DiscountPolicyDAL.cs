using MySqlConnector;
using Model;
using BusinessEnum;
namespace DAL
{
    public class DiscountPolicyDAL
    {
        private string query = "";
        public MySqlConnection connection = DbConfig.GetConnection();

        public DiscountPolicy GetDiscountPolicy(MySqlDataReader reader)
        {
            StaffDAL staffDAL = new StaffDAL();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            OrderDAL orderDAL = new OrderDAL();
            DiscountPolicy discountPolicy = new DiscountPolicy(reader.GetInt32("policy_id"),
            reader.GetString("title"),
            reader.GetDateTime("from_date"),
            reader.GetDateTime("to_date"),
            new Staff(reader.GetInt32("create_by"), "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active),
            reader.GetDateTime("create_at"),
            reader.GetDecimal("minimum_purchase_amount"),
            reader.GetDecimal("maximum_purchase_amount"),
            reader.GetDecimal("money_supported"),
            reader.GetString("payment_method"),
            new PhoneDetail(reader.GetInt32("phone_detail_id"), new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "",  new DateTime(), "",new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new List<Imei>(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new DateTime()),
            reader.GetDateTime("update_at"),
            new Staff(reader.GetInt32("update_by"), "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active),
            reader.GetDecimal("discount_price"),
            reader.GetString("description"),
            new Order(0, new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(),"", 0)
            );
            return discountPolicy;
        }
        public DiscountPolicy GetDiscountPolicyById(int discountPolicyID)
        {
            StaffDAL staffDAL = new StaffDAL();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();

            DiscountPolicy discountPolicy = new DiscountPolicy(0, "", new DateTime(), new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new DateTime(), 0, 0, 0, "", new PhoneDetail(0, new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "",  new DateTime(), "",new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new List<Imei>(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new DateTime()), new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), 0, "", new Order(0, new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active),new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0));
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"SELECT * FROM discountpolicies 
                        WHERE policy_id = @discountpolicyid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@discountpolicyid", discountPolicyID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) discountPolicy = GetDiscountPolicy(reader);
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
            discountPolicy.CreateBy = staffDAL.GetStaffByID(discountPolicy.CreateBy.StaffID);
            discountPolicy.PhoneDetail = phoneDetailsDAL.GetPhoneDetailByID(discountPolicy.PhoneDetail.PhoneDetailID);
            discountPolicy.UpdateBy = staffDAL.GetStaffByID(discountPolicy.UpdateBy.StaffID);
            return discountPolicy;
        }
        public List<DiscountPolicy>? GetDiscountPolicies()
        {
            List<DiscountPolicy> lst = new List<DiscountPolicy>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"SELECT * FROM discountPolicies DP
                        INNER JOIN staffs S ON DP.create_by = S.staff_id
                        INNER JOIN phonedetails PD ON DP.phone_detail_id = PD.phone_detail_id
                        INNER JOIN phones P ON P.phone_id = PD.phone_id
                        INNER JOIN brands B ON B.brand_id = P.brand_id
                        INNER JOIN romsizes RS ON PD.rom_size_id = RS.rom_size_id
                        INNER JOIN colors C ON PD.color_id = C.color_id
                        INNER JOIN orders O ON S.staff_id = O.seller_id
                        WHERE DP.to_date >= date(current_time());";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(GetDiscountPolicy(reader));
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

            // //Xu li cac phone trung lap:
            // List<Phone> output = new List<Phone>();
            // foreach (var p in lst)
            // {
            //     int count = 0;
            //     foreach (var o in output)
            //     {
            //         if (o.PhoneName == p.PhoneName && o.PhoneID == p.PhoneID) count++;
            //     }
            //     if (count == 0) output.Add(GetPhoneById(p.PhoneID));
            // }

            // if (output.Count() == 0) return null;
            return lst;
        }
        public List<DiscountPolicy> GetDiscountValidated(){
            List<DiscountPolicy> lst = new List<DiscountPolicy>();
            try{
                if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            query = @"select * from discountpolicies where date(from_date) <= current_timestamp() and date(to_date) >=current_timestamp();";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read()){
                lst.Add(GetDiscountPolicy(reader));
            }
            reader.Close();
            }catch(MySqlException ex){
                Console.WriteLine(ex.Message);
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            List<DiscountPolicy> output = new List<DiscountPolicy>();
            foreach(var l in lst){
                output.Add(GetDiscountPolicyById(l.PolicyID));
            }
            return output;
        }
    }
}

