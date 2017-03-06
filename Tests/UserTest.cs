using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;


namespace TinderApp
{
  public class UserTest: IDisposable
  {
    public UserTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=tinder_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      User.DeleteAll();
    }

    [Fact]
    public void GetAll_DatabaseEmptyAtFirst_ZeroOutput()
    {
      //Arrange, Act
      int result = User.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OverrideBool_SameUser_ReturnsEqual()
    {
      //Arrange, Act
      User userOne = new User ("Nick", "hello");
      User userTwo = new User ("Nick", "hello");

      //Assert
      Assert.Equal(userTwo, userOne);
    }

    [Fact]
    public void Save_OneUser_UserSavedToDatabase()
    {
      //Arrange
      User testUser = new User ("Nick", "I like stuff");

      //Act
      testUser.Save();
      List<User> output = User.GetAll();
      List<User> verify = new List<User>{testUser};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Save_OneUser_UserSavedWithCorrectID()
    {
      //Arrange
      User testUser = new User ("Nick", "blah");
      testUser.Save();
      User savedUser = User.GetAll()[0];

      //Act
      int output = savedUser.userId;
      int verify = testUser.userId;

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void SaveGetAll_ManyUsers_ReturnListOfUsers()
    {
      //Arrange
      User userOne = new User ("nick", "hello");
      userOne.Save();
      User userTwo = new User ("Jiwon", "hi");
      userTwo.Save();

      //Act
      List<User> output = User.GetAll();
      List<User> verify = new List<User>{userOne, userTwo};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void AddGenderGetGenders_OneUser_ListOfGenders()
    {
        User testUser = new User("Nick", "hello");
        testUser.Save();
        testUser.AddGender("Male");
        testUser.AddGender("Transgender");
        List<string> output = testUser.GetGenders();
        List<string> verify = new List<string>{"Male", "Transgender"};

        Assert.Equal(verify, output);
    }

    [Fact]
    public void AddWorkGetWorks_OneUser_ListOfWorks()
    {
        User testUser = new User("Nick", "hello");
        testUser.Save();
        testUser.AddWork("McDonalds");
        testUser.AddWork("Burger King");
        List<string> output = testUser.GetWorks();
        List<string> verify = new List<string>{"McDonalds", "Burger King"};

        Assert.Equal(verify, output);
    }

    [Fact]
    public void AddFoodGetFoods_OneUser_ListOfFoods()
    {
        User testUser = new User("Nick", "hello");
        testUser.Save();
        testUser.AddFood("McDonalds");
        testUser.AddFood("Burger King");
        List<string> output = testUser.GetFoods();
        List<string> verify = new List<string>{"McDonalds", "Burger King"};

        Assert.Equal(verify, output);
    }

    // [Fact]
    // public void Find_OneBandId_ReturnBandFromDatabase()
    // {
    //   //Arrange
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //
    //   //Act
    //   Band foundBand = Band.Find(testBand.GetId());
    //
    //   //Assert
    //   Assert.Equal(testBand, foundBand);
    // }
    //
    // [Fact]
    // public void SearchName_Name_ReturnBandFromDatabase()
    // {
    //   //Arrange
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //
    //   //Act
    //   List<Band> output = Band.SearchName("Green Day");
    //   List<Band> verify = new List<Band>{testBand};
    //
    //   //Assert
    //   Assert.Equal(verify, output);
    // }
    //
    // [Fact]
    // public void AddVenue_OneBand_VenueAddedToJoinTable()
    // {
    //   //Arrange
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //   Venue testVenue = new Venue("Park");
    //   testVenue.Save();
    //   testBand.AddVenue(testVenue);
    //
    //   //Act
    //   List<Venue> output = testBand.GetVenues();
    //   List<Venue> verify = new List<Venue>{testVenue};
    //
    //   //Assert
    //   Assert.Equal(verify, output);
    // }
    //
    // [Fact]
    // public void DeleteVenue_TwoVenueRemoveOne_VenueRemovedFromJoinTable()
    // {
    //   //Arrange
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //   Venue testVenue1 = new Venue("Park");
    //   Venue testVenue2 = new Venue("Manhattan Square");
    //   testVenue1.Save();
    //   testVenue2.Save();
    //   testBand.AddVenue(testVenue1);
    //   testBand.AddVenue(testVenue2);
    //   testBand.DeleteVenue(testVenue1);
    //
    //   //Act
    //   List<Venue> output = testBand.GetVenues();
    //   List<Venue> verify = new List<Venue>{testVenue2};
    //
    //   //Assert
    //   Assert.Equal(verify, output);
    // }
    //
    // [Fact]
    // public void DeleteVenues_OneBand_VenueRemovedFromJoinTable()
    // {
    //   //Arrange
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //   Venue testVenue1 = new Venue("Park");
    //   Venue testVenue2 = new Venue("Manhattan Square");
    //   testVenue1.Save();
    //   testVenue2.Save();
    //   testBand.DeleteVenues();
    //
    //   //Act
    //   int output = testBand.GetVenues().Count;
    //   //Assert
    //   Assert.Equal(0, output);
    // }
    //
    //
    // [Fact]
    // public void Band_Delete_RemoveObjectFromDatabase()
    // {
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //
    //   testBand.DeleteThis();
    //
    //   Assert.Equal(0, Band.GetAll().Count);
    // }
    //
    // [Fact]
    // public void Band_Update_UpdateDatabaseAndLocalObject()
    // {
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //
    //   testBand.Update("Yellow Day");
    //   Band expectedBand = new Band("Yellow Day", testBand.GetId());
    //
    //   Assert.Equal(expectedBand, Band.Find(testBand.GetId()));
    // }
    //
    // [Fact]
    // public void Band_Save_NoSaveOnDuplicateBand()
    // {
    //   Band testBand = new Band ("Green Day");
    //   testBand.Save();
    //   Band secondBand = new Band ("Green Day");
    //   secondBand.Save();
    //
    //   Assert.Equal(1, Band.GetAll().Count);
    // }

  }
}
