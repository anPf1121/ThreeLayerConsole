using BusinessEnum;

namespace Model;
public class Phone
{
    public int PhoneID { get; set; } = 0;
    public string PhoneName { get; set; } = string.Empty;
    public Brand Brand { get; set; } = default!;
    public string Camera { get; set; } = string.Empty;
    public string RAM { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Processor { get; set; } = string.Empty;
    public string BatteryCapacity { get; set; } = string.Empty;
    public string SimSlot { get; set; } = string.Empty;
    public string OS { get; set; } = string.Empty;
    public string Screen { get; set; } = string.Empty;
    public string Connection { get; set; } = string.Empty;
    public ROMSize ROM { get; set; } = default!;
    public PhoneColor Color { get; set; } = default!;
    public List<Imei> ListImeis { get; set; } = default!;
    public int Quantity { get; set; } 
    public PhoneEnum.Status PhoneStatusType { get; set; }
    public DateTime ReleaseDate { get; set; } 
    public string ChargePort { get; set; } = string.Empty;
    public Staff CreateBy { get; set; } = default!;
    public DateTime CreateAt { get; set; } 
    public DateTime UpdateAt { get; set; }
    public Staff UpdateBy { get; set; } = default!; 
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}