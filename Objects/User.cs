using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TinderApp
{
  public class User
  {
    private int _id;
    private string _name;
    private string _description;

    public User(string name, string description, int userId = 0){
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

        // public void DeleteThis()
        // {
        //   SqlConnection conn = DB.Connection();
        //   conn.Open();
        //   SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE id = @TargetId; DELETE FROM users_venues WHERE user_id = @TargetId;", conn);
        //   cmd.Parameters.Add(new SqlParameter("@TargetId", this.userId));
        //   cmd.ExecuteNonQuery();
        //   DB.CloseSqlConnection(conn);
        // }
    //
        // public void Update(string newName)
        // {
        //   SqlConnection conn = DB.Connection();
        //   conn.Open();
        //
        //   SqlCommand cmd = new SqlCommand("UPDATE users SET name = @NewName WHERE id = @TargetId;", conn);
        //   cmd.Parameters.Add(new SqlParameter("@NewName", newName));
        //   cmd.Parameters.Add(new SqlParameter("@TargetId", this.userId));
        //   cmd.ExecuteNonQuery();
        //   this.name = newName;
        //   DB.CloseSqlConnection(conn);
        // }

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

        // public static List<User> SearchName(string name)
        // {
        //   List<User> foundUsers = new List<User>{};
        //   SqlConnection conn = DB.Connection();
        //   conn.Open();
        //
        //   SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE name LIKE @UserName", conn);
        //   cmd.Parameters.Add(new SqlParameter("@UserName", "%" + name + "%"));
        //   SqlDataReader rdr = cmd.ExecuteReader();
        //
        //   while (rdr.Read())
        //   {
        //     int userId = rdr.GetInt32(0);
        //     string newName = rdr.GetString(1);
        //     User foundUser = new User(newName, userId);
        //     foundUsers.Add(foundUser);
        //   }
        //
        //   DB.CloseSqlConnection(conn, rdr);
        //   return foundUsers;
        // }
    }
}
