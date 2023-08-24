using Ults;
using BL;
using Xunit;
using Model;

namespace PhoneStorePL.Tests;

public class LoginTests
{
    [Fact]
    public void Login_Success1()
    {
        string userName = "seller01";
        string password = "abc123";
        Assert.True(new StaffBL().Login(userName, password));
    }
}
