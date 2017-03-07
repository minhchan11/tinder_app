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
       Get["/users"] = _ => {
           List<User> AllUsers = User.GetAll();
           return View["users.cshtml", AllUsers];
       };
       Get["/users/new"] = _ => {
         return View["users_form.cshtml"];
       };
       Post["/users/new"] = _ => {
         User user1 = new User(Request.Form["user-name"], Request.Form["user-description"]);
         user1.Save();
         return View["users.cshtml", user1];
       };

       Get[""] = _ => {

       };
    }
  }
}
