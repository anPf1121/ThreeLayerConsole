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
            DiscountPolicy discountPolicy = new DiscountPolicy(
                reader.GetInt32("policy_id"), // dc policy, staff, Phonedetails
                reader.GetString("title"),
                reader.GetDateTime("from_date"),
                reader.GetDateTime("to_date"),
                staffDAL.GetStaff(reader),
                reader.GetDateTime("create_at"),
                reader.GetDecimal("minimum_purchase_amount"),
                reader.GetDecimal("maximum_purchase_amount"),
                reader.GetDecimal("money_supported"),
                reader.GetString("payment_method"),
                phoneDetailsDAL.GetPhoneDetail(reader),
                reader.GetDateTime("update_at"),
                staffDAL.GetStaff(reader),
                reader.GetDecimal("discount_price"),
                reader.GetString("description"),
                orderDAL.GetOrder(reader)
            );
            return discountPolicy;
        }
        public DiscountPolicy? GetDiscountPolicyById(int discountPolicyID)
        {
            DiscountPolicy? discountPolicy = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                query = @"SELECT * FROM discountpolicies DP
                        INNER JOIN staffs S ON DP.create_by = S.staff_id
                        INNER JOIN phonedetails PD ON DP.phone_detail_id = PD.phone_detail_id
                        INNER JOIN phones P ON P.phone_id = PD.phone_id
                        INNER JOIN brands B ON B.brand_id = P.brand_id
                        INNER JOIN romsizes RS ON PD.rom_size_id = RS.rom_size_id
                        INNER JOIN colors C ON PD.color_id = C.color_id
                        INNER JOIN orders O ON S.staff_id = O.seller_id
                        WHERE DP.policy_id = @discountpolicyid;";
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

            // if (phone != null)
            // {
            //     PhoneDetailsDAL phoneDetailsDAL = new PhoneDetailsDAL();
            //     phone.PhoneDetails = phoneDetailsDAL.GetPhoneDetailsByPhoneID(phone.PhoneID);
            // }

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
    }
}

