using WebBanSua.Models;

namespace WebBanSua.ModelViews
{
    public class CartItem
    {
        public SanPham sanPham { get; set; }
        public int soLuong { get; set; }
        public double TongTien => soLuong * sanPham.GiaSp;
    }
}
