namespace phim2101.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietHD")]
    public partial class ChiTietHD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaVe { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHD { get; set; }

        public int MaDV { get; set; }

        public int MaUD { get; set; }

        [StringLength(10)]
        public string Ghe { get; set; }

        public DateTime? NgayBanVe { get; set; }

        public int? SoLuong { get; set; }

        public virtual DichVu DichVu { get; set; }

        public virtual Hoadon Hoadon { get; set; }

        public virtual UuDai UuDai { get; set; }

        public virtual Ve Ve { get; set; }
    }
}
