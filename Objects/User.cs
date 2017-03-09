using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace TinderApp
{
  public class User
  {
    private int _id;
    private string _name;
    private string _description;

    public User(string name = null, string description = null, int userId = 0){
      _id = userId;
      _name = name;
      _description = description;
    }

    public string name {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    public string description {
      get
      {
        return this._description;
      }
      set
      {
        this._description = value;
      }
    }

    public int userId
    {
      get
      {
        return this._id;
      }
      set
      {
        this._id = value;
      }
    }

    public static void DeleteAll()
    {
      DB.DeleteAll("users");
      DB.DeleteAll("genders");
      DB.DeleteAll("works");
      DB.DeleteAll("foods");
      DB.DeleteAll("users_genders");
      DB.DeleteAll("users_works");
      DB.DeleteAll("users_foods");
      DB.DeleteAll("likes");
      DB.DeleteAll("hobbies");
      DB.DeleteAll("users_hobbies");
      DB.DeleteAll("ratings");
    }

    public static List<User> GetAll()
    {
      List<User> allUsers = new List<User>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM users;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        User newUser = new User(rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(0));
        allUsers.Add(newUser);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allUsers;
    }

    public override bool Equals(System.Object randomUser)
    {
      if(!(randomUser is User))
      {
        return false;
      }
      else
      {
        User newUser = (User) randomUser;
        bool newEqual = (this.userId == newUser.userId) && (this.name == newUser.name) && (this.description == newUser.description);
        return newEqual;
      }
    }

    public void DeleteUser(int userId)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmdDelete = new SqlCommand("DELETE FROM users WHERE id = @UserId;DELETE FROM users_genders WHERE user_id = @UserId;DELETE FROM users_works WHERE user_id = @UserId;DELETE FROM users_foods WHERE user_id = @UserId;DELETE FROM users_hobbies WHERE user_id = @UserId;DELETE FROM users_avatars WHERE user_id = @UserId;", conn);
        cmdDelete.Parameters.Add(new SqlParameter("@UserId", userId.ToString()));
        cmdDelete.ExecuteNonQuery();
        DB.CloseSqlConnection(conn);
    }

    public List<int> GetLikedUsers()
    {
        List<int> likedUserIds = new List<int>{};
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM likes WHERE userLiking_id = @UserId;", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));

        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
          likedUserIds.Add(rdr.GetInt32(2));
        }
        DB.CloseSqlConnection(conn, rdr);
        return likedUserIds;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO users (name, description) OUTPUT INSERTED.id VALUES (@UserName, @UserDescription);", conn);
      cmd.Parameters.Add(new SqlParameter("@UserName", this.name));
      cmd.Parameters.Add(new SqlParameter("@UserDescription", this.description));

      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this.userId = rdr.GetInt32(0);
      }
      DB.CloseSqlConnection(conn, rdr);
    }

    public void AddGender(string genderValue)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int genderId = 0;
        SqlCommand cmd = new SqlCommand();

        if (!CheckExistence("genders", genderValue))
        {
            cmd.CommandText = "INSERT INTO genders (gender) OUTPUT INSERTED.id VALUES (@UserGender);";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM genders WHERE gender = @UserGender;";
        }
        cmd.Connection = conn;
        cmd.Parameters.Add(new SqlParameter("@UserGender", genderValue));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            genderId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO users_genders (user_id, gender_id) VALUES (@UserId, @GenderId);", conn);
        cmd2.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmd2.Parameters.Add(new SqlParameter("@GenderId", genderId.ToString()));

        cmd2.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public static List<string> GetAllGenders()
    {
        List<string> allGenders = new List<string>{};
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM genders;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
          allGenders.Add(rdr.GetString(1));
        }

        DB.CloseSqlConnection(conn, rdr);
        return allGenders;
    }

    public List<string> GetGenders()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<string> genderList = new List<string>{};

        SqlCommand cmd = new SqlCommand("SELECT genders.* FROM users JOIN users_genders ON (users.id = users_genders.user_id) JOIN genders ON (users_genders.gender_id = genders.id) WHERE users.id = @UserId;", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            genderList.Add(rdr.GetString(1));
        }
        DB.CloseSqlConnection(conn, rdr);
        return genderList;
    }

    public void DeleteGender(string gender)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int genderId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM genders WHERE gender = @GenderName;", conn);
        cmdQuery.Parameters.Add(new SqlParameter("@GenderName", gender));
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            genderId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmdDelete = new SqlCommand("DELETE FROM users_genders WHERE user_id = @UserId AND gender_id = @GenderId;", conn);
        cmdDelete.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmdDelete.Parameters.Add(new SqlParameter("@GenderId", genderId.ToString()));
        cmdDelete.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public void AddWork(string workValue)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int workId = 0;
        SqlCommand cmd = new SqlCommand();

        if (!CheckExistence("works", workValue))
        {
            cmd.CommandText = "INSERT INTO works (work) OUTPUT INSERTED.id VALUES (@UserWork);";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM works WHERE work = @UserWork;";
        }
        cmd.Connection = conn;
        cmd.Parameters.Add(new SqlParameter("@UserWork", workValue));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            workId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO users_works (user_id, work_id) VALUES (@UserId, @WorkId);", conn);
        cmd2.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmd2.Parameters.Add(new SqlParameter("@WorkId", workId.ToString()));

        cmd2.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public static List<string> GetAllWorks()
    {
        List<string> allWorks = new List<string>{};
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM works;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
          allWorks.Add(rdr.GetString(1));
        }

        DB.CloseSqlConnection(conn, rdr);
        return allWorks;
    }

    public List<string> GetWorks()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<string> workList = new List<string>{};

        SqlCommand cmd = new SqlCommand("SELECT works.* FROM users JOIN users_works ON (users.id = users_works.user_id) JOIN works ON (users_works.work_id = works.id) WHERE users.id = @UserId;", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            workList.Add(rdr.GetString(1));
        }
        DB.CloseSqlConnection(conn, rdr);
        return workList;
    }

    public void DeleteWork(string work)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int workId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM works WHERE work = @WorkName;", conn);
        cmdQuery.Parameters.Add(new SqlParameter("@WorkName", work));
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            workId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmdDelete = new SqlCommand("DELETE FROM users_works WHERE user_id = @UserId AND work_id = @WorkId;", conn);
        cmdDelete.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmdDelete.Parameters.Add(new SqlParameter("@WorkId", workId.ToString()));
        cmdDelete.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public void AddFood(string foodValue)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int foodId = 0;
        SqlCommand cmd = new SqlCommand();

        if (!CheckExistence("foods", foodValue))
        {
            cmd.CommandText = "INSERT INTO foods (food) OUTPUT INSERTED.id VALUES (@UserFood);";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM foods WHERE food = @UserFood;";
        }
        cmd.Connection = conn;
        cmd.Parameters.Add(new SqlParameter("@UserFood", foodValue));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            foodId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO users_foods (user_id, food_id) VALUES (@UserId, @FoodId);", conn);
        cmd2.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmd2.Parameters.Add(new SqlParameter("@FoodId", foodId.ToString()));

        cmd2.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public static List<string> GetAllFoods()
    {
        List<string> allFoods = new List<string>{};
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM foods;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
          allFoods.Add(rdr.GetString(1));
        }

        DB.CloseSqlConnection(conn, rdr);
        return allFoods;
    }

    public List<string> GetFoods()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<string> foodList = new List<string>{};

        SqlCommand cmd = new SqlCommand("SELECT foods.* FROM users JOIN users_foods ON (users.id = users_foods.user_id) JOIN foods ON (users_foods.food_id = foods.id) WHERE users.id = @UserId;", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            foodList.Add(rdr.GetString(1));
        }
        DB.CloseSqlConnection(conn, rdr);
        return foodList;
    }

    public void DeleteFood(string food)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int foodId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM foods WHERE food = @FoodName;", conn);
        cmdQuery.Parameters.Add(new SqlParameter("@FoodName", food));
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            foodId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmdDelete = new SqlCommand("DELETE FROM users_foods WHERE user_id = @UserId AND food_id = @FoodId;", conn);
        cmdDelete.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmdDelete.Parameters.Add(new SqlParameter("@FoodId", foodId.ToString()));
        cmdDelete.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public void AddHobby(string hobbyValue)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int hobbyId = 0;
        SqlCommand cmd = new SqlCommand();

        if (!CheckExistence("hobbies", hobbyValue))
        {
            cmd.CommandText = "INSERT INTO hobbies (hobby) OUTPUT INSERTED.id VALUES (@UserHobby);";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM hobbies WHERE hobby = @UserHobby;";
        }
        cmd.Connection = conn;
        cmd.Parameters.Add(new SqlParameter("@UserHobby", hobbyValue));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            hobbyId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd2 = new SqlCommand("INSERT INTO users_hobbies (user_id, hobby_id) VALUES (@UserId, @HobbyId);", conn);
        cmd2.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmd2.Parameters.Add(new SqlParameter("@HobbyId", hobbyId.ToString()));

        cmd2.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public static List<string> GetAllHobbies()
    {
        List<string> allHobbies = new List<string>{};
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM hobbies;", conn);
        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
          allHobbies.Add(rdr.GetString(1));
        }

        DB.CloseSqlConnection(conn, rdr);
        return allHobbies;
    }

    public List<string> GetHobbies()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<string> hobbyList = new List<string>{};

        SqlCommand cmd = new SqlCommand("SELECT hobbies.* FROM users JOIN users_hobbies ON (users.id = users_hobbies.user_id) JOIN hobbies ON (users_hobbies.hobby_id = hobbies.id) WHERE users.id = @UserId;", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            hobbyList.Add(rdr.GetString(1));
        }
        DB.CloseSqlConnection(conn, rdr);
        return hobbyList;
    }

    public void DeleteHobby(string hobby)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int hobbyId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM hobbies WHERE hobby = @HobbyName;", conn);
        cmdQuery.Parameters.Add(new SqlParameter("@HobbyName", hobby));
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            hobbyId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmdDelete = new SqlCommand("DELETE FROM users_hobbies WHERE user_id = @UserId AND hobby_id = @HobbyId;", conn);
        cmdDelete.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
        cmdDelete.Parameters.Add(new SqlParameter("@HobbyId", hobbyId.ToString()));
        cmdDelete.ExecuteNonQuery();
        DB.CloseSqlConnection(conn, rdr);
    }

    public static bool CheckExistence(string tableName, string rowValue)
    {
        bool existence = false;
        SqlConnection conn = DB.Connection();
        conn.Open();
        SqlCommand cmd = new SqlCommand();

        if (tableName == "genders")
        {
            cmd.CommandText = "SELECT * FROM genders WHERE gender = @RowValue;";
        }
        else if(tableName == "works")
        {
            cmd.CommandText = "SELECT * FROM works WHERE work = @RowValue;";
        }
        else if(tableName == "hobbies")
        {
            cmd.CommandText = "SELECT * FROM hobbies WHERE hobby = @RowValue;";
        }
        else
        {
            cmd.CommandText = "SELECT * FROM foods WHERE food = @RowValue;";
        }
        cmd.Connection = conn;

        cmd.Parameters.Add(new SqlParameter("@RowValue", rowValue));

        SqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            existence = true;
        }

        DB.CloseSqlConnection(conn, rdr);
        return existence;
    }

    public void AddLike(int idLiked)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO likes (userLiking_id, userLiked_id) VALUES (@UserLiking, @UserLiked);", conn);
        cmd.Parameters.Add(new SqlParameter("@UserLiking", this.userId.ToString()));
        cmd.Parameters.Add(new SqlParameter("@UserLiked", idLiked.ToString()));

        cmd.ExecuteNonQuery();
        DB.CloseSqlConnection(conn);
    }



    public void DeleteLike(int idLiked)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("DELETE FROM likes WHERE userLiked_id = @UserLiked AND userLiking_id = @UserLiking;", conn);
        cmd.Parameters.Add(new SqlParameter("@UserLiked", idLiked.ToString()));
        cmd.Parameters.Add(new SqlParameter("@UserLiking", this.userId.ToString()));

        cmd.ExecuteNonQuery();
        DB.CloseSqlConnection(conn);
    }

    public static List<User> Filter(Dictionary<string, string> preferences, int locationId = 0)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<User> foundUsers = new List<User>{};

        if(preferences["rating"] != "no preference")
        {
            foundUsers = FindByMinRating(Int32.Parse(preferences["rating"]));
            if (locationId != 0)
            {
                List<User> geographicallyFilteredUsers = Location.FindNearbyUsers(locationId);
                foundUsers = foundUsers.Intersect(geographicallyFilteredUsers).ToList();
            }
            if (foundUsers.Count == 0)
            {
                return foundUsers;
            }
        }
        if(preferences["gender"] != "no preference")
        {
            if(foundUsers.Count > 0)
            {
                foundUsers = FindByGender(preferences["gender"], foundUsers);
            }
            else
            {
                foundUsers = FindByGender(preferences["gender"], User.GetAll());
            }
            if (foundUsers.Count == 0)
            {
                return foundUsers;
            }
        }
        if(preferences["work"] != "no preference")
        {
            if(foundUsers.Count > 0)
            {
                foundUsers = FindByWork(preferences["work"], foundUsers);
            }
            else
            {
                foundUsers = FindByWork(preferences["work"], User.GetAll());
            }
            if (foundUsers.Count == 0)
            {
                return foundUsers;
            }
        }
        if(preferences["food"] != "no preference")
        {
            if(foundUsers.Count > 0)
            {
                foundUsers = FindByFood(preferences["food"], foundUsers);
            }
            else
            {
                foundUsers = FindByFood(preferences["food"], User.GetAll());
            }
            if (foundUsers.Count == 0)
            {
                return foundUsers;
            }
        }
        if(preferences["hobby"] != "no preference")
        {
            if(foundUsers.Count > 0)
            {
                foundUsers = FindByHobby(preferences["hobby"], foundUsers);
            }
            else
            {
                foundUsers = FindByHobby(preferences["hobby"], User.GetAll());
            }
        }
        return foundUsers;
    }

    public static List<User> FindByGender(string gender, List<User> passedList)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<User> newList = new List<User>{};
        int genderId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM genders WHERE gender = @GenderName;", conn);
        cmdQuery.Parameters.Add("@GenderName", gender);
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            genderId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd = new SqlCommand("SELECT users.* FROM genders JOIN users_genders ON (genders.id = users_genders.gender_id) JOIN users ON (users_genders.user_id = users.id) WHERE genders.id = @GenderId", conn);

        cmd.Parameters.Add("@GenderId", genderId.ToString());
        rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
            User foundUser = new User(rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(0));
            if (passedList.Contains(foundUser))
            {
                newList.Add(new User(rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(0)));
            }
        }

        DB.CloseSqlConnection(conn, rdr);
        return newList;
    }

    public static List<User> FindByWork(string work, List<User> passedList)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<User> userList = new List<User>{};
        int workId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM works WHERE work = @WorkName;", conn);
        cmdQuery.Parameters.Add("@WorkName", work);
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            workId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd = new SqlCommand("SELECT users.* FROM works JOIN users_works ON (works.id = users_works.work_id) JOIN users ON (users_works.user_id = users.id) WHERE works.id = @WorkId", conn);

        cmd.Parameters.Add("@WorkId", workId.ToString());
        rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
            User foundUser = new User(rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(0));
            if (passedList.Contains(foundUser))
            {
                userList.Add(foundUser);
            }
        }

        DB.CloseSqlConnection(conn, rdr);
        return userList;
    }

    public static List<User> FindByFood(string food, List<User> passedList)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<User> userList = new List<User>{};
        int foodId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM foods WHERE food = @FoodName;", conn);
        cmdQuery.Parameters.Add("@FoodName", food);
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            foodId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd = new SqlCommand("SELECT users.* FROM foods JOIN users_foods ON (foods.id = users_foods.food_id) JOIN users ON (users_foods.user_id = users.id) WHERE foods.id = @FoodId", conn);

        cmd.Parameters.Add("@FoodId", foodId.ToString());
        rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
            User foundUser = new User(rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(0));
            if(passedList.Contains(foundUser))
            {
                userList.Add(foundUser);
            }
        }

        DB.CloseSqlConnection(conn, rdr);
        return userList;
    }


public static List<User> FilterCurrentUser(int id, List<User> userList)
{
  for(int i = userList.Count - 1; i >= 0; i--)
  {
    if (userList[i].userId == id)
    {
      userList.RemoveAt(i);
    }
  }
  return userList;
}
    public static List<User> FindByHobby(string hobby, List<User> passedList)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<User> userList = new List<User>{};
        int hobbyId = 0;
        SqlCommand cmdQuery = new SqlCommand("SELECT * FROM hobbies WHERE hobby = @HobbyName;", conn);
        cmdQuery.Parameters.Add("@HobbyName", hobby);
        SqlDataReader rdr = cmdQuery.ExecuteReader();
        while(rdr.Read())
        {
            hobbyId = rdr.GetInt32(0);
        }

        if(rdr != null)
        {
          rdr.Close();
        }

        SqlCommand cmd = new SqlCommand("SELECT users.* FROM hobbies JOIN users_hobbies ON (hobbies.id = users_hobbies.hobby_id) JOIN users ON (users_hobbies.user_id = users.id) WHERE hobbies.id = @HobbyId;", conn);

        cmd.Parameters.Add("@HobbyId", hobbyId.ToString());
        rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
            User foundUser = new User(rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(0));
            if (passedList.Contains(foundUser))
            {
                userList.Add(foundUser);
            }
        }

        DB.CloseSqlConnection(conn, rdr);
        return userList;
    }

    public static List<User> FindByMinRating (int rating)
    {
        List<User> orderedList = User.GetUsersByAscendingRatingOrder();
        int count = 0;
        while ((int)orderedList[count].GetAverageRating() < rating)
        {
            count++;
            if (count == orderedList.Count)
            {
                return (new List<User>{});
            }
        }

        return orderedList.GetRange(count, orderedList.Count - count);
    }

    public static void AddRating(int id, int rating)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("INSERT INTO ratings (user_rated_id, rating) VALUES (@UserId, @RatingId);", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", id));
        cmd.Parameters.Add(new SqlParameter("@RatingId", rating));
        cmd.ExecuteNonQuery();
        DB.CloseSqlConnection(conn);
    }

    public double GetAverageRating()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        List<int> ratingList = new List<int>{};

        SqlCommand cmd = new SqlCommand("SELECT * FROM ratings WHERE user_rated_id = @UserId;", conn);
        cmd.Parameters.Add("@UserId", this.userId);
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
            ratingList.Add(rdr.GetInt32(2));
        }

        if(ratingList.Count > 0)
        {
            double averageRating = (double)ratingList.Sum()/(double)ratingList.Count;
            return averageRating;
        }
        else
        {
            return 0;
        }

    }

    public static List<User> GetUsersByAscendingRatingOrder()
    {
        List<User> sortedList = User.GetAll().OrderBy(o=>o.GetAverageRating()).ToList();
        return sortedList;
    }

    public bool CheckIfLatestUser()
    {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT TOP 1 id FROM users ORDER BY id DESC;", conn);
        int lastId = 0;
        SqlDataReader rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
            lastId = rdr.GetInt32(0);
        }

        DB.CloseSqlConnection(conn, rdr);
        return (lastId == this.userId);
    }

        public static User Find(int id)
        {
        User foundUser = new User("", "");
        SqlConnection conn = DB.Connection();
        conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE id = @UserId;", conn);
          cmd.Parameters.Add(new SqlParameter("@UserId", id.ToString()));
          SqlDataReader rdr = cmd.ExecuteReader();

          while (rdr.Read())
          {
            foundUser.userId = rdr.GetInt32(0);
            foundUser.name = rdr.GetString(1);
            foundUser.description = rdr.GetString(2);
          }

          DB.CloseSqlConnection(conn, rdr);
          return foundUser;
        }

        public void UpdateUsersName(string newName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE users SET name = @NewName OUTPUT INSERTED.* WHERE id = @UserId;", conn);

            cmd.Parameters.Add(new SqlParameter("@NewName", newName));

            cmd.Parameters.Add(new SqlParameter("@UserId", userId.ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this.name = rdr.GetString(1);
            }

            DB.CloseSqlConnection(conn, rdr);
        }

        public void UpdateUsersDescription(string newDescription)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE users SET description = @NewDescription OUTPUT INSERTED.* WHERE id = @UserId;", conn);

            cmd.Parameters.Add(new SqlParameter("@NewDescription", newDescription));

            cmd.Parameters.Add(new SqlParameter("@UserId", userId.ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this.description = rdr.GetString(2);
            }

            DB.CloseSqlConnection(conn, rdr);
        }

        public void AddAvatarToUser(Avatar thisAvatar)
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("INSERT INTO users_avatars (user_id, avatar_id) VALUES (@UserId, @AvatarId);", conn);
          cmd.Parameters.Add("@UserId", this.userId.ToString());
          cmd.Parameters.Add("@AvatarId", thisAvatar.avatarId.ToString());
          cmd.ExecuteNonQuery();
          DB.CloseSqlConnection(conn);
        }

        public Avatar GetAvatar()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("SELECT avatars.* FROM users JOIN users_avatars ON (users.id = users_avatars.user_id) JOIN avatars ON (avatars.id = users_avatars.avatar_id) WHERE users.id = @UserId;", conn);
          cmd.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
          SqlDataReader rdr = cmd.ExecuteReader();

          Avatar foundAvatar = new Avatar(null,null,0);
          while(rdr.Read())
          {
            foundAvatar.avatarId = rdr.GetInt32(0);
            foundAvatar.avatarPath = rdr.GetString(2);
            foundAvatar.avatarBinary = System.Text.Encoding.Default.GetString((byte[]) rdr.GetValue(1));
          }
          DB.CloseSqlConnection(conn, rdr);
          return foundAvatar;
        }

        public Location GetLocation()
        {

          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("SELECT locations.id FROM users JOIN users_locations ON (users.id = users_locations.user_id) JOIN locations ON (locations.id = users_locations.location_id) WHERE users.id = @UserId;", conn);
          cmd.Parameters.Add(new SqlParameter("@UserId", this.userId.ToString()));
          SqlDataReader rdr = cmd.ExecuteReader();
          Location foundLocation = new Location("");
          while(rdr.Read())
          {
            foundLocation = Location.Find(rdr.GetInt32(0));
          }
          DB.CloseSqlConnection(conn, rdr);
          return foundLocation;
        }
    }
}
