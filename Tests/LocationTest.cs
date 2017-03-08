// using System;
// using System.Collections.Generic;
// using System.Data.SqlClient;
// using Xunit;
//
//
// namespace TinderApp
// {
//   public class LocationTest: IDisposable
//   {
//     public LocationTest()
//     {
//       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=tinder_test;Integrated Security=SSPI;";
//     }
//
//     public void Dispose()
//     {
//       Location.DeleteAll();
//     }
//
//     [Fact]
//         public void GetAll_DatabaseEmptyAtFirst_ZeroOutput()
//         {
//           //Arrange, Act
//           int result = Location.GetAll().Count;
//
//           //Assert
//           Assert.Equal(0, result);
//         }
//     [Fact]
//         public void OverrideBool_SameLocation_ReturnsEqual()
//         {
//           //Arrange, Act
//           Location placeOne = new Location ("POINT (-73.9949905872345 40.728616558706655)");
//           Location placeTwo = new Location ("POINT (-73.9949905872345 40.728616558706655)");
//
//           //Assert
//           Assert.Equal(placeTwo, placeOne);
//         }
//     [Fact]
//         public void Save_NewLocation_SaveToDatabase()
//         {
//           //Arrange, Act
//           Location testPlace = new Location ("POINT (-73.9949905872345 40.728616558706655)");
//           testPlace.Save();
//           //Assert
//           Location result = Location.GetAll()[0];
//           Assert.Equal(testPlace, result);
//         }
//     [Fact]
//         public void Save_NewLocations_SaveToDatabase()
//         {
//           //Arrange, Act
//           Location placeOne = new Location ("POINT (-73.9949905872345 40.728616558706655)");
//           Location placeTwo = new Location ("POINT (-73.9949905872345 40.728616558706655)");
//           placeOne.Save();
//           placeTwo.Save();
//           //Assert
//           List<Location> result = Location.GetAll();
//           List<Location> expected = new List<Location> {placeOne, placeTwo};
//           Assert.Equal(expected, result);
//         }
//     [Fact]
//         public void Save_OneLocation_LocationSavedWithCorrectID()
//         {
//           //Arrange
//           Location testLocation = new Location ("POINT (-73.9949905872345 40.728616558706655)");
//           testLocation.Save();
//           Location savedLocation = Location.GetAll()[0];
//
//           //Act
//           int output = savedLocation.locationId;
//           int verify = testLocation.locationId;
//
//           //Assert
//           Assert.Equal(verify, output);
//         }
//
//     [Fact]
//         public void Find_LocationId_LocationWithIdFound()
//         {
//           // Arrange
//           Location testLocation = new Location ("POINT (-73.9949905872345 40.728616558706655)");
//           testLocation.Save();
//
//           //Act
//           Location foundLocation = Location.Find(testLocation.locationId);
//
//           //Assert
//           Assert.Equal(testLocation, foundLocation);
//         }
//
//     [Fact]
//         public void FindNearby_Location_ListOfMatchedLocation()
//         {
//           // Arrange
//           Location locationOne = new Location ("POINT (-73.993808 40.702999)");
//           locationOne.Save();
//           Location locationTwo = new Location ("POINT (-73.994014 40.703058)");
//           locationTwo.Save();
//
//
//           //Act
//           List<Location> output= Location.FindNearby(locationOne);
//           List<Location> verify = new List<Location>{Location.Find(locationTwo.locationId)};
//
//           //Assert
//           Assert.Equal(verify, output);
//         }
//     [Fact]
//         public void Location_Update_UpdateDatabaseAndLocalObject()
//         {
//           Location testLocation = new Location ("POINT (-73.993808 40.702999)");
//           testLocation.Save();
//
//           testLocation.Update("POINT (-73.994014 40.703058)");
//           Location expectedLocation = new Location("POINT (-73.994014 40.703058)", testLocation.locationId);
//
//           Assert.Equal(expectedLocation, Location.Find(testLocation.locationId));
//         }
//
//   }
// }
