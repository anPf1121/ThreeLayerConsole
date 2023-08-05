using Model;
namespace Interface; 

public interface IStaffBL
{
    Staff LoggedInStaff { get; }
    bool Login(string username, string password);
    void Logout();
}
