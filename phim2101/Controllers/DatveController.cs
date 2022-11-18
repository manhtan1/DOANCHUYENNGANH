using phim2101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phim2101.Controllers
{
    public class DatveController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult TrangDatVe(int id)
        {
            /*List<GioHangItem> lstgiohang = Laygiohang();
            GioHangItem product = lstgiohang.Find(n => n.MaPhim == id);*/
            Phim phim = db.Phims.Find(id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Home");
            }
            return View(phim);
        }
        public List<GioHangItem> Laygiohang()
        {
            List<GioHangItem> lstgiohang = Session["GioHang"] as List<GioHangItem>;
            if (lstgiohang == null)
            {
                lstgiohang = new List<GioHangItem>();
                Session["GioHang"] = lstgiohang;
            }
            return lstgiohang;
        }
        public ActionResult Themgiohang(int MaPhim, string ghe, string strURL)
        {
            List<GioHangItem> lstgiohang = Laygiohang();
            GioHangItem product = lstgiohang.Find(n => n.MaPhim == MaPhim);
            var site = ghe;
            if (product == null)
            {
                product = new GioHangItem(MaPhim);
                product.Ghe = site;
                lstgiohang.Add(product);
                return Redirect(strURL);
            }
            else
            {
                product.Ghe = product.Ghe + ", " + site;
                product.SoLuong += 1;
                lstgiohang.Add(product);
                return Redirect(strURL);
            }
        }
        public ActionResult Datve(int ? id)
        {
            List<GioHangItem> lstgiohang = Session["GioHang"] as List<GioHangItem>;
            GioHangItem gh = lstgiohang.Find(n => n.MaPhim == id);
            return View(gh);
        }
        [HttpPost]
        public ActionResult Datve(int id)
        {
            
            
            //db.SaveChanges();
            return RedirectToAction("index", "home");
        }
        public ActionResult datvee(int? id)
        {
            List<GioHangItem> lstgiohang = Session["GioHang"] as List<GioHangItem>;
            GioHangItem gh = lstgiohang.Find(n => n.MaPhim == id);
            return View(gh);
        }
        [HttpPost]
        public ActionResult datvee(int id)
        {
            Random rnd = new Random();
            int mave = rnd.Next(1, 1000);
            int mahd = rnd.Next(1, 1000);
            List<GioHangItem> lstgiohang = Session["GioHang"] as List<GioHangItem>;
            var session = (KhachHang)Session["TaiKhoan"];
            GioHangItem gh = lstgiohang.Find(n => n.MaPhim == id);
            Ve ve = new Ve();
            ve.MaPhim = gh.MaPhim;
            ve.MaPhong = gh.MaPhong;
            db.Ves.Add(ve);
            Hoadon hd = new Hoadon();
            hd.MaKH = session.MaKH;
            hd.TongTien = gh.dthanhtien;
            hd.MaNV = 1;
            db.Hoadons.Add(hd);

            ChiTietHD cthd = new ChiTietHD();

            cthd.MaHD = hd.MaHD;
            cthd.MaVe = ve.MaVe;
            cthd.MaUD = 1;
            cthd.MaDV = 1;
            cthd.Ghe = gh.Ghe;
            cthd.SoLuong = gh.SoLuong;
            cthd.NgayBanVe = DateTime.Now;
            db.ChiTietHDs.Add(cthd);
            /*foreach (var v in db.Ves.ToList())
            {
                foreach (var h in db.Hoadons.ToList())
                {
                    
                    if (v.MaVe == mave)
                    {
                        mave = rnd.Next(1, 1000);
                    }
                    else if (h.MaHD == mahd)
                    {
                        mahd = rnd.Next(1, 1000);
                    }
                    else
                    {
                        Ve ve = new Ve();
                        ve.MaVe = mave;
                        ve.MaPhim = gh.MaPhim;
                        ve.MaPhong = gh.MaPhong;
                        db.Ves.Add(ve);
                        db.SaveChanges();
                        Hoadon hd = new Hoadon();
                        hd.MaHD = mahd;
                        hd.MaKH = session.MaKH;
                        hd.TongTien = gh.dthanhtien;
                        hd.MaNV = 1;
                        db.Hoadons.Add(hd);
                        db.SaveChanges();
                        ChiTietHD cthd = new ChiTietHD();

                        cthd.MaHD = mahd;
                        cthd.MaVe = mave;
                        cthd.MaUD = 1;
                        cthd.MaDV = 1;
                        cthd.Ghe = gh.Ghe;
                        cthd.SoLuong = gh.SoLuong;
                        cthd.NgayBanVe = DateTime.Now;
                        db.ChiTietHDs.Add(cthd);
                    }
                }
            }
*/
            db.SaveChanges();
            return RedirectToAction("index", "home");
        }
    }
}