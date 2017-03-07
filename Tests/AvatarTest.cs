// using System;
// using System.Collections.Generic;
// using System.Data.SqlClient;
// using Xunit;
//
//
// namespace TinderApp
// {
//   public class AvatarTest: IDisposable
//   {
//     public AvatarTest()
//     {
//       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=tinder_test;Integrated Security=SSPI;";
//     }
//
//     public void Dispose()
//     {
//       Avatar.DeleteAll();
//     }
//
//     [Fact]
//         public void GetAll_DatabaseEmptyAtFirst_ZeroOutput()
//         {
//           //Arrange, Act
//           int result = Avatar.GetAll().Count;
//
//           //Assert
//           Assert.Equal(0, result);
//         }
//
//   }
// }
