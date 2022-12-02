using phim2101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phim2101.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Admin/Home

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string user, string pw)
        {
            var nhanvien = db.NhanViens.SingleOrDefault(m => m.UserName.ToLower() == user.ToLower() && m.Password == pw);
            if (nhanvien !=null)
            {
                Session["user"] = user;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View();
            }
            

        }
    }
}