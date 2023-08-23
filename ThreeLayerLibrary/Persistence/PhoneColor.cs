using BusinessEnum;

namespace Model;
public class PhoneColor
{
    public int ColorID { get; set; }
    public string Color { get; set; } 
    public PhoneColor(int colorID, string color){
        this.ColorID = colorID;
        this.Color = color;
    }
}