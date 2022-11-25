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
using phim2101.Others;

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
        public ActionResult Payment(int id)
        {
            var session = (KhachHang)Session["TaiKhoan"];
            List<GioHangItem> lstgiohang = Session["GioHang"] as List<GioHangItem>;
            Phim phim = db.Phims.Find(id);
            GioHangItem gh = lstgiohang.Find(n => n.MaPhim == id);
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", gh.dthanhtien.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", session.MaKH.ToString()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang online"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "Thanh toán ghế phim"+phim.ToString()); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

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
    }
}