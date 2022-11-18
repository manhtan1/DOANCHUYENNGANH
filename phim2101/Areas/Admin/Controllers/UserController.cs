
using phim2101.Areas.Admin.Code;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phim2101.Areas.Admin.Controllers
{
    internal class UserController : UserSessions
    {
        public string emailAdmin { get; set; }
        public string password { get; set; }
    }
}