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
            reader.GetString("description")
            );
            return discountPolicy;
        }
        public DiscountPolicy GetDiscountPolicyById(int discountPolicyID)
        {
            StaffDAL staffDAL = new StaffDAL();
            PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();

            DiscountPolicy discountPolicy = new DiscountPolicy(0, "", new DateTime(), new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new DateTime(), 0, 0, 0, "", new PhoneDetail(0, new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "",  new DateTime(), "",new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new List<Imei>(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new DateTime()), new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), 0, "");
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

