using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanSua.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
          
        }

        public int MaDh { get; set; }
        public int MaKh { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThaiHuyDon { get; set; }
        public bool ThanhToan { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string Note { get; set; }
        public int TongTien { get; set; }

        public virtual KhachHang MaKhNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
