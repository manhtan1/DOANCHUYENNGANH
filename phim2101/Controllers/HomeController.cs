using phim2101.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;

namespace phim2101.Controllers
{
    public class HomeController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            return View(db.Phims.OrderByDescending(n => n.ThoigianChieu < DateTime.Now).ToList().ToPagedList(pageNum, pageSize));
        }



        [HttpGet]
        public ActionResult phimsapchieu()
        {
            return View(db.Phims.Where(n => n.ThoigianChieu > DateTime.Now).ToList());
        }
        [HttpGet]
        public ActionResult phimdangchieu()
        {
            return View(db.Phims.Where(n => n.ThoigianChieu < DateTime.Now).ToList());
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult dangky(FormCollection collection)
        {
            KhachHang kh = new KhachHang();
            var taikhoan = collection["TenDN"];
            var dienthoai = collection["DienThoai"];
            var hoten = collection["hoten"];
            var email = collection["Email"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            var diachi = collection["DiaChi"];
            //var mk = collection["matkhau"];

            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi2"] = "Bạn chưa nhập Tài khoản!!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Bạn chưa cài mật khẩu !!";
            }
            else if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                if (matkhau != nhaplaimatkhau)
                {
                    ViewData["Loi8"] = "Mật khẩu nhập lại không trùng với mật khẩu !!";
                }
                ViewData["Loi4"] = "Bạn phải nhập lại mật khẩu!!";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Bạn để trống số điện thoại của bạn !!";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi7"] = "Bạn để trống địa chỉ bạn đang ở !!";
            }
            else if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Bạn để trống họ tên";
            }else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Bạn để trống email";
            }
            else
            {
                kh.taikhoan = taikhoan;
                kh.SDTKH = dienthoai;
                kh.TenKH = hoten;
                kh.Email = email;
                kh.matkhau = matkhau;
                kh.DiaChi = diachi;
                db.KhachHangs.Add(kh);
                db.SaveChanges();

                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var mk = collection["MatKhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(mk))
            {
                ViewData["Loi2"] = "Cần nhập mật khẩu";
            }
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.taikhoan == tendn && n.matkhau == mk);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Tài khoản hoặc mật khẩu không chính xác"; ;
                }
            }
            return View();
        }

        public ActionResult ChitietPhim(int id)
        {
            Phim phim = db.Phims.Find(id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            phim.luotxem += 1;
            db.Entry(phim).State = EntityState.Modified;
            db.SaveChanges();
            return View(phim);
        }
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchFilm)
        {
            return RedirectToAction("Search", "Home", new { key = searchFilm });
        }
        [HttpGet]
        public ActionResult timkiemtheloai(int id)
        {

            if (db.Phims.Where(n => n.MaLP == id).ToList() == null)
            {
                return HttpNotFound();
            }
            return View(db.Phims.Where(n => n.MaLP == id).ToList());
        }
        public ActionResult Cmt(int idphim, FormCollection collection)
        {
            var cmt = collection["cmt"];
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Phim> phim = new List<Phim>();
            Phim p = phim.SingleOrDefault(n => n.MaPhim == idphim);



            if (string.IsNullOrEmpty(cmt))
            {
                ViewData["Loi"] = "Bạn chưa nhập Comment";
            }
            CT_CMT cm = new CT_CMT();

            cm.MaPhim = idphim;
            cm.MaKH = kh.MaKH;
            cm.comment = cmt;
            cm.ngaycmt = DateTime.Now;
            db.CT_CMT.Add(cm);
            db.SaveChanges();
            //return RedirectToAction("DetailsMusic", "Home", idbaihat);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult TheoDoiPhim(int id)
        {

            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Phim phim = db.Phims.Find(id);
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else {
                Phim_Theo_Doi fl = new Phim_Theo_Doi();
                fl.MaKH = kh.MaKH;
                fl.MaPhim = phim.MaPhim;
                db.Phim_Theo_Doi.Add(fl);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            
        }
        public ActionResult TopView()
        {

            return View(db.Phims.OrderByDescending(n=>n.luotxem).Take(5).ToList());
        }
        [HttpPost]
        public ActionResult FollowFilm(int id)
        {
            Phim phim = db.Phims.Find(id);
            return RedirectToAction("DangNhap", "Home");
            /*KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Phim phim = db.Phims.Find(id);
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                Phim_Theo_Doi fl = new Phim_Theo_Doi();
                fl.MaKH = kh.MaKH;
                fl.MaPhim = phim.MaPhim;
                db.Phim_Theo_Doi.Add(fl);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }*/

        }

    }
}