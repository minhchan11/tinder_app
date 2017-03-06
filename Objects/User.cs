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
        User newUser = new User(rdr.GetString(1), rdr.GetInt32(0));
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
        bool newEqual = (this.userId == newUser.userId) && (this.name == newUser.name) && (this.gender == newUser.gender) && (this.work == newUser.work) && (this.food == newUser.food) && (this.description == newUser.description);
        return newEqual;
      }
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

    public void AddGender(string gender)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int genderId = 0;

        if (!CheckExistence("genders", "gender", gender))
        {
            SqlCommand cmdGender = new SqlCommand("INSERT INTO genders (gender) OUTPUT INSERTED.id VALUES (@UserGender);", conn);
            cmdGender.Parameters.Add(new SqlParameter("@UserGender", gender));
            SqlDataReader rdr = cmdGender.ExecuteReader();
            while(rdr.Read())
            {
                genderId = rdr.GetInt32(0);
            }
        }
        else
        {
            SqlCommand cmdGender = new SqlCommand("SELECT * FROM genders WHERE gender = @UserGender;", conn);
            cmdGender.Parameters.Add(new SqlParameter("@UserGender", gender));
            SqlDataReader rdr = cmdGender.ExecuteReader();
            while(rdr.Read())
            {
                genderId = rdr.GetInt32(0);
            }
        }
        SqlCommand cmd = new SqlCommand("INSERT INTO users_genders (user_id, gender_id) VALUES (@UserId, @GenderId);", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.id.ToString()));
        cmd.Parameters.Add(new SqlParameter("@GenderId", genderId.ToString()));

        cmd.ExecuteNonQuery();
        DB.CloseSqlConnection(conn);
    }

    public void AddWork(string work)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int workId = 0;

        if (!CheckExistence("works", "work", work))
        {
            SqlCommand cmdWork = new SqlCommand("INSERT INTO works (work) OUTPUT INSERTED.id VALUES (@UserWork);", conn);
            cmdWork.Parameters.Add(new SqlParameter("@UserWork", work));
            SqlDataReader rdr = cmdWork.ExecuteReader();
            while(rdr.Read())
            {
                workId = rdr.GetInt32(0);
            }
        }
        else
        {
            SqlCommand cmdWork = new SqlCommand("SELECT * FROM works WHERE work = @UserWork;", conn);
            cmdWork.Parameters.Add(new SqlParameter("@UserWork", work));
            SqlDataReader rdr = cmdWork.ExecuteReader();
            while(rdr.Read())
            {
                workId = rdr.GetInt32(0);
            }
        }
        SqlCommand cmd = new SqlCommand("INSERT INTO users_works (user_id, work_id) VALUES (@UserId, @WorkId);", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.id.ToString()));
        cmd.Parameters.Add(new SqlParameter("@WorkId", workId.ToString()));

        cmd.ExecuteNonQuery();
        DB.CloseSqlConnection(conn);
    }

    public void AddFood(string food)
    {
        SqlConnection conn = DB.Connection();
        conn.Open();
        int foodId = 0;

        if (!CheckExistence("foods", "food", food))
        {
            SqlCommand cmdFood = new SqlCommand("INSERT INTO foods (food) OUTPUT INSERTED.id VALUES (@UserFood);", conn);
            cmdFood.Parameters.Add(new SqlParameter("@UserFood", food));
            SqlDataReader rdr = cmdFood.ExecuteReader();
            while(rdr.Read())
            {
                foodId = rdr.GetInt32(0);
            }
        }
        else
        {
            SqlCommand cmdFood = new SqlCommand("SELECT * FROM foods WHERE food = @UserFood;", conn);
            food.Parameters.Add(new SqlParameter("@UserFood", food));
            SqlDataReader rdr = cmdFood.ExecuteReader();
            while(rdr.Read())
            {
                foodId = rdr.GetInt32(0);
            }
        }
        SqlCommand cmd = new SqlCommand("INSERT INTO users_foods (user_id, food_id) VALUES (@UserId, @FoodId);", conn);
        cmd.Parameters.Add(new SqlParameter("@UserId", this.id.ToString()));
        cmd.Parameters.Add(new SqlParameter("@FoodId", foodId.ToString()));

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
    //
    // public static User Find(int id)
    //     {
    //     User foundUser = new User("");
    //     SqlConnection conn = DB.Connection();
    //     conn.Open();
    //
    //       SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE id = @UserId;", conn);
    //       cmd.Parameters.Add(new SqlParameter("@UserId", id.ToString()));
    //       SqlDataReader rdr = cmd.ExecuteReader();
    //
    //       while (rdr.Read())
    //       {
    //         foundUser.userId = rdr.GetInt32(0);
    //         foundUser.name = rdr.GetString(1);
    //       }
    //
    //       DB.CloseSqlConnection(conn, rdr);
    //       return foundUser;
    //     }

        public static bool CheckExistence(string tableName, string columnName, string rowValue)
        {
            bool existence = false;
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * From @TableName WHERE @ColumnName = @Value;", conn);
            cmd.Parameters.Add(new SqlParameter("@TableName", tableName));
            cmd.Parameters.Add(new SqlParameter("@ColumnName", columnName));
            cmd.Parameters.Add(new SqlParameter("@Value", rowValue));

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                existence = true;
            }

            DB.CloseSqlConnection(conn, rdr);
            return existence;
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
