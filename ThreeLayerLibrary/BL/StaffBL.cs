using System.Collections.Generic;
using BusinessEnum;
using Model;
using DAL;

namespace BL;
public class StaffBL
{
    private StaffDAL idal = new StaffDAL();
    public Staff? Authenticate(string username, string password)
    {
        Staff? staff = null;
        
        staff = idal.GetAccountByUsername(username);
        if (staff != null)
        {
            string hashedInputPassword = idal.CreateMD5(password);
            if (hashedInputPassword.Equals(staff.Password))
            {
                if (staff.Role == StaffEnum.Role.Seller || staff.Role == StaffEnum.Role.Accountant)
                    return staff;

                else
                    return null;
            }
            else return null;
        }
        else return null;
    }
}