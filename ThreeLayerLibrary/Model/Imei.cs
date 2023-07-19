using BusinessEnum;

namespace Model;
public class Imei
{
    public string PhoneImei { get; set; } = string.Empty;
    public ImeiEnum.Status Status { get; set; }
}