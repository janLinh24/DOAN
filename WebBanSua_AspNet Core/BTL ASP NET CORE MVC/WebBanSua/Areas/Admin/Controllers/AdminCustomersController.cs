using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanSua.Models;

namespace WebBanSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCustomersController : Controller
    {
        private readonly CuaHangBanSuaContext _context;

        public AdminCustomersController(CuaHangBanSuaContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            string successMessage = TempData["SuccessMessage"] as string;

            // Truyền thông điệp thành công cho View
            ViewBag.SuccessMessage = successMessage;
            ViewBag.MessageTimeout = 5000; // Thời gian tính bằng mili giây (trong trường hợp này là 5 giây)

            return View(await _context.KhachHangs.ToListAsync());
        }

        public IActionResult ThemKH()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemKH([Bind("MaKh,TenKh,GioiTinh,AvatarKh,Diachi,Ngaysinh,Phone,Email,Password,CreateDate")] KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(khachhang.TenKh) || string.IsNullOrEmpty(khachhang.GioiTinh) || string.IsNullOrEmpty(khachhang.Email) /*|| khachhang.Phone == null || khachhang.Ngaysinh == null*/)
                {
                    ViewBag.Loi1 = "Thông tin không được để trống";
                    return View("ThemKH");
                }

                var checkEmail = await _context.KhachHangs.SingleOrDefaultAsync(x => x.Email.Trim().ToLower() == khachhang.Email.Trim().ToLower());
                if (checkEmail != null)
                {
                    ViewBag.Loi2 = "Địa chỉ Email đã tồn tại";
                    return View("ThemKH");
                }

                var checkPhone = await _context.KhachHangs.SingleOrDefaultAsync(x => x.Phone == khachhang.Phone);
                if (checkPhone != null)
                {
                    ViewBag.Loi3 = "Số điện thoại đã tồn tại";
                    return View("ThemKH");
                }

                // Tạo mới tài khoản cho người dùng
                var user = new Account
                {
                    TaiKhoan = khachhang.Email,
                    RoleId = 3,
                    CreateDate = DateTime.Now
                };

                // Lưu thông tin khách hàng và tài khoản vào cơ sở dữ liệu
                _context.Add(khachhang);
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                return RedirectToAction("Index", "AdminCustomers");
            }

            return View();
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaKH(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            var hasOrders = await _context.DonHangs.AnyAsync(o => o.MaKh == id);
            if (hasOrders)
            {
                TempData["ErrorMessage"] = "Không thể xóa khách hàng vì khách hàng này có đơn hàng.";
                return RedirectToAction("Index");
            }
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.TaiKhoan == khachHang.Email && a.RoleId != 1);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            _context.KhachHangs.Remove(khachHang);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
            return RedirectToAction("Index");
        }

    }
}
