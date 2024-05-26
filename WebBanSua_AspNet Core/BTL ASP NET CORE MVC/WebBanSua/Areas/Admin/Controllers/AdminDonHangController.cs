using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebBanSua.Models;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Collections.Generic;
namespace WebBanSua.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminDonHangController : Controller
    {
        private readonly CuaHangBanSuaContext _context;
        //
        //

        public AdminDonHangController(CuaHangBanSuaContext context)
        {
            _context = context;



        }
        public IActionResult Index()
        {

            // Lấy danh sách đơn hàng từ cơ sở dữ liệu và kết hợp thông tin khách hàng và chi tiết đơn hàng
            var danhSachDonHang = _context.DonHangs
                .Include(dh => dh.MaKhNavigation) // Kết hợp bảng KhachHang
                .Include(dh => dh.ChiTietDonHangs) // Kết hợp các chi tiết đơn hàng
                    .ThenInclude(cd => cd.MaSpNavigation).Where(cd => cd.TrangThaiHuyDon == 1) // Kết hợp bảng SanPham trong chi tiết đơn hàng
                .ToList();

            // Trả về view và chuyển danh sách đơn hàng cho view đó để hiển thị
            return View(danhSachDonHang);



        }
        public IActionResult DonHangXacNhan()
        {

            // Lấy danh sách đơn hàng từ cơ sở dữ liệu và kết hợp thông tin khách hàng và chi tiết đơn hàng
            var DonHangXacNhan = _context.DonHangs
                .Include(dh => dh.MaKhNavigation) // Kết hợp bảng KhachHang
                .Include(dh => dh.ChiTietDonHangs) // Kết hợp các chi tiết đơn hàng
                    .ThenInclude(cd => cd.MaSpNavigation).Where(cd => cd.TrangThaiHuyDon == 2) // Kết hợp bảng SanPham trong chi tiết đơn hàng
                .ToList();

            // Trả về view và chuyển danh sách đơn hàng cho view đó để hiển thị
            return View(DonHangXacNhan);



        }
        public IActionResult DonHangHuy()
        {

            // Lấy danh sách đơn hàng từ cơ sở dữ liệu và kết hợp thông tin khách hàng và chi tiết đơn hàng
            var DonHangHuy = _context.DonHangs
                .Include(dh => dh.MaKhNavigation) // Kết hợp bảng KhachHang
                .Include(dh => dh.ChiTietDonHangs) // Kết hợp các chi tiết đơn hàng
                    .ThenInclude(cd => cd.MaSpNavigation).Where(cd => cd.TrangThaiHuyDon == 0) // Kết hợp bảng SanPham trong chi tiết đơn hàng
                .ToList();
            // Trả về view và chuyển danh sách đơn hàng cho view đó để hiển thị
            return View(DonHangHuy);



        }
        public IActionResult ThanhToan()
        {

            // Lấy danh sách đơn hàng từ cơ sở dữ liệu và kết hợp thông tin khách hàng và chi tiết đơn hàng
            var ThanhToan = _context.DonHangs
                .Include(dh => dh.MaKhNavigation) // Kết hợp bảng KhachHang
                .Include(dh => dh.ChiTietDonHangs) // Kết hợp các chi tiết đơn hàng
                    .ThenInclude(cd => cd.MaSpNavigation).Where(cd => cd.TrangThaiHuyDon == 3) // Kết hợp bảng SanPham trong chi tiết đơn hàng
                .ToList();
            // Trả về view và chuyển danh sách đơn hàng cho view đó để hiển thị
            return View(ThanhToan);



        }
        [HttpPost]
        public async Task<IActionResult> ConfirmCancellation(int id)
        {
            // Tìm đơn hàng trong cơ sở dữ liệu dựa trên id
            var donHang = await _context.DonHangs.FindAsync(id);

            if (donHang == null)
            {
                return NotFound();
            }

            // Thực hiện thay đổi trạng thái hủy đơn
            donHang.TrangThaiHuyDon = 2; // Giả sử 2 là trạng thái hủy đơn hàng

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.DonHangs.Update(donHang);
            await _context.SaveChangesAsync();

            // Redirect về trang Index hoặc trang nào đó sau khi thực hiện xác nhận hủy đơn hàng thành công
            return RedirectToAction("DonHangXacNhan");
        }

        [HttpPost]
        public async Task<IActionResult> HuyDonHang(int id)
        {
            // Tìm đơn hàng trong cơ sở dữ liệu dựa trên id
            var donHangHuy = await _context.DonHangs.FindAsync(id);

            if (donHangHuy == null)
            {
                return NotFound();
            }

            // Thực hiện thay đổi trạng thái hủy đơn
            donHangHuy.TrangThaiHuyDon = 0; // Giả sử 2 là trạng thái hủy đơn hàng

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.DonHangs.Update(donHangHuy);
            await _context.SaveChangesAsync();

            // Redirect về trang Index hoặc trang nào đó sau khi thực hiện xác nhận hủy đơn hàng thành công
            return RedirectToAction("DonHangHuy");
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToan(int id)
        {
            // Tìm đơn hàng trong cơ sở dữ liệu dựa trên id
            var thanhToan = await _context.DonHangs.FindAsync(id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            // Thực hiện thay đổi trạng thái hủy đơn
            thanhToan.TrangThaiHuyDon = 3;
            thanhToan.ThanhToan = true;
            thanhToan.NgayThanhToan = DateTime.Now;
            // Lưu thay đổi vào cơ sở dữ liệu
            _context.DonHangs.Update(thanhToan);
            await _context.SaveChangesAsync();

            // Redirect về trang Index hoặc trang nào đó sau khi thực hiện xác nhận hủy đơn hàng thành công
            return RedirectToAction("ThanhToan");
        }

        [HttpPost]
        public async Task<IActionResult> XoaDonHang(int maDonHang)
        {
            // Tìm đơn hàng trong bảng DonHang dựa trên mã đơn hàng
            var donHang = await _context.DonHangs.FindAsync(maDonHang);

            if (donHang == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy đơn hàng
            }

            // Xóa các chi tiết đơn hàng liên quan trong bảng ChiTietDonHang
            var chiTietDonHangs = await _context.ChiTietDonHangs
                .Where(ct => ct.MaDh == maDonHang)
                .ToListAsync();

            // Xóa các chi tiết đơn hàng
            _context.ChiTietDonHangs.RemoveRange(chiTietDonHangs);

            // Xóa đơn hàng từ bảng DonHang
            _context.DonHangs.Remove(donHang);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Redirect hoặc trả về kết quả phù hợp
            return RedirectToAction("DonHangHuy"); // Ví dụ: chuyển hướng đến trang Index của đơn hàng sau khi xóa thành công
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.MaKhNavigation)
                .FirstOrDefaultAsync(m => m.MaDh == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        //public async Task<IActionResult> XuatHoaDon(int id)
        //{

        //    var donHang = await _context.DonHangs
        //                            .Include(dh => dh.ChiTietDonHangs)
        //                                .ThenInclude(ct => ct.MaSpNavigation)
        //                            .FirstOrDefaultAsync(dh => dh.MaDh == id);

        //    if (donHang == null)
        //    {
        //        return NotFound();
        //    }

        //    PdfDocument document = new PdfDocument();
        //    PdfPage page = document.AddPage();
        //    XGraphics gfx = XGraphics.FromPdfPage(page);
        //    XFont font = new XFont("Arial", 12, XFontStyle.Regular);

        //    string hoaDonString = $"HÓA ĐƠN - Mã đơn hàng: {donHang.MaDh}";
        //    string ngayDatString = $"Ngày đặt: {donHang.NgayTao}";

        //    XSize hoaDonSize = gfx.MeasureString(hoaDonString, font);
        //    XSize ngayDatSize = gfx.MeasureString(ngayDatString, font);

        //    double ngayDatStartX = 50 + hoaDonSize.Width + 50;

        //    gfx.DrawString(hoaDonString, font, XBrushes.Black, new XPoint(50, 80));
        //    gfx.DrawString(ngayDatString, font, XBrushes.Black, new XPoint(ngayDatStartX, 80));

        //    gfx.DrawString($"Cửa Hàng Đồ Uống PozaaTea", font, XBrushes.Black, new XPoint(50, 50));

        //    int tableTopY = 120;
        //    int tableLeftX = 50;
        //    int columnWidthSTT = 35;
        //    int columnWidthTenSP = 250;
        //    int columnWidthSoluong = 60;
        //    int columnWidthDonGia = 100;
        //    int columnWidthThanhTien = 100;
        //    int cellPadding = 5;
        //    int tableWidth = columnWidthSTT + columnWidthTenSP + columnWidthSoluong + columnWidthDonGia + columnWidthThanhTien + 5 * cellPadding;
        //    int tableHeight = donHang.ChiTietDonHangs.Count * 20 + 60;

        //    if (tableLeftX + tableWidth > XUnit.FromCentimeter(21).Point || tableTopY + tableHeight > XUnit.FromCentimeter(29.7).Point)
        //    {
        //        // handle if table exceeds page size
        //    }

        //    gfx.DrawString("STT", font, XBrushes.Black, new XPoint(tableLeftX + cellPadding, tableTopY + cellPadding));
        //    gfx.DrawString("Tên Sản Phẩm", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + cellPadding, tableTopY + cellPadding));
        //    gfx.DrawString("Số Lượng", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + cellPadding, tableTopY + cellPadding));
        //    gfx.DrawString("Đơn Giá", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + cellPadding, tableTopY + cellPadding));
        //    gfx.DrawString("Thành Tiền", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + columnWidthDonGia + cellPadding, tableTopY + cellPadding));

        //    int rowY = tableTopY + 2 * cellPadding + 20;
        //    int stt = 1;
        //    foreach (var chiTiet in donHang.ChiTietDonHangs)
        //    {
        //        gfx.DrawString(stt.ToString(), font, XBrushes.Black, new XPoint(tableLeftX + cellPadding, rowY));
        //        gfx.DrawString(chiTiet.MaSpNavigation.TenSp, font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + cellPadding, rowY));
        //        gfx.DrawString(chiTiet.SoLuong.ToString(), font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + cellPadding, rowY));
        //        gfx.DrawString(chiTiet.MaSpNavigation.GiaSp.ToString(), font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + cellPadding, rowY));
        //        gfx.DrawString((chiTiet.SoLuong * chiTiet.MaSpNavigation.GiaSp).ToString(), font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + columnWidthDonGia + cellPadding, rowY));
        //        stt++;
        //        rowY += (int)font.GetHeight();
        //    }

        //    int khoangCachTuBang = 20;
        //    int toaDoY = tableTopY + donHang.ChiTietDonHangs.Count * 20 + 60 + khoangCachTuBang;
        //    gfx.DrawString($"TỔNG TIỀN: {donHang.TongTien}", font, XBrushes.Black, new XPoint(50, toaDoY));
        //    //gfx.DrawString($"Tên Khách Hàng: {donHang.MaKhNavigation.MaKh.ToString()} ", font, XBrushes.Black, new XPoint(70, toaDoY));

        //    MemoryStream stream = new MemoryStream();
        //    document.Save(stream, false);
        //    stream.Position = 0;

        //    return File(stream, "application/pdf", $"HoaDon_{donHang.MaDh}.pdf");
        //}

        public async Task<IActionResult> XuatHoaDon(int id)
        {
            var query = from dh in _context.DonHangs
                        where dh.MaDh == id
                        select new
                        {
                            dh,
                            Address = dh.MaKhNavigation.Diachi,
                            CustomerName = dh.MaKhNavigation.TenKh,
                            ChiTietDonHangs = dh.ChiTietDonHangs.Select(ct => new
                            {
                                ct,
                                ProductName = ct.MaSpNavigation.TenSp,
                                ProductPrice = ct.MaSpNavigation.GiaSp
                            }).ToList()
                        };

            var result = await query.FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            var donHang = result.dh;
            var tenKhachHang = result.CustomerName;
            var diachi = result.Address;
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Times New Roman", 12, XFontStyle.Regular);

            string hoaDonString = $"HÓA ĐƠN - Mã đơn hàng: {donHang.MaDh}";
            string ngayDatString = $"Ngày đặt: {donHang.NgayTao}";
            string tenKhachHangString = $"Tên Khách Hàng: {tenKhachHang}";
            string diaChiString = $"Địa chỉ: {diachi}";

            XSize hoaDonSize = gfx.MeasureString(hoaDonString, font);
            XSize ngayDatSize = gfx.MeasureString(ngayDatString, font);

            double ngayDatStartX = 50 + hoaDonSize.Width + 50;

            gfx.DrawString(hoaDonString, font, XBrushes.Black, new XPoint(50, 80));
            gfx.DrawString(ngayDatString, font, XBrushes.Black, new XPoint(ngayDatStartX, 80));
            gfx.DrawString($"Cửa Hàng Đồ Uống PozaaTea", font, XBrushes.Black, new XPoint(50, 50));

            int tableTopY = 120;
            int tableLeftX = 50;
            int columnWidthSTT = 35;
            int columnWidthTenSP = 250;
            int columnWidthSoluong = 60;
            int columnWidthDonGia = 100;
            int columnWidthThanhTien = 100;
            int cellPadding = 5;
            int tableWidth = columnWidthSTT + columnWidthTenSP + columnWidthSoluong + columnWidthDonGia + columnWidthThanhTien + 5 * cellPadding;
            int tableHeight = donHang.ChiTietDonHangs.Count * 20 + 60;

            if (tableLeftX + tableWidth > XUnit.FromCentimeter(21).Point || tableTopY + tableHeight > XUnit.FromCentimeter(29.7).Point)
            {
                // handle if table exceeds page size
            }

            gfx.DrawString("STT", font, XBrushes.Black, new XPoint(tableLeftX + cellPadding, tableTopY + cellPadding));
            gfx.DrawString("Tên Sản Phẩm", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + cellPadding, tableTopY + cellPadding));
            gfx.DrawString("Số Lượng", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + cellPadding, tableTopY + cellPadding));
            gfx.DrawString("Đơn Giá", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + cellPadding, tableTopY + cellPadding));
            gfx.DrawString("Thành Tiền", font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + columnWidthDonGia + cellPadding, tableTopY + cellPadding));

            int rowY = tableTopY + 2 * cellPadding + 20;
            int stt = 1;
            foreach (var chiTiet in result.ChiTietDonHangs)
            {
                gfx.DrawString(stt.ToString(), font, XBrushes.Black, new XPoint(tableLeftX + cellPadding, rowY));
                gfx.DrawString(chiTiet.ProductName, font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + cellPadding, rowY));
                gfx.DrawString(chiTiet.ct.SoLuong.ToString(), font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + cellPadding, rowY));
                gfx.DrawString(chiTiet.ProductPrice.ToString(), font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + cellPadding, rowY));
                gfx.DrawString((chiTiet.ct.SoLuong * chiTiet.ProductPrice).ToString(), font, XBrushes.Black, new XPoint(tableLeftX + columnWidthSTT + columnWidthTenSP + columnWidthSoluong + columnWidthDonGia + cellPadding, rowY));
                stt++;
                rowY += (int)font.GetHeight();
            }

            int khoangCachTuBang = 20;
            int toaDoY = tableTopY + result.ChiTietDonHangs.Count * 20 + 60 + khoangCachTuBang;
            gfx.DrawString($"TỔNG TIỀN: {donHang.TongTien}", font, XBrushes.Black, new XPoint(50, toaDoY));
            gfx.DrawString(tenKhachHangString, font, XBrushes.Black, new XPoint(50, toaDoY + 20)); // Draw the customer's name
            gfx.DrawString(diaChiString, font, XBrushes.Black, new XPoint(50, toaDoY + 40)); // Draw the customer's name

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            stream.Position = 0;

            return File(stream, "application/pdf", $"HoaDon_{donHang.MaDh}.pdf");
        }


    }
}
