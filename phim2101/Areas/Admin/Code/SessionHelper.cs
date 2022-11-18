using phim2101.Areas.Admin.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phim2101.Areas.Admin.Code
{
    public class SessionHelper
    {
        public static void SetSession(UserSessions sessions)
        {
            HttpContext.Current.Session["loginSesion"] = sessions;
        }

        public static UserSessions GetSession()
        {
            var session = HttpContext.Current.Session["loginSesion"];
            if (session == null)
                return null;
            else
            {
                return session as UserSessions;
            }    
        }

    }
}