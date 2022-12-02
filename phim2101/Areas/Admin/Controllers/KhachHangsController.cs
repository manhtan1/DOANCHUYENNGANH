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
    public class KhachHangsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/KhachHangs
        public ActionResult Index()
        {
            var phims = db.Phims.Include(p => p.DinhDangPhim).Include(p => p.TheLoaiPhim);
            DateTime currentTime = DateTime.Now;
            DateTime star1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 7, 0, 0);
            DateTime end1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 0, 0);
            DateTime start2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 15, 0);
            DateTime end2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 21, 15, 0);
            NhanVien nv = (NhanVien)Session["user"];
            var count = db.PhanQuyens.Count(m => m.MaNV == nv.MaNV && m.IDChucnang == 1);
            if (count == 0)
            {
                return Redirect("/Admin/Baoloi/khongcapquyen");
            }
            else if (nv.Ca == "Ca1")
            {
                if (currentTime < end1 && currentTime > star1)
                {
                    return View(db.KhachHangs.ToList());
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
                    return View(db.KhachHangs.ToList());
                }
                else
                {
                    return Redirect("/admin/home/ngoaithoigian");
                }
            }
            return View();
        }

        // GET: Admin/KhachHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,SDTKH,DiaChi,NgaySinh,taikhoan,matkhau,Email")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Edit/5
        public ActionResult Editt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editt([Bind(Include = "MaKH,TenKH,SDTKH,DiaChi,NgaySinh ,taikhoan,matkhau,Emai")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                KhachHang kh = db.KhachHangs.Find(khachHang.MaKH);
                /*khachHang.taikhoan = kh.taikhoan;
                khachHang.matkhau =kh.matkhau;*/
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khachHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
