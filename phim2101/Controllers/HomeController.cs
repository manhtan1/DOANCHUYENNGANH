using phim2101.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Configuration;
using PagedList;
using System.Web.Mvc;
using phim2101.Others;
using log4net;

namespace phim2101.Controllers
{
    public class HomeController : Controller
    {

        private static readonly ILog log =
  LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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
        public ActionResult Dangxuat()
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            if (Session["TaiKhoan"] != null)
            {
                Session["TaiKhoan"] = null;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("DangNhap", "Home");
            }
        }
        public ActionResult ThongTinTK()
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                return View();

            }
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
        
        public ActionResult TopView()
        {

            return View(db.Phims.OrderByDescending(n=>n.luotxem).Take(5).ToList());
        }
        [HttpPost]
        public ActionResult FollowFilm1(int id)
        {
            Phim phim = db.Phims.Find(id);
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
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
            }

        }
        [HttpPost]
        public ActionResult UnFollowFilm1(int MaPhim, string strURL)
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            var fl = db.Phim_Theo_Doi.Where(n => n.MaKH == kh.MaKH && n.MaPhim == MaPhim).ToList();
            foreach(var i in fl)
            {
                db.Phim_Theo_Doi.Remove(i);
                db.SaveChanges();
            }
            return Redirect(strURL);
        }
        public ActionResult Payment(int id)
        {
            var session = (KhachHang)Session["TaiKhoan"];
            List<GioHangItem> lstgiohang = Session["GioHang"] as List<GioHangItem>;
            GioHangItem gh = lstgiohang.Find(n => n.MaPhim == id);
            Phim phim = db.Phims.Find(id);
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", tmnCode);
            vnpay.AddRequestData("vnp_Amount", (gh.dthanhtien * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", "");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang: ve xem phim " + phim.TenPhim);
            vnpay.AddRequestData("vnp_OrderType", "190001"); //default value: other  190001 la ma loai ve xem phim 
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            //Billing
            vnpay.AddRequestData("vnp_Bill_Mobile", "0355738400");
            vnpay.AddRequestData("vnp_Bill_Email", "nguyenthu753951@gmail.com");
            var fullName = "Nguyen Thi Thu";
            if (!String.IsNullOrEmpty(fullName))
            {
                var indexof = fullName.IndexOf(' ');
                vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
            }
            vnpay.AddRequestData("vnp_Bill_Address", "325 HT");
            vnpay.AddRequestData("vnp_Bill_City", "ABC");
            vnpay.AddRequestData("vnp_Bill_Country", "VN");
            vnpay.AddRequestData("vnp_Bill_State", "");

            // Invoice

            vnpay.AddRequestData("vnp_Inv_Phone", "0355738400");
            vnpay.AddRequestData("vnp_Inv_Email", "nguyenthu753951@gmail.com");
            vnpay.AddRequestData("vnp_Inv_Customer", "ABC");
            vnpay.AddRequestData("vnp_Inv_Address", "ABC");
            vnpay.AddRequestData("vnp_Inv_Company", "ABC");
            vnpay.AddRequestData("vnp_Inv_Taxcode", "ABC");
            vnpay.AddRequestData("vnp_Inv_Type", "ABC");

            string paymentUrl = vnpay.CreateRequestUrl(url, hashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            Response.Redirect(paymentUrl);
            return Redirect(paymentUrl);
        }

        [HttpGet]
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
        
        public ActionResult Huyve(int mave,int idhoadon)
        {
            var session = (KhachHang)Session["TaiKhoan"];
            Hoadon hd = db.Hoadons.Find(idhoadon);
            Ve v = db.Ves.Find(idhoadon);
            ChiTietHD cthd = db.ChiTietHDs.SingleOrDefault(n => n.MaVe == mave && n.MaHD == idhoadon);
            db.ChiTietHDs.Remove(cthd);
            db.SaveChanges();
            db.Hoadons.Remove(hd);
            db.Ves.Remove(v);
            db.SaveChanges();
            return RedirectToAction("index", "Home");
        }
    }
}