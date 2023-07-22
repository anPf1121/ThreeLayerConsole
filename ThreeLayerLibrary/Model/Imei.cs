using BusinessEnum;

namespace Model;
public class Imei
{
    public string PhoneImei { get; set; }
    public PhoneEnum.ImeiStatus Status { get; set; }
    public Imei(string phoneImei, PhoneEnum.ImeiStatus status){
        this.PhoneImei = phoneImei;
        this.Status = status;
    }
}