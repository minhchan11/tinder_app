using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TinderApp
{
  public class Avatar
  {
    private int _id;
    private string _path;
    private string _binaryValue;

    public Avatar(string path, string binaryValue = null, int Id = 0 )
    {
      _id = Id;
      _path = path;
      _binaryValue = binaryValue;
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

    public string userPath
    {
      get
      {
        return this._path;
      }
      set
      {
        this._path = value;
      }
    }

    public string binaryValue
    {
      get
      {
        return this._binaryValue;
      }
      set
      {
        this._binaryValue = value;
      }
    }

    // public void AddImage(Image newImage)
    // {
    //
    // }

    public static void DeleteAll()
    {
      DB.DeleteAll("avatars");
    }

    public static List<Avatar> GetAll()
       {
         List<Avatar> allAvatars = new List<Avatar>{};
         SqlConnection conn = DB.Connection();
         conn.Open();

         SqlCommand cmd = new SqlCommand("SELECT * FROM avatars;", conn);
         SqlDataReader rdr = cmd.ExecuteReader();

         while(rdr.Read())
         {
           Avatar newAvatar = new Avatar(rdr.GetString(1), System.Text.Encoding.Default.GetString((byte[]) rdr.GetValue(2)), rdr.GetInt32(0));
           allAvatars.Add(newAvatar);
         }

         DB.CloseSqlConnection(conn, rdr);
         return allAvatars;
       }

  }
}
