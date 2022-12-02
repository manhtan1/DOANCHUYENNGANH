using phim2101.Models;
using System;
using System.Linq;

namespace phim2101.Models
{
    public class GioHangItem
    {
        DBContext db = new DBContext();
        public int MaPhim { get; set; }
        public int MaPhong { get; set; }
        public DateTime SuatChieu { get; set; }
        public string Ghe { get; set; }
        public int SoLuong { get; set; }
        public double dthanhtien
        { 
            get { return SoLuong * 60000; }
        }
        public GioHangItem(int id,int idphong)
        {
            MaPhim = id;
            ChiTietPhong phong = db.ChiTietPhongs.SingleOrDefault(n => n.MaPhim == id && n.MaPhong==idphong);
            MaPhong = phong.MaPhong;
            SuatChieu = (DateTime)phong.SuatChieu;
            Ghe = "";
            SoLuong = 1;

        }
    }
}