using BusinessEnum;

namespace Model;
public class Staff
{
    public int StaffID { get; set; }
    public string StaffName { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public StaffEnum.Status Status { get; set; }
    public StaffEnum.Role Role { get; set; }
    public Staff(int staffID, string staffName, string phoneNumber, string userName, string password, string address, StaffEnum.Role role, StaffEnum.Status status){
        this.StaffID = staffID;
        this.StaffName = staffName;
        this.PhoneNumber = phoneNumber;
        this.UserName = userName;
        this.Password = password;
        this.Address = address;
        this.Role = role;
        this.Status = status;
    }
}