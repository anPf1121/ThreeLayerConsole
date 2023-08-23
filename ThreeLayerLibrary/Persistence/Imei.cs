using BusinessEnum;

namespace Model;
public class Imei
{
    public PhoneDetail PhoneDetail { get; set; }
    public string PhoneImei { get; set; }
    public PhoneEnum.ImeiStatus Status { get; set; }
    public Imei(PhoneDetail phoneDetail, string phoneImei, PhoneEnum.ImeiStatus status){
        this.PhoneDetail = phoneDetail;
        this.PhoneImei = phoneImei;
        this.Status = status;
    }
}