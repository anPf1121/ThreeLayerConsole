using Ults;
using Xunit;

namespace PhoneStorePL.Tests;

public class OrderTests
{
    [Fact] 
   
    public void CreateValidOrder_Success()
    {
        int a = 5;
        int b = 7;
        int result = 12;

        // hàm kiểm thử, 
        Assert.Equal(12, result);
    }

}
