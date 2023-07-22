using System.Drawing;
using BusinessEnum;
namespace Model;

public class PhoneDetail
{
    public int PhoneDetailID { get; set; }
    public Phone Phone { get; set; }
    public ROMSize ROMSize { get; set; }
    public PhoneColor PhoneColor { get; set; }
    public PhoneEnum.Status PhoneStatusType { get; set; }
    public List<Imei> ListImei { get; set; }
    public Staff UpdateBy { get; set; }
    public DateTime UpdateAt { get; set; }
    public PhoneDetail(int phoneDetailID, Phone phone, ROMSize romSize, PhoneColor phoneColor, PhoneEnum.Status phoneStatusType, List<Imei> listImei, Staff updateBy, DateTime updateAt)
    {
        this.PhoneDetailID = phoneDetailID;
        this.Phone = phone;
        this.ROMSize = romSize;
        this.PhoneColor = phoneColor;
        this.PhoneStatusType = phoneStatusType;
        this.ListImei = listImei;
        this.UpdateBy = updateBy;
        this.UpdateAt = updateAt;
    }

}