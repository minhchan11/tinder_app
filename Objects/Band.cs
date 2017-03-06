using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTrackerApp
{
  public class Band
  {
    private int _id;
    private string _name;

    public Band(string bandName, int bandId = 0){
      _id = bandId;
      _name = bandName;
    }

    public string bandName {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }


    public int bandId
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
        Band newBand = new Band(rdr.GetString(1), rdr.GetInt32(0));
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
        bool newEqual = (this.bandId == newBand.bandId) && (this.bandName == newBand.bandName);
        return newEqual;
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@BandName)", conn);
      cmd.Parameters.Add(new SqlParameter("@BandName", this.bandName));
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this.bandId = rdr.GetInt32(0);
      }
      DB.CloseSqlConnection(conn, rdr);
    }

        public void DeleteThis()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @TargetId; DELETE FROM bands_venues WHERE band_id = @TargetId;", conn);
          cmd.Parameters.Add(new SqlParameter("@TargetId", this.bandId));
          cmd.ExecuteNonQuery();
          DB.CloseSqlConnection(conn);
        }
    //
        public void Update(string newName)
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("UPDATE bands SET name = @NewName WHERE id = @TargetId;", conn);
          cmd.Parameters.Add(new SqlParameter("@NewName", newName));
          cmd.Parameters.Add(new SqlParameter("@TargetId", this.bandId));
          cmd.ExecuteNonQuery();
          this.bandName = newName;
          DB.CloseSqlConnection(conn);
        }
    //
    public static Band Find(int id)
        {
        Band foundBand = new Band("");
        SqlConnection conn = DB.Connection();
        conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", conn);
          cmd.Parameters.Add(new SqlParameter("@BandId", id.ToString()));
          SqlDataReader rdr = cmd.ExecuteReader();

          while (rdr.Read())
          {
            foundBand.bandId = rdr.GetInt32(0);
            foundBand.bandName = rdr.GetString(1);
          }

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
