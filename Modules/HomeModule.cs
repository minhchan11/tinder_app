using Nancy;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TinderApp
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/deleteAll"] =_=> {
        User.DeleteAll();
        Location.DeleteAll();
        Avatar.DeleteAll();
        return View["index.cshtml"];
      };

      Get["/"] = _ => {
          return View["index.cshtml"];
      };

      Get["/users/new"] = _ => View["users_form.cshtml"];

      Post["/users/new/added"] = _ => {
        User newUser = new User(Request.Form["user-name"], Request.Form["user-description"]);
        newUser.Save();
        Avatar newAvatar = new Avatar (Request.Form["value"]);
        newAvatar.Save();
        newAvatar.Display();
        newAvatar.DeleteJpg();
        Location newLocation = new Location(Request.Form["location"]);
        newLocation.Save();
        newLocation.AddUserToLocation(newUser.userId);
        newUser.AddAvatarToUser(newAvatar);
        newUser.AddGender(Request.Form["user-gender"]);
        newUser.AddWork(Request.Form["user-work"]);
        newUser.AddFood(Request.Form["user-food"]);
        newUser.AddHobby(Request.Form["user-hobby"]);
        List<User> allUsers = User.GetAll();
        Dictionary<string, object> Model = new Dictionary<string, object>{{"currentUser", newUser},{"avatar", newAvatar},{"allUsers", allUsers}, {"userId", newUser.userId}};
        return View["index-loggedin.cshtml", Model];
      };

      Get["/loggedin/{id}"] = parameters => {
        User newUser = User.Find(parameters.id);
        return View["index_loggedin.cshtml", newUser];
      };

      Get["/users"] = _ => {
        Dictionary<string, object> Model = new Dictionary<string, object>{};

        Model.Add("allgenders", User.GetAllGenders());
        Model.Add("allworks", User.GetAllWorks());
        Model.Add("allfoods", User.GetAllFoods());
        Model.Add("allhobbies", User.GetAllHobbies());
        return View["user.cshtml", Model];
      };

      Patch["/users/edit/{id}"] = parameters => {
        User targetUser = User.Find(parameters.id);
        targetUser.UpdateUsersName(Request.Form["new-user-name"]);
        return View["users.cshtml", User.GetAll()];
      };

      Delete["/users/{id}"] = parameters =>
      {
        User targetUser = User.Find(parameters.id);
        targetUser.DeleteUser(parameters.id);
        return View["index.cshtml"];
      };

      Get["/users/{currentId}/current"] = parameters => {
        User SelectedUser = User.Find(parameters.currentId);
        Avatar SelectedAvatar = SelectedUser.GetAvatar();
        List<User> allUsers = User.GetAll();
        Dictionary<string, object> Model = new Dictionary<string, object>{{"currentUser", SelectedUser},{"avatar", SelectedAvatar},{"allUsers", allUsers}, {"userId", parameters.currentId}};
        return View["index-loggedin.cshtml", Model];
      };


      // Get["/users/{id}"] = parameters => {
        // var SelectedUser = User.Find(parameters.id);
        // var UserUsers = SelectedUser.name;
        // Dictionary<string, object> Model = new Dictionary<string, object>{{"user", SelectedUser.userId},{"name", SelectedUser.name},{"description", SelectedUser.description}, {"gender", SelectedUser.GetGenders()},{"work", SelectedUser.GetWorks()}, {"food", SelectedUser.GetFoods()}, {"hobby", SelectedUser.GetHobbies()}};

      //   return View["user.cshtml"];
      // };

      Get["/users/{id}"] = parameters => {
        User SelectedUser = User.Find(parameters.id);
        Location userLocation = SelectedUser.GetLocation();
        Dictionary<string, object> Model = new Dictionary<string, object>
        {
          {"user", SelectedUser},
          {"nearby-users", User.FilterCurrentUser(SelectedUser.userId, Location.FindNearbyUsers(userLocation.locationId))},
          {"food-users", User.FilterCurrentUser(SelectedUser.userId, User.FindByFood(SelectedUser.GetFoods()[0], User.GetAll()))},
          {"work-users", User.FilterCurrentUser(SelectedUser.userId, User.FindByWork(SelectedUser.GetWorks()[0], User.GetAll()))},
          {"hottest-users", User.FilterCurrentUser(SelectedUser.userId, User.FindByMinRating(3))}
        };

        return View["user.cshtml", Model];
      };

      Get["/users/{currentId}/details/{id}"] = parameters => {
        Dictionary<string, object> Model = new Dictionary<string, object>
        {
          {"current-user", User.Find(parameters.currentId)},
          {"details-user", User.Find(parameters.id)}
        };
        return View["user-detail.cshtml", Model];
      };


      // Post["/users/new"] = _ => {
      //   Avatar testAvatar = new Avatar (Request.Form["value"]);
      //   testAvatar.Save();
      //   testAvatar.Display();
      //   testAvatar.DeleteJpg();
      //   return View["user_profile.cshtml",testAvatar];
      // };

       /*Get["/users/new"] = _ => {
         return View["users_form.cshtml"];
       };
       Post["/users/new"] = _ => {
         User user1 = new User(Request.Form["user-name"], Request.Form["user-description"]);
         user1.Save();
         return View["users.cshtml", user1];
       };*/


      /* Get["/users/detail/{id}"] = parameters =>
       {
           User user = User.Find(parameters.id);
           return View["user-detail.cshtml", user];
       };*/


       /*Post["/users/delete"] = _ => {
           User.DeleteAll();
           return View["index.cshtml"];
       };*/

      //  Dictionary<string, object> model = ModelMaker();
      //   model.Add("recipe", newRecipe);
    }
    // public static Dictionary<string, object> ModelMaker()
    // {
    //  Dictionary<string, object> model = new Dictionary<string, object>
    //  {
    //    {"recipes", Recipe.GetAll()},
    //    {"categories", Category.GetAll()},
    //    {"ingredients", Ingredient.GetAll()}
    //  };
    //  return model;
   }
}
