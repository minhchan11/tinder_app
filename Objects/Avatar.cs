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
       Avatar newAvatar = new Avatar(rdr.GetString(2), System.Text.Encoding.Default.GetString((byte[]) rdr.GetValue(1)), rdr.GetInt32(0));
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

   public void Save()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("INSERT INTO avatars (path, image) OUTPUT INSERTED.id,INSERTED.image, INSERTED.path SELECT @ImagePath, BulkColumn FROM Openrowset(Bulk '"+this.avatarPath+"', SINGLE_BLOB) as tb;", conn);
     cmd.Parameters.Add("@ImagePath",  this.avatarPath);
     SqlDataReader rdr = cmd.ExecuteReader();
     while(rdr.Read())
     {
       this.avatarId = rdr.GetInt32(0);
       this.avatarPath = rdr.GetString(2);
       this.avatarBinary = System.Text.Encoding.Default.GetString((byte[]) rdr.GetValue(1));
     }
     DB.CloseSqlConnection(conn, rdr);
   }

   public static Avatar Find(int id)
   {
     Avatar foundAvatar = new Avatar("");
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("SELECT * FROM avatars WHERE id = @AvatarId;", conn);
     cmd.Parameters.Add(new SqlParameter("@AvatarId", id.ToString()));
     SqlDataReader rdr = cmd.ExecuteReader();

     while (rdr.Read())
     {
       foundAvatar.avatarId = rdr.GetInt32(0);
       foundAvatar.avatarPath = rdr.GetString(2);
       foundAvatar.avatarBinary = System.Text.Encoding.Default.GetString((byte[]) rdr.GetValue(1));
     }

     DB.CloseSqlConnection(conn, rdr);
     return foundAvatar;
   }

   public void Display()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("Declare @sql varchar(500) SET @sql = 'BCP \"SELECT tbBin FROM tinder.dbo.avatars where id=@Id\" QUERYOUT C:\\users\\epicodus\\desktop\\tinder_app\\Contents\\Profile\\output\\@Id.jpg -T -f C:\\users\\epicodus\\desktop\\tinder_app\\Resources\\testblob.fmt -S ' + '(localdb)\\mssqllocaldb'; EXEC master.dbo.xp_CmdShell @sql;", conn);
     cmd.Parameters.Add("@Id", this.avatarId);
     cmd.ExecuteNonQuery();
     DB.CloseSqlConnection(conn);
   }


  }
}
