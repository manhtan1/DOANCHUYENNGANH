using phim2101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RapPhim2101.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private string emailAdmin = "admin@gmail.com";

        private string password = "123456789";
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginAdmin model)
        {
            if (model.email != this.emailAdmin || model.password != this.password)
            {

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(model);
            }

            return RedirectToAction("Index", "Home");


        }
    }
}