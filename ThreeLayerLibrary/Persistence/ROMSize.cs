using BusinessEnum;

namespace Model;
public class ROMSize
{
    public int ROMID { get; set; }
    public string ROM { get; set; }
    public ROMSize(int romID, string rom){
        this.ROMID = romID;
        this.ROM = rom;
    }
}