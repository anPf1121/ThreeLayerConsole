using BusinessEnum;

namespace Model;
public class Staff
{
    public int StaffID { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public StaffEnum.Status Status { get; set; }
    public StaffEnum.Role Role { get; set; }
}