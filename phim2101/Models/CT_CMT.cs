namespace phim2101.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CT_CMT
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaPhim { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaKH { get; set; }

        [Column(TypeName = "ntext")]
        public string comment { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaycmt { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual Phim Phim { get; set; }
    }
}
