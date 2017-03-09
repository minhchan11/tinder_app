using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
    public void UpdateUsersName_OneUser_UpdatedName()
    {
      User testUser = new User("John", "I love Epicodus");
      testUser.Save();

      string newName = "Nick";
      testUser.UpdateUsersName(newName);

      Assert.Equal("Nick", newName);
    }

    [Fact]
    public void UpdateUsersDescription_OneUser_UpdatedDescription()
    {
      User testUser = new User("John", "I love Epicodus");
      testUser.Save();

      string newDescription = "I love coding";
      testUser.UpdateUsersDescription(newDescription);

      Assert.Equal("I love coding", newDescription);
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
    public void DeleteUser_OneUser_EmptyList()
    {
        User testUser = new User("Riley", "Hello");
        testUser.Save();
        User testUser2 = new User("John", "I love Epicodus");
        testUser2.Save();
        List<User> expectedUser = new List<User>{testUser2};
        testUser.DeleteUser(testUser.userId);

        Assert.Equal(expectedUser, User.GetAll());
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
    public void DeleteHobby_OneUser_EmptyList()
    {
        User testUser = new User("Riley", "hello");
        testUser.Save();
        testUser.AddHobby("Coding");
        testUser.AddHobby("Singing");
        testUser.DeleteHobby("Singing");
        List<string> output = testUser.GetHobbies();
        List<string> verify = new List<string>{"Coding"};

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
    public void Filter_ManyUsers_ListOfUsersWithMatchingPreferences()
    {
        User nick = new User("nick", "hey");
        User minh = new User("minh", "hi");
        User jiwon = new User("jiwon", "sup");
        User renee = new User("renee", "dude");
        User john = new User("john", "hey");
        nick.Save();
        minh.Save();
        jiwon.Save();
        renee.Save();
        john.Save();
        nick.AddGender("Male");
        minh.AddGender("Male");
        jiwon.AddGender("Female");
        renee.AddGender("Female");
        john.AddGender("Male");
        nick.AddWork("McDonalds");
        minh.AddWork("McDonalds");
        jiwon.AddWork("Burger King");
        renee.AddWork("KFC");
        john.AddWork("Epicodus");
        nick.AddHobby("Gym");
        minh.AddHobby("Fashion");
        jiwon.AddHobby("Hello Kitty");
        renee.AddHobby("Traveling");
        john.AddHobby("Grading");
        Dictionary<string, string> preferences = new Dictionary<string, string>()
        {
            {"rating", "no preference"},
            {"gender", "Male"},
            {"work", "McDonalds"},
            {"food", "no preference"},
            {"hobby", "Gym"}
        };
        List<User> expected = new List<User>{nick};
        List<User> actual = User.Filter(preferences);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Filter_ManyUsers_ListOfUsersWithMatchingPreferencesAndRating()
    {
        User nick = new User("nick", "hey");
        User minh = new User("minh", "hi");
        User jiwon = new User("jiwon", "sup");
        User renee = new User("renee", "dude");
        User john = new User("john", "hey");
        nick.Save();
        minh.Save();
        jiwon.Save();
        renee.Save();
        john.Save();
        User.AddRating(nick.userId, 4);
        User.AddRating(nick.userId, 3);
        User.AddRating(nick.userId, 2);
        User.AddRating(nick.userId, 2);
        User.AddRating(nick.userId, 3);
        User.AddRating(minh.userId, 5);
        User.AddRating(minh.userId, 5);
        User.AddRating(minh.userId, 4);
        User.AddRating(minh.userId, 3);
        User.AddRating(minh.userId, 5);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 2);
        User.AddRating(jiwon.userId, 3);
        User.AddRating(renee.userId, 4);
        User.AddRating(renee.userId, 3);
        User.AddRating(renee.userId, 2);
        User.AddRating(renee.userId, 2);
        User.AddRating(renee.userId, 3);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 2);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 1);
        nick.AddGender("Male");
        minh.AddGender("Male");
        jiwon.AddGender("Female");
        renee.AddGender("Female");
        john.AddGender("Male");
        nick.AddWork("McDonalds");
        minh.AddWork("McDonalds");
        jiwon.AddWork("Burger King");
        renee.AddWork("KFC");
        john.AddWork("Epicodus");
        nick.AddHobby("Gym");
        minh.AddHobby("Fashion");
        jiwon.AddHobby("Hello Kitty");
        renee.AddHobby("Traveling");
        john.AddHobby("Grading");
        Dictionary<string, string> preferences = new Dictionary<string, string>()
        {
            {"rating", "2"},
            {"gender", "no preference"},
            {"work", "McDonalds"},
            {"food", "no preference"},
            {"hobby", "Fashion"}
        };
        List<User> expected = new List<User>{minh};
        List<User> actual = User.Filter(preferences);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Filter_ManyUsers_ListOfUsersWithMatchingPreferencesAndRatingAndLocation()
    {
        User nick = new User("nick", "hey");
        User minh = new User("minh", "hi");
        User jiwon = new User("jiwon", "sup");
        User renee = new User("renee", "dude");
        User john = new User("john", "hey");
        nick.Save();
        minh.Save();
        jiwon.Save();
        renee.Save();
        john.Save();
        Location testLocation1 = new Location ("POINT (-73.993808 40.702999)");
        Location testLocation2 = new Location ("POINT (-7.993808 4.703999)");
        Location testLocation3 = new Location ("POINT (-73.993808 40.702899)");
        Location testLocation4 = new Location ("POINT (-73.993808 4.702999)");
        Location testLocation5 = new Location ("POINT (-73.993808 1.702999)");
        testLocation1.Save();
        testLocation2.Save();
        testLocation3.Save();
        testLocation4.Save();
        testLocation5.Save();
        testLocation1.AddUserToLocation(nick.userId);
        testLocation2.AddUserToLocation(minh.userId);
        testLocation3.AddUserToLocation(jiwon.userId);
        testLocation4.AddUserToLocation(renee.userId);
        testLocation5.AddUserToLocation(john.userId);
        User.AddRating(nick.userId, 4);
        User.AddRating(nick.userId, 3);
        User.AddRating(nick.userId, 2);
        User.AddRating(nick.userId, 2);
        User.AddRating(nick.userId, 3);
        User.AddRating(minh.userId, 5);
        User.AddRating(minh.userId, 5);
        User.AddRating(minh.userId, 4);
        User.AddRating(minh.userId, 3);
        User.AddRating(minh.userId, 5);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 1);
        User.AddRating(jiwon.userId, 2);
        User.AddRating(jiwon.userId, 3);
        User.AddRating(renee.userId, 4);
        User.AddRating(renee.userId, 3);
        User.AddRating(renee.userId, 2);
        User.AddRating(renee.userId, 2);
        User.AddRating(renee.userId, 3);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 2);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 1);
        User.AddRating(john.userId, 1);
        nick.AddGender("Male");
        minh.AddGender("Male");
        jiwon.AddGender("Female");
        renee.AddGender("Female");
        john.AddGender("Male");
        nick.AddWork("McDonalds");
        minh.AddWork("McDonalds");
        jiwon.AddWork("Burger King");
        renee.AddWork("KFC");
        john.AddWork("Epicodus");
        nick.AddHobby("Gym");
        minh.AddHobby("Fashion");
        jiwon.AddHobby("Hello Kitty");
        renee.AddHobby("Traveling");
        john.AddHobby("Grading");
        Dictionary<string, string> preferences = new Dictionary<string, string>()
        {
            {"rating", "2"},
            {"gender", "no preference"},
            {"work", "McDonalds"},
            {"food", "no preference"},
            {"hobby", "Fashion"}
        };
        List<User> expected = new List<User>{};
        List<User> actual = User.Filter(preferences, testLocation1.locationId);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AddRatingGetAverageRating_OneUser_AverageRatingInt()
    {
        User testUser = new User("Nick", "hello");
        testUser.Save();
        User.AddRating(testUser.userId, 2);
        User.AddRating(testUser.userId, 4);
        User.AddRating(testUser.userId, 5);
        User.AddRating(testUser.userId, 1);
        User.AddRating(testUser.userId, 5);
        User.AddRating(testUser.userId, 4);
        Assert.Equal(3.5, testUser.GetAverageRating());
    }

    [Fact]
    public void GetUsersByAscendingRatingOrder_ManyUsers_UserListSortedByRating()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        User.AddRating(user1.userId, 4);
        User.AddRating(user1.userId, 3);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 4);
        User.AddRating(user2.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 2);
        User.AddRating(user3.userId, 3);
        List<User> expected = new List<User>{user3, user1, user2};
        List<User> actual = User.GetUsersByAscendingRatingOrder();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindByMinRating_ManyUsers_UserListWithMinRatingRemoveOneUser()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        User.AddRating(user1.userId, 4);
        User.AddRating(user1.userId, 3);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 4);
        User.AddRating(user2.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 2);
        User.AddRating(user3.userId, 3);
        List<User> expected = new List<User>{user1, user2};
        List<User> actual = User.FindByMinRating(2);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindByMinRating_ManyUsers_UserListWithMinRatingRemoveTwoUsers()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        User.AddRating(user1.userId, 4);
        User.AddRating(user1.userId, 3);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 4);
        User.AddRating(user2.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 2);
        User.AddRating(user3.userId, 3);
        List<User> expected = new List<User>{user2};
        List<User> actual = User.FindByMinRating(4);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FindByMinRating_ManyUsers_UserListWithMinRatingRemoveAllUsers()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        User.AddRating(user1.userId, 4);
        User.AddRating(user1.userId, 3);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 2);
        User.AddRating(user1.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 5);
        User.AddRating(user2.userId, 4);
        User.AddRating(user2.userId, 3);
        User.AddRating(user2.userId, 5);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 1);
        User.AddRating(user3.userId, 2);
        User.AddRating(user3.userId, 3);
        List<User> expected = new List<User>{};
        List<User> actual = User.FindByMinRating(5);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CheckIfLatestUser_ManyUsers_ReturnTrue()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        Assert.Equal(true, user3.CheckIfLatestUser());
    }

    [Fact]
    public void CheckIfLatestUser_ManyUsers_ReturnFalse()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        Assert.Equal(false, user1.CheckIfLatestUser());
    }

    [Fact]
    public void GetAllGenders_ManyUsers_ListOfAllGenders()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        user1.AddGender("male");
        user2.AddGender("female");
        user3.AddGender("transexual");
        user2.AddGender("pansexual");
        List<string> expected = new List<string>{"male", "female", "transexual", "pansexual"};
        List<string> actual = User.GetAllGenders();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAllWorks_ManyUsers_ListOfAllWorks()
    {
        User user1 = new User("1", "hello");
        User user2 = new User("2", "hola");
        User user3 = new User("3", "hi");
        user1.Save();
        user2.Save();
        user3.Save();
        user1.AddWork("McDonalds");
        user2.AddWork("BurgerKing");
        user3.AddWork("Epicodus");
        user2.AddWork("Five Guys");
        List<string> expected = new List<string>{"McDonalds", "BurgerKing", "Epicodus", "Five Guys"};
        List<string> actual = User.GetAllWorks();
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
  }
}
