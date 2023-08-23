using System.Collections.Generic;
using Interface;
using BusinessEnum;
using Model;
using DAL;

namespace BL;

public class StaffBL : IStaffBL
{
    public Staff LoggedInStaff { get; private set; }
    private Staff? loggedInStaff;
    private StaffDAL idal = new StaffDAL();
    public bool Login(string username, string password)
    {
        Staff? staff = null;
        staff = idal.GetAccountByUsername(username);
        if (staff != null)
        {
            string hashedInputPassword = idal.CreateMD5(password);
            if (hashedInputPassword.Equals(staff.Password))
            {
                LoggedInStaff = staff;
                loggedInStaff = staff;
                return true;
            }
        }
        return false;
    }
    public void Logout()
    {
        LoggedInStaff = null;
        loggedInStaff = null;   
    }
}