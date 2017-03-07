
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
      Post["/"] = _ => {
        Avatar testAvatar = new Avatar (Request.Form["value"]);
        testAvatar.Save();
        testAvatar.Display();
        return View["user_profile.cshtml",testAvatar];
      };


    // }
    // public static Dictionary<string, object> ModelMaker()
    // {
    //   Dictionary<string, object> model = new Dictionary<string, object>
    //   {
    //     {"Bands", Band.GetAll()},
    //     {"Venues", Venue.GetAll()},
    //   };
    //   return model;
    // }
    }
  }
}
