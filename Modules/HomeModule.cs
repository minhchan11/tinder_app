using Nancy;
using System;
using System.Collections.Generic;

namespace TinderApp
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
          return View["index.cshtml"];
      };

      Get["/users/new"] = _ => View["users_form.cshtml"];

      Post["/users/new/added"] = _ => {
        User newUser = new User(Request.Form["user-name"], Request.Form["user-description"]);
        newUser.Save();
        Avatar testAvatar = new Avatar (Request.Form["value"]);
        testAvatar.Save();
        testAvatar.Display();
        testAvatar.DeleteJpg();
        Location newLocation = new Location(Request.Form["location"]);
        newLocation.Save();
        newLocation.AddUserToLocation(newUser.userId);
        newUser.AddAvatarToUser(testAvatar);
        newUser.AddGender(Request.Form["user-gender"]);
        newUser.AddWork(Request.Form["user-work"]);
        newUser.AddFood(Request.Form["user-food"]);
        newUser.AddHobby(Request.Form["user-hobby"]);
        Dictionary<string, object> Model = new Dictionary<string, object>{{"user", newUser.userId},{"name", newUser.name},{"description", newUser.description}, {"gender", Request.Form["user-gender"]},{"work", Request.Form["user-work"]}, {"food", Request.Form["user-food"]}, {"hobby", Request.Form["user-hobby"]}, {"avatar", testAvatar}};
        return View["user_profile.cshtml", Model];
      };

      Get["/loggedin/{id}"] = parameters => {
        User newUser = User.Find(parameters.id);
        return View["index_loggedin.cshtml", newUser];
      };

      Get["/users/profile/{id}"] = parameters => {
        User SelectedUser = User.Find(parameters.id);
        Dictionary<string, object> Model = new Dictionary<string, object>{{"user", SelectedUser.userId},{"name", SelectedUser.name},{"description", SelectedUser.description}, {"gender", SelectedUser.GetGenders()},{"work", SelectedUser.GetWorks()}, {"food", SelectedUser.GetFoods()}, {"hobby", SelectedUser.GetHobbies()}};
        Model.Add("avatar", SelectedUser.GetAvatar());
        return View["user_profile.cshtml", Model];
      };

      Get["/users"] = _ => {
        Dictionary<string, object> Model = new Dictionary<string, object>{};

        Model.Add("userlist", User.GetAll());
        Model.Add("allgenders", User.GetAllGenders());
        Model.Add("allworks", User.GetAllWorks());
        Model.Add("allfood", User.GetAllFoods());
        Model.Add("allhobbies", User.GetAllHobbies());
        return View["users.cshtml", Model];
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
        return View["users.cshtml", User.GetAll()];
      };

      Get["/users/{id}"] = parameters => {
        var SelectedUser = User.Find(parameters.id);
        var UserUsers = SelectedUser.name;
        Dictionary<string, object> Model = new Dictionary<string, object>{{"user", SelectedUser.userId},{"name", SelectedUser.name},{"description", SelectedUser.description}, {"gender", SelectedUser.GetGenders()},{"work", SelectedUser.GetWorks()}, {"food", SelectedUser.GetFoods()}, {"hobby", SelectedUser.GetHobbies()}};

        return View["user.cshtml", Model];
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
