using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;


namespace BandTrackerApp
{
  public class VenueTest: IDisposable
  {
    public VenueTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }

    [Fact]
    public void GetAll_DatabaseEmptyAtFirst_ZeroOutput()
    {
      //Arrange, Act
      int result = Venue.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OverrideBool_SameVenue_ReturnsEqual()
    {
      //Arrange, Act
      Venue venueOne = new Venue ("Manhattan Square");
      Venue venueTwo = new Venue ("Manhattan Square");

      //Assert
      Assert.Equal(venueTwo, venueOne);
    }

    [Fact]
    public void Save_OneVenue_VenueSavedToDatabase()
    {
      //Arrange
      Venue testVenue = new Venue ("Manhattan Square");

      //Act
      testVenue.Save();
      List<Venue> output = Venue.GetAll();
      List<Venue> verify = new List<Venue>{testVenue};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Save_OneVenue_VenueSavedWithCorrectID()
    {
      //Arrange
      Venue testVenue = new Venue ("Manhattan Square");
      testVenue.Save();
      Venue savedVenue = Venue.GetAll()[0];

      //Act
      int output = savedVenue.GetId();
      int verify = testVenue.GetId();

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void SaveGetAll_ManyVenues_ReturnListOfVenues()
    {
      //Arrange
      Venue venueOne = new Venue ("Manhattan Square");
      venueOne.Save();
      Venue venueTwo = new Venue ("Central Park");
      venueTwo.Save();

      //Act
      List<Venue> output = Venue.GetAll();
      List<Venue> verify = new List<Venue>{venueOne, venueTwo};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Find_OneVenueId_ReturnVenueFromDatabase()
    {
      //Arrange
      Venue testVenue = new Venue ("Manhattan Square");
      testVenue.Save();

      //Act
      Venue foundVenue = Venue.Find(testVenue.GetId());

      //Assert
      Assert.Equal(testVenue, foundVenue);
    }

    [Fact]
    public void SearchName_Name_ReturnVenueFromDatabase()
    {
      //Arrange
      Venue testVenue = new Venue ("Manhattan Square");
      testVenue.Save();

      //Act
      List<Venue> output = Venue.SearchName("Manhattan Square");
      List<Venue> verify = new List<Venue>{testVenue};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void AddBand_OneBand_BandAddedToJoinTable()
    {
      //Arrange
      Venue testVenue = new Venue ("Manhattan Square");
      testVenue.Save();
      Band testBand = new Band("Green Day");
      testBand.Save();
      testVenue.AddBand(testBand);

      //Act
      List<Band> output = testVenue.GetBands();
      List<Band> verify = new List<Band>{testBand};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void DeleteBand_TwoBandRemoveOne_BandRemovedFromJoinTable()
    {
      //Arrange
      Venue testVenue= new Venue("Park");
      testVenue.Save();
      Band testBand1 = new Band ("Green Day");
      testBand1.Save();
      Band testBand2 = new Band ("Spice Girl");
      testBand2.Save();
      testVenue.AddBand(testBand1);
      testVenue.AddBand(testBand2);
      testVenue.DeleteBand(testBand2);


      //Act
      List<Band> output = testVenue.GetBands();
      List<Band> verify = new List<Band>{testBand1};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void DeleteBands_OneVenue_AllBandsRemovedFromJoinTable()
    {
      //Arrange
      Venue testVenue= new Venue("Park");
      testVenue.Save();
      Band testBand1 = new Band ("Green Day");
      testBand1.Save();
      Band testBand2 = new Band ("Spice Girl");
      testBand2.Save();
      testVenue.AddBand(testBand1);
      testVenue.AddBand(testBand2);
      testVenue.DeleteBands();

      //Act
      int output = testVenue.GetBands().Count;
      //Assert
      Assert.Equal(0, output);
    }


    [Fact]
    public void Venue_Delete_RemoveObjectFromDatabase()
    {
      Venue testVenue = new Venue ("Manhattan Square");
      testVenue.Save();

      testVenue.DeleteThis();

      Assert.Equal(0, Venue.GetAll().Count);
    }

    [Fact]
    public void Venue_Update_UpdateDatabaseAndLocalObject()
    {
      Venue testVenue = new Venue ("Manhattan Square");
      testVenue.Save();

      testVenue.Update("Central Park");
      Venue expectedVenue = new Venue("Central Park", testVenue.GetId());

      Assert.Equal(expectedVenue, Venue.Find(testVenue.GetId()));
    }

    [Fact]
    public void Venue_Save_NoSaveOnDuplicateVenue()
    {
      Venue testVenue = new Venue ("Manhattan Square");
      testVenue.Save();
      Venue secondVenue = new Venue ("Manhattan Square");
      secondVenue.Save();

      Assert.Equal(1, Venue.GetAll().Count);
    }
  }
}
