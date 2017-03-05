using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTrackerApp
{
  public class Band
  {
    private int _id;
    private string _name;

    public Band(string bandName, int bandId = 0)
    {
      _id = bandId;
      _name = bandName;
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }

    public string GetName()
    {
      return _name;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public static void DeleteAll()
    {
      DB.DeleteAll("bands");
    }



    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        Band newBand = new Band(bandName, bandId);
        allBands.Add(newBand);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allBands;
    }

    public override bool Equals(System.Object randomBand)
    {
      if(!(randomBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) randomBand;
        bool idEquality = (this.GetId() == newBand.GetId());
        bool nameEquality = (this.GetName() == newBand.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      int potentialId = this.IsNewBand();
      if (potentialId == -1)
      {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@BandName) ;", conn);
      cmd.Parameters.Add(new SqlParameter("@BandName", this.GetName()));
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        potentialId = rdr.GetInt32(0);
      }

      DB.CloseSqlConnection(conn, rdr);
      }
      this.SetId(potentialId);
    }

    public void DeleteThis()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @TargetId; DELETE FROM bands_venues WHERE band_id = @TargetId;", conn);
      cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

      cmd.ExecuteNonQuery();
      DB.CloseSqlConnection(conn);
    }

    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE bands SET name = @NewName WHERE id = @TargetId;", conn);
      cmd.Parameters.Add(new SqlParameter("@NewName", newName));
      cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));
      cmd.ExecuteNonQuery();

      this.SetName(newName);
      DB.CloseSqlConnection(conn);
    }

    public int IsNewBand()
    {
      // Checks to see if an ingredient exists in the database already. If it does, returns the id. Otherwise, returns -1
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT id FROM bands WHERE name = @BandName", conn);
      cmd.Parameters.Add(new SqlParameter("@BandName", this.GetName()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = -1;
      while (rdr.Read())
      {
        foundId = rdr.GetInt32(0);
      }

      DB.CloseSqlConnection(conn, rdr);
      return foundId;
    }

    public static Band Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", conn);
      cmd.Parameters.Add(new SqlParameter("@BandId", id.ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int bandId = 0;
      string bandName = null;

      while (rdr.Read())
      {
        bandId = rdr.GetInt32(0);
        bandName = rdr.GetString(1);
      }

      Band foundBand = new Band(bandName, bandId);

      DB.CloseSqlConnection(conn, rdr);
      return foundBand;
    }

    public static List<Band> SearchName(string bandName)
    {
      List<Band> foundBands = new List<Band>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE name LIKE @BandName", conn);
      cmd.Parameters.Add(new SqlParameter("@BandName", "%" + bandName + "%"));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        Band foundBand = new Band(newName, bandId);
        foundBands.Add(foundBand);
      }

      DB.CloseSqlConnection(conn, rdr);
      return foundBands;
    }

    public void AddVenue(Venue newVenue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);", conn);
      cmd.Parameters.Add(new SqlParameter("@VenueId", newVenue.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId().ToString()));
      cmd.ExecuteNonQuery();

      DB.CloseSqlConnection(conn);
    }

    public void DeleteVenue(Venue newVenue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM bands_venues WHERE venue_id = @VenueId AND band_id = @BandId;", conn);
      cmd.Parameters.Add(new SqlParameter("@VenueId", newVenue.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId().ToString()));
      cmd.ExecuteNonQuery();

      DB.CloseSqlConnection(conn);
    }

    public void DeleteVenues()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM bands_venues WHERE band_id = @BandId;", conn);
      cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId().ToString()));
      cmd.ExecuteNonQuery();

      DB.CloseSqlConnection(conn);
    }

    public List<Venue> GetVenues()
    {
      List<Venue> allVenues = new List<Venue>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.band_id) JOIN venues ON (venues.id = bands_venues.venue_id) WHERE bands.id = @BandId;", conn);
      cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId().ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        Venue newVenue = new Venue(newName, newId);
        allVenues.Add(newVenue);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allVenues;
    }
  }
}
