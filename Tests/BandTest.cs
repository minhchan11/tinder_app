using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;


namespace TinderApp
{
  public class BandTest: IDisposable
  {
    public BandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=tinder_test;Integrated Security=SSPI;";
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
      int result = Band.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OverrideBool_SameBand_ReturnsEqual()
    {
      //Arrange, Act
      Band bandOne = new Band ("Green Day");
      Band bandTwo = new Band ("Green Day");

      //Assert
      Assert.Equal(bandTwo, bandOne);
    }

    [Fact]
    public void Save_OneBand_BandSavedToDatabase()
    {
      //Arrange
      Band testBand = new Band ("Green Day");

      //Act
      testBand.Save();
      List<Band> output = Band.GetAll();
      List<Band> verify = new List<Band>{testBand};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Save_OneBand_BandSavedWithCorrectID()
    {
      //Arrange
      Band testBand = new Band ("Green Day");
      testBand.Save();
      Band savedBand = Band.GetAll()[0];

      //Act
      int output = savedBand.GetId();
      int verify = testBand.GetId();

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void SaveGetAll_ManyBands_ReturnListOfBands()
    {
      //Arrange
      Band bandOne = new Band ("Green Day");
      bandOne.Save();
      Band bandTwo = new Band ("Spice Girl");
      bandTwo.Save();

      //Act
      List<Band> output = Band.GetAll();
      List<Band> verify = new List<Band>{bandOne, bandTwo};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Find_OneBandId_ReturnBandFromDatabase()
    {
      //Arrange
      Band testBand = new Band ("Green Day");
      testBand.Save();

      //Act
      Band foundBand = Band.Find(testBand.GetId());

      //Assert
      Assert.Equal(testBand, foundBand);
    }

    [Fact]
    public void SearchName_Name_ReturnBandFromDatabase()
    {
      //Arrange
      Band testBand = new Band ("Green Day");
      testBand.Save();

      //Act
      List<Band> output = Band.SearchName("Green Day");
      List<Band> verify = new List<Band>{testBand};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void AddVenue_OneBand_VenueAddedToJoinTable()
    {
      //Arrange
      Band testBand = new Band ("Green Day");
      testBand.Save();
      Venue testVenue = new Venue("Park");
      testVenue.Save();
      testBand.AddVenue(testVenue);

      //Act
      List<Venue> output = testBand.GetVenues();
      List<Venue> verify = new List<Venue>{testVenue};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void DeleteVenue_TwoVenueRemoveOne_VenueRemovedFromJoinTable()
    {
      //Arrange
      Band testBand = new Band ("Green Day");
      testBand.Save();
      Venue testVenue1 = new Venue("Park");
      Venue testVenue2 = new Venue("Manhattan Square");
      testVenue1.Save();
      testVenue2.Save();
      testBand.AddVenue(testVenue1);
      testBand.AddVenue(testVenue2);
      testBand.DeleteVenue(testVenue1);

      //Act
      List<Venue> output = testBand.GetVenues();
      List<Venue> verify = new List<Venue>{testVenue2};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void DeleteVenues_OneBand_VenueRemovedFromJoinTable()
    {
      //Arrange
      Band testBand = new Band ("Green Day");
      testBand.Save();
      Venue testVenue1 = new Venue("Park");
      Venue testVenue2 = new Venue("Manhattan Square");
      testVenue1.Save();
      testVenue2.Save();
      testBand.DeleteVenues();

      //Act
      int output = testBand.GetVenues().Count;
      //Assert
      Assert.Equal(0, output);
    }


    [Fact]
    public void Band_Delete_RemoveObjectFromDatabase()
    {
      Band testBand = new Band ("Green Day");
      testBand.Save();

      testBand.DeleteThis();

      Assert.Equal(0, Band.GetAll().Count);
    }

    [Fact]
    public void Band_Update_UpdateDatabaseAndLocalObject()
    {
      Band testBand = new Band ("Green Day");
      testBand.Save();

      testBand.Update("Yellow Day");
      Band expectedBand = new Band("Yellow Day", testBand.GetId());

      Assert.Equal(expectedBand, Band.Find(testBand.GetId()));
    }

    [Fact]
    public void Band_Save_NoSaveOnDuplicateBand()
    {
      Band testBand = new Band ("Green Day");
      testBand.Save();
      Band secondBand = new Band ("Green Day");
      secondBand.Save();

      Assert.Equal(1, Band.GetAll().Count);
    }

  }
}
