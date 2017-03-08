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


      Post["/users/new"] = _ => {
        Avatar testAvatar = new Avatar (Request.Form["value"]);
        testAvatar.Save();
        testAvatar.Display();
        testAvatar.DeleteJpg();
        return View["user_profile.cshtml",testAvatar];
      };

      Get["/users/new/form"] = _ => View["users_form.cshtml"];

      Post["/users/new"] = _ => {
        User newUser = new User(Request.Form["user-name"], Request.Form["user-description"]);
        newUser.Save();
        Avatar testAvatar = new Avatar (Request.Form["value"]);
        testAvatar.Save();
        testAvatar.Display();
        testAvatar.DeleteJpg();
        Dictionary<string, object> Model = new Dictionary<string, object>{{"name", newUser.name},{"description", newUser.description}, {"gender", Request.Form["user-gender"]},{"work", Request.Form["user-work"]}, {"food", Request.Form["user-food"]}, {"hobby", Request.Form["user-hooby"]}, {"avatar", testAvatar}};
        return View["user.cshtml", Model];
      };

      Get["/users"] = _ => {
         List<User> AllUsers = User.GetAll();
         return View["users.cshtml", AllUsers];
      };

       /*Get["/users/new"] = _ => {
         return View["users_form.cshtml"];
       };
       Post["/users/new"] = _ => {
         User user1 = new User(Request.Form["user-name"], Request.Form["user-description"]);
         user1.Save();
         return View["users.cshtml", user1];
       };

       Get["/users/{id}"] = parameters => {
          Dictionary<string, object> model = new Dictionary<string, object>();
           var SelectedUser = User.Find(parameters.id);
           var UserUsers = SelectedUser.GetUsers();
           List<User> AllUsers = User.GetAll();
           model.Add("venue", SelectedUser);
           model.Add("venueUsers", UserUsers);
           model.Add("allUsers", AllUsers);
           return View["user.cshtml"];
       };

       Get["/users/detail/{id}"] = parameters =>
       {
           User user = User.Find(parameters.id);
           return View["user-detail.cshtml", user];
       };

       Delete["/users/{id}"] = parameters =>
       {
           User targetUser = User.Find(parameters.id);
           targetUser.DeleteUser(parameters.id);
           return View["users.cshtml", User.GetAll()];
       };

       Post["/users/delete"] = _ => {
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
