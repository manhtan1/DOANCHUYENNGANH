namespace phim2101.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            CT_CMT = new HashSet<CT_CMT>();
            Hoadons = new HashSet<Hoadon>();
            Phims = new HashSet<Phim>();
        }

        [Key]
        public int MaKH { get; set; }

        [StringLength(250)]
        public string TenKH { get; set; }

        [StringLength(10)]
        public string SDTKH { get; set; }

        [StringLength(250)]
        public string DiaChi { get; set; }

        public DateTime? NgaySinh { get; set; }

        [Required]
        [StringLength(250)]
        public string matkhau { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string taikhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_CMT> CT_CMT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hoadon> Hoadons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Phim> Phims { get; set; }
    }
}
