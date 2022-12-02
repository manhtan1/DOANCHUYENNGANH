using phim2101.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Configuration;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using System.Threading.Tasks;
using phim2101.Others;
using System.IO;

namespace phim2101.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Admin/Home

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Dangnhap");
            }
            else
            {
                return View();
            }
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string user, string pw)
        {
            var nhanvien = db.NhanViens.SingleOrDefault(m => m.UserName.ToLower() == user.ToLower() && m.Password == pw);
            if (nhanvien != null)
            {
                Session["user"] = nhanvien;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View();
            }
            

        }
        public ActionResult Dangxuat()
        {
            Session["user"] = null;
            return Redirect("/admin/home/dangnhap");
        }
        public ActionResult ngoaithoigian()
        {
            return View();
        }
    }
}