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
      User.DeleteAll();
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

    [Fact]
    public void Save_NewAvatar_SaveToDatabase()
    {
      //Arrange, Act
      Avatar testAvatar = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat.jpg");
      testAvatar.Save();
      //Assert
      Avatar result = Avatar.GetAll()[0];
      Assert.Equal(testAvatar, result);
    }

    [Fact]
    public void Save_NewAvatars_SaveToDatabase()
    {
      //Arrange, Act
      Avatar avatarOne = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat.jpg");
      Avatar avatarTwo = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat5.jpg");
      avatarOne.Save();
      avatarTwo.Save();
      //Assert
      List<Avatar> result = Avatar.GetAll();
      List<Avatar> expected = new List<Avatar> {avatarOne, avatarTwo};
      Assert.Equal(expected, result);
    }

    [Fact]
    public void Save_OneAvatar_AvatarSavedWithCorrectID()
    {
      //Arrange
      Avatar testAvatar = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat.jpg");
      testAvatar.Save();
      Avatar savedAvatar = Avatar.GetAll()[0];

      //Act
      int output = savedAvatar.avatarId;
      int verify = testAvatar.avatarId;

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Find_AvatarId_AvatarWithIdFound()
    {
      // Arrange
      Avatar testAvatar = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat.jpg");
      testAvatar.Save();

      //Act
      Avatar foundAvatar = Avatar.Find(testAvatar.avatarId);

      //Assert
      Assert.Equal(testAvatar, foundAvatar);
    }

    [Fact]
    public void Avatar_Update_UpdateDatabaseAndLocalObject()
    {
      Avatar testAvatar = new Avatar ("C:\\Users\\epicodus\\Desktop\\cat.jpg");
      testAvatar.Save();

      testAvatar.Update("C:\\Users\\epicodus\\Desktop\\cat5.jpg");

      Assert.Equal(testAvatar, Avatar.GetAll()[0]);
    }

  }
}
