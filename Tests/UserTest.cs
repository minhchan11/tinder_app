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

    [Fact]
    public void DeleteGender_OneUser_EmptyList()
    {
        User testUser = new User("Riley", "hello");
        testUser.Save();
        testUser.AddGender("Male");
        testUser.AddGender("Transgender");
        testUser.DeleteGender("Male");
        List<string> output = testUser.GetGenders();
        List<string> verify = new List<string>{"Transgender"};

        Assert.Equal(verify, output);
    }

    [Fact]
    public void DeleteWork_OneUser_EmptyList()
    {
        User testUser = new User("Riley", "hello");
        testUser.Save();
        testUser.AddWork("Microsoft");
        testUser.AddWork("Epicodus");
        testUser.DeleteWork("Microsoft");
        List<string> output = testUser.GetWorks();
        List<string> verify = new List<string>{"Epicodus"};

        Assert.Equal(verify, output);
    }

    [Fact]
    public void DeleteFood_OneUser_EmptyList()
    {
        User testUser = new User("Riley", "hello");
        testUser.Save();
        testUser.AddFood("Sushi");
        testUser.AddFood("Burger");
        testUser.DeleteFood("Sushi");
        List<string> output = testUser.GetFoods();
        List<string> verify = new List<string>{"Burger"};

        Assert.Equal(verify, output);
    }

    [Fact]
    public void GetLikedUsers_OneUser_ListOfLikedUsers()
    {
        User testUser = new User("Riley", "hello");
        testUser.Save();
        User likedUser = new User("Carl", "sup");
        likedUser.Save();
        User likedUser2 = new User("Fred", "dude");
        likedUser2.Save();
        testUser.AddLike(likedUser.userId);
        testUser.AddLike(likedUser2.userId);
        List<int> output = testUser.GetLikedUsers();
        List<int> expected = new List<int>{likedUser.userId, likedUser2.userId};
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Find_OneUser_FoundUserObject()
    {
        User testUser = new User("Nick", "Konnichiwa");
        testUser.Save();
        User expected = User.Find(testUser.userId);
        Assert.Equal(expected, testUser);
    }

    [Fact]
    public void AddHobbyGetHobbies_OneUser_FoundHobbyStrings()
    {
        User testUser = new User("Nick", "hola");
        testUser.Save();
        testUser.AddHobby("Gym");
        testUser.AddHobby("Eating");
        List<string> output = testUser.GetHobbies();
        List<string> expected = new List<string>{"Gym", "Eating"};
        Assert.Equal(expected, output);
    }

    [Fact]
    public void FindByGender_TwoUsers_ListOfUsersByGender()
    {
        User testUser = new User("Nick", "hello");
        User testUser2 = new User("Minh", "hi");
        User testUser3 = new User("Jiwon", "hey");
        testUser.Save();
        testUser2.Save();
        testUser3.Save();
        testUser.AddGender("Male");
        testUser.AddGender("Female");
        testUser2.AddGender("Male");
        testUser3.AddGender("Female");
        List<User> userList = new List<User>{testUser, testUser2};
        List<User> actual = User.FindByGender("Female", userList);
        List<User> expected = new List<User>{testUser};
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindByWork_TwoUsers_ListOfUsersByWork()
    {
        User testUser = new User("Nick", "hello");
        User testUser2 = new User("Minh", "hi");
        User testUser3 = new User("Jiwon", "hey");
        testUser.Save();
        testUser2.Save();
        testUser3.Save();
        testUser.AddWork("Mcdonalds");
        testUser.AddWork("Burger King");
        testUser2.AddWork("KFC");
        testUser3.AddWork("Mcdonalds");
        List<User> userList = new List<User>{testUser2, testUser3};
        List<User> actual = User.FindByWork("Mcdonalds", userList);
        List<User> expected = new List<User>{testUser3};
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindByFood_TwoUsers_ListOfUsersByFood()
    {
        User testUser = new User("Nick", "hello");
        User testUser2 = new User("Minh", "hi");
        User testUser3 = new User("Jiwon", "hey");
        testUser.Save();
        testUser2.Save();
        testUser3.Save();
        testUser.AddFood("Sushi");
        testUser.AddFood("Burgers");
        testUser2.AddFood("Burgers");
        testUser3.AddFood("Burritos");
        List<User> userList = new List<User>{testUser, testUser3};
        List<User> actual = User.FindByFood("Burgers", userList);
        List<User> expected = new List<User>{testUser};
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindByHobby_TwoUsers_ListOfUsersByHobby()
    {
        User testUser = new User("Nick", "hello");
        User testUser2 = new User("Minh", "hi");
        User testUser3 = new User("Jiwon", "hey");
        testUser.Save();
        testUser2.Save();
        testUser3.Save();
        testUser.AddHobby("Gym");
        testUser.AddHobby("Eating");
        testUser2.AddHobby("Gym");
        testUser3.AddHobby("Gym");
        List<User> userList = new List<User>{testUser, testUser2};
        List<User> actual = User.FindByHobby("Gym", userList);
        List<User> expected = new List<User>{testUser, testUser2};
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Contains_True()
    {
        User testUser = new User("Nick", "hello");
        User testUser2 = new User("Jiwon", "hi");
        List<User> testList = new List<User>{testUser, testUser2};
        Assert.Equal(true, testList.Contains(testUser));
    }


    // [Fact]
    // public void AddLike_OneUser_ListOfLikes()

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
