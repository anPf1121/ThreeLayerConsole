using System.Drawing;
using BusinessEnum;
namespace Model;

public class PhoneDetail
{
    public int PhoneDetailID { get; set; }
    public Phone Phone { get; set; }
    public ROMSize ROMSize { get; set; }
    public PhoneColor PhoneColor { get; set; }
    public decimal Price {get;set;}
    public int Quantity {get;set;}
    public PhoneEnum.Status PhoneStatusType { get; set; }
    public Staff UpdateBy { get; set; }
    public DateTime UpdateAt { get; set; }
    public PhoneDetail(int phoneDetailID, Phone phone, ROMSize romSize, PhoneColor phoneColor,decimal price,int quantity, PhoneEnum.Status phoneStatusType, Staff updateBy, DateTime updateAt)
    {
        this.PhoneDetailID = phoneDetailID;
        this.Phone = phone;
        this.ROMSize = romSize;
        this.PhoneColor = phoneColor;
        this.Price = price;
        this.Quantity = quantity;
        this.PhoneStatusType = phoneStatusType;
        this.UpdateBy = updateBy;
        this.UpdateAt = updateAt;
    }
}