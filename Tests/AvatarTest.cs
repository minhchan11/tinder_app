using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;


namespace TinderApp
{
  public class AvatarTest: IDisposable
  {
    public AvatarTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=tinder_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Avatar.DeleteAll();
    }

    [Fact]
    public void GetAll_DatabaseEmptyAtFirst_ZeroOutput()
    {
      //Arrange, Act
      int result = Avatar.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OverrideBool_SameAvatar_ReturnsEqual()
    {
      //Arrange, Act
      Avatar avatarOne = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat.jpg");
      Avatar avatarTwo = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat.jpg");

      //Assert
      Assert.Equal(avatarTwo, avatarOne);
    }

  }
}
