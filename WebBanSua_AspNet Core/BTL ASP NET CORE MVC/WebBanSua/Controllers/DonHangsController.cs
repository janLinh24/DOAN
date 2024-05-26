using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanSua.Models;

namespace WebBanSua.Controllers
{

    public class DonHangsController : Controller
    {
        private readonly CuaHangBanSuaContext _context;

        public DonHangsController(CuaHangBanSuaContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;

            var DonHang = _context.DonHangs
               .Include(dh => dh.MaKhNavigation) // Kết hợp bảng KhachHang
               .Include(dh => dh.ChiTietDonHangs) // Kết hợp các chi tiết đơn hàng
                   .ThenInclude(cd => cd.MaSpNavigation).Where(cd => cd.TrangThaiHuyDon == 1 || cd.TrangThaiHuyDon == 2) // Kết hợp bảng SanPham trong chi tiết đơn hàng
               .ToList();
            return View(DonHang);
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


        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDh,MaKh,NgayTao,TrangThaiHuyDon,ThanhToan,NgayThanhToan,Note")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email", donHang.MaKh);
            return View(donHang);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrangThaiHuyDon(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);

            if (donHang == null)
            {
                return NotFound();
            }

            donHang.TrangThaiHuyDon = 1; // Cập nhật TrangThaiHuyDon thành 1

            _context.Update(donHang);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDonHang(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);

            if (donHang == null)
            {
                return NotFound();
            }

            donHang.TrangThaiHuyDon = 0; // Cập nhật TrangThaiHuyDon thành 1

            _context.Update(donHang);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email", donHang.MaKh);
            return View(donHang);
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDh,MaKh,NgayTao,TrangThaiHuyDon,ThanhToan,NgayThanhToan,Note")] DonHang donHang)
        {
            if (id != donHang.MaDh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.MaDh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "Email", donHang.MaKh);
            return View(donHang);
        }


        



        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDh == id);
        }
    }
}
