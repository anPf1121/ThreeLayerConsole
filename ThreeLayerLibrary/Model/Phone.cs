using BusinessEnum;
using DAL;

namespace Model;
public class Phone
{
    public int PhoneID { get; set; }
    public string PhoneName { get; set; }
    public Brand Brand { get; set; }
    public string Camera { get; set; }
    public string RAM { get; set; }
    public string Weight { get; set; }
    public string Processor { get; set; }
    public string BatteryCapacity { get; set; }
    public string SimSlot { get; set; }
    public string OS { get; set; }
    public string Screen { get; set; }
    public string Connection { get; set; }
    public List<PhoneDetail>? PhoneDetails { get; set; } // tạm thời để null để test
    public DateTime ReleaseDate { get; set; } 
    public string ChargePort { get; set; }
    public Staff CreateBy { get; set; }
    public DateTime CreateAt { get; set; } 
    public string Description { get; set; }
    public Phone(int phoneID, string phoneName, Brand brand, string camera, string ram, string weight, string processor, string batteryCapacity, string simSlot, string os, string screen, string connection, List<PhoneDetail> phoneDetails, DateTime releaseDate, string chargePort, Staff createBy, DateTime createAt, string description){
        this.PhoneID = phoneID;
        this.PhoneName = phoneName;
        this.Brand = brand;
        this.Camera = camera;
        this.RAM = ram;
        this.Weight = weight;
        this.Processor = processor;
        this.BatteryCapacity = batteryCapacity;
        this.SimSlot = simSlot;
        this.OS = os;
        this.Screen = screen;
        this.Connection = connection;
        this.PhoneDetails = phoneDetails;
        this.ReleaseDate = releaseDate;
        this.ChargePort = chargePort;
        this.CreateBy = createBy;
        this.CreateAt = createAt;
        this.Description = description;
    }
}