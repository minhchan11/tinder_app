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

    public int avatarId
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

    public string avatarPath
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

    public string avatarBinary
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

   public override bool Equals(System.Object randomAvatar)
       {
         if(!(randomAvatar is Avatar))
         {
           return false;
         }
         else
         {
           Avatar newAvatar = (Avatar) randomAvatar;
           bool newEqual = (this.avatarId == newAvatar.avatarId) && (this.avatarPath == newAvatar.avatarPath) && (this.avatarBinary == newAvatar.avatarBinary);
           return newEqual;
         }
       }

  }
}
