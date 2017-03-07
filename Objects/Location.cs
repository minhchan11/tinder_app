using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TinderApp
{
  public class Location
  {
    private int _id;
    private string _coord;


    public Location(string coord, int locationId = 0){
      _id = locationId;
      _coord = coord;
    }

    public string locationCoord {
      get
      {
        return this._coord;
      }
      set
      {
        this._coord = value;
      }
    }


    public int locationId
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
      DB.DeleteAll("locations");
    }

    public static List<Location> GetAll()
       {
         List<Location> allLocations = new List<Location>{};
         SqlConnection conn = DB.Connection();
         conn.Open();

         SqlCommand cmd = new SqlCommand("SELECT id, location.STAsText() FROM locations;", conn);
         SqlDataReader rdr = cmd.ExecuteReader();

         while(rdr.Read())
         {
           Location newLocation = new Location(rdr.GetString(1), rdr.GetInt32(0));
           allLocations.Add(newLocation);
         }

         DB.CloseSqlConnection(conn, rdr);
         return allLocations;
       }

       public override bool Equals(System.Object randomLocation)
           {
             if(!(randomLocation is Location))
             {
               return false;
             }
             else
             {
               Location newLocation = (Location) randomLocation;
               bool newEqual = (this.locationId == newLocation.locationId) && (this.locationCoord == newLocation.locationCoord);
               return newEqual;
             }
           }


     public void Save()
         {
           SqlConnection conn = DB.Connection();
           conn.Open();
           SqlCommand cmd = new SqlCommand("INSERT INTO locations (location) OUTPUT INSERTED.id VALUES (geography::STGeomFromText(@LocationCoord, 4326));", conn);
           cmd.Parameters.Add("@LocationCoord", this.locationCoord);
           SqlDataReader rdr = cmd.ExecuteReader();
           while(rdr.Read())
           {
             this.locationId = rdr.GetInt32(0);
           }
           DB.CloseSqlConnection(conn, rdr);
         }

    public static Location Find(int id)
        {
          Location foundLocation = new Location("");
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT id, location.STAsText() FROM locations WHERE id = @LocationId;", conn);
          cmd.Parameters.Add(new SqlParameter("@LocationId", id.ToString()));
          SqlDataReader rdr = cmd.ExecuteReader();

          while (rdr.Read())
          {
            foundLocation.locationId = rdr.GetInt32(0);
            foundLocation.locationCoord = rdr.GetString(1);
          }

          DB.CloseSqlConnection(conn, rdr);
          return foundLocation;
        }

      public static List<Location> FindNearby(Location searchLocation)
      {
        List<Location> matchedLocations = new List<Location>{};
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("DECLARE @g geography; SET @g = geography::STGeomFromText(@LocationCoord, 4326); SELECT id, location.STAsText() FROM locations WHERE radius.STContains(@g) = 'true' AND id <> @LocationId;", conn);
        cmd.Parameters.Add(new SqlParameter("@LocationCoord", searchLocation.locationCoord));
        cmd.Parameters.Add(new SqlParameter("@LocationId", searchLocation.locationId));
        SqlDataReader rdr = cmd.ExecuteReader();
        while(rdr.Read())
        {
          Location newLocation = new Location(rdr.GetString(1), rdr.GetInt32(0));
          matchedLocations.Add(newLocation);
        }

        DB.CloseSqlConnection(conn, rdr);
        return matchedLocations;
      }

      public void Update(string newLocation)
      {
        SqlConnection conn = DB.Connection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("UPDATE locations SET location = @newLocation WHERE id = @TargetId;", conn);
        cmd.Parameters.Add(new SqlParameter("@newLocation", newLocation));
        cmd.Parameters.Add(new SqlParameter("@TargetId", this.locationId));
        cmd.ExecuteNonQuery();
        this.locationCoord = newLocation;
        DB.CloseSqlConnection(conn);
      }

  }
}
