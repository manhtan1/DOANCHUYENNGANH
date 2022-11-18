namespace phim2101.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Phim")]
    public partial class Phim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Phim()
        {
            ChiTietPhongs = new HashSet<ChiTietPhong>();
            CT_CMT = new HashSet<CT_CMT>();
            KhachHangs = new HashSet<KhachHang>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaPhim { get; set; }

        public int MaDP { get; set; }

        [StringLength(250)]
        public string TenPhim { get; set; }

        [StringLength(50)]
        public string QuocGia { get; set; }

        public int MaLP { get; set; }

        [Required]
        [StringLength(50)]
        public string traller { get; set; }

        [Required]
        [StringLength(50)]
        public string hinhtraller { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string MoTa { get; set; }

        [StringLength(10)]
        public string thoiluong { get; set; }

        public DateTime? ThoigianChieu { get; set; }

        public DateTime? ThoiGianKetThuc { get; set; }

        public int? luotxem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhong> ChiTietPhongs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_CMT> CT_CMT { get; set; }

        public virtual DinhDangPhim DinhDangPhim { get; set; }

        public virtual TheLoaiPhim TheLoaiPhim { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhachHang> KhachHangs { get; set; }
        public List<Phim> searchByKey(string key)
        {
            DBContext db = new DBContext();
            return db.Phims.SqlQuery("Select * from Phim as p where (p.TenPhim like N'%" + key + "%') ").ToList();

        }
    }
}
