using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using phim2101.Models;

namespace phim2101.Areas.Admin.Controllers
{
    public class ChiTietHDsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/ChiTietHDs
        public ActionResult Index()
        {
            var chiTietHDs = db.ChiTietHDs.Include(c => c.DichVu).Include(c => c.Hoadon).Include(c => c.UuDai).Include(c => c.Ve);
            DateTime currentTime = DateTime.Now;
            DateTime star1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 7, 0, 0);
            DateTime end1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 0, 0);
            DateTime start2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 15, 0);
            DateTime end2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 21, 15, 0);
            NhanVien nv = (NhanVien)Session["user"];
            var count = db.PhanQuyens.Count(m => m.MaNV == nv.MaNV && m.IDChucnang == 4);
            if (count == 0)
            {
                return Redirect("/Admin/Baoloi/khongcapquyen");
            }
            else if (nv.Ca == "Ca1")
            {
                if (currentTime < end1 && currentTime > star1)
                {
                    return View(chiTietHDs.ToList());

                }
                else
                {
                    return Redirect("/admin/home/ngoaithoigian");
                }
            }
            else if (nv.Ca == "Ca2")
            {
                if (currentTime < end2 && currentTime > start2)
                {
                    return View(chiTietHDs.ToList());

                }
                else
                {
                    return Redirect("/admin/home/ngoaithoigian");
                }
            }
            return View();
        }
        public ActionResult Status(int idhoadon,int idve)
        {
            ChiTietHD cthd = db.ChiTietHDs.SingleOrDefault(n => n.MaVe == idve && n.MaHD == idhoadon);
            if (cthd.Status == true)
            {
                cthd.Status = false;
                db.Entry(cthd).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                cthd.Status = true;
                db.Entry(cthd).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("index", "Chitiethds");
        }
        public ActionResult Delete(int idhoadon, int idve)
        {
            Hoadon hd = db.Hoadons.Find(idhoadon);
            Ve v = db.Ves.Find(idve);
            ChiTietHD cthd = db.ChiTietHDs.SingleOrDefault(n => n.MaVe == idve && n.MaHD == idhoadon);
            db.ChiTietHDs.Remove(cthd);
            db.SaveChanges();
            db.Hoadons.Remove(hd);
            db.Ves.Remove(v);
            db.SaveChanges();
            return RedirectToAction("index", "Chitiethds");
        }
    }
}
