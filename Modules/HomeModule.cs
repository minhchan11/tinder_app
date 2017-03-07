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
      Post["/user-profile"] = _ => {
        Avatar testAvatar = new Avatar (Request.Form["value"]);
        testAvatar.Save();
        testAvatar.Display();
        testAvatar.DeleteJpg();
        return View["user_profile.cshtml",testAvatar];
      };

      //  Get["/users"] = _ => {
      //      List<User> AllUsers = User.GetAll();
      //      return View["users.cshtml", AllUsers];
      //  };
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
    }
  }
}
