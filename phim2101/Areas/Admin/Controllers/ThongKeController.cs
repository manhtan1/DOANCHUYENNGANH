using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phim2101.Models;
using System.Data.Entity;
using PagedList;


namespace phim2101.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        private DBContext db = new DBContext();
        public ActionResult Index(int ? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var cthd = db.ChiTietHDs.Include(n => n.Hoadon).Include(n => n.Ve).ToList();

            DateTime currentTime = DateTime.Now;
            DateTime star1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 7, 0, 0);
            DateTime end1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 0, 0);
            DateTime start2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 15, 0);
            DateTime end2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 21, 15, 0);
            NhanVien nv = (NhanVien)Session["user"];
            var count = db.PhanQuyens.Count(m => m.MaNV == nv.MaNV && m.IDChucnang == 6);
            if (count == 0)
            {
                return Redirect("/Admin/Baoloi/khongcapquyen");
            }
            else if (nv.Ca == "Ca1")
            {
                if (currentTime < end1 && currentTime > star1)
                {
                    return View(cthd.ToPagedList(pageNum, pageSize));
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
                    return View(cthd.ToPagedList(pageNum, pageSize));
                }
                else
                {
                    return Redirect("/admin/home/ngoaithoigian");
                }
            }
            return View();
        }
        public double doanhthutheophim(int idmave,int idhoadon)
        {
            double tongtien = 0;
            var phim = db.Phims.ToList();
            var cthd = db.ChiTietHDs.Include(n => n.Hoadon).Include(n => n.Ve).ToList();
            foreach(var i in cthd)
            {
                foreach(var j in phim)
                {
                    if (i.Ve.MaPhim == j.MaPhim && i.Status==true)
                    {
                        tongtien += (double)i.Hoadon.TongTien;
                    }
                }
            }
            return tongtien;
        }
        public double monthly(int idmave, int idhoadon)
        {
            double tongtien = 0;
            var phim = db.Phims.ToList();
            var cthd = db.ChiTietHDs.Include(n => n.Hoadon).Include(n => n.Ve).ToList();
            ChiTietHD hd = db.ChiTietHDs.SingleOrDefault(n => n.MaVe == idmave && n.MaHD == idhoadon);
            foreach (var i in cthd)
            {
                foreach (var j in phim)
                {
                    if (i.Ve.MaPhim == j.MaPhim && i.Status == true)
                    {
                        if (i.NgayBanVe.Value.Month.ToString() == DateTime.Now.Month.ToString())
                        {
                            tongtien += (double)i.Hoadon.TongTien;
                        }
                    }
                }
            }
            return tongtien;
        }
        public double annual(int idmave, int idhoadon)
        {
            double tongtien = 0;
            var phim = db.Phims.ToList();
            var cthd = db.ChiTietHDs.Include(n => n.Hoadon).Include(n => n.Ve).ToList();
            ChiTietHD hd = db.ChiTietHDs.SingleOrDefault(n => n.MaVe == idmave && n.MaHD == idhoadon);
            foreach (var i in cthd)
            {
                foreach (var j in phim)
                {
                    if (i.Ve.MaPhim == j.MaPhim && i.Status == true)
                    {
                        if (i.NgayBanVe.Value.Year.ToString() == DateTime.Now.Year.ToString())
                        {
                            tongtien += (double)i.Hoadon.TongTien;
                        }
                    }
                }
            }
            return tongtien;
        }
        public  int pendingrequests()
        {
            int moi = 0;
            var hd = db.ChiTietHDs.ToList();
            foreach (var item in hd)
            {
                if (item.Status == false)
                {
                    moi++;
                }
            }
            return moi;
        }
        public ActionResult Loc(string start,string end)
        {
            DateTime bd = DateTime.Parse(start.ToString());
            DateTime kt = DateTime.Parse(end.ToString());
            var cthd = db.ChiTietHDs.Include(n => n.Hoadon).Include(n => n.Ve).Where(n=>n.NgayBanVe>bd && n.NgayBanVe< kt).ToList();
            return View(cthd);
        }
    }
}