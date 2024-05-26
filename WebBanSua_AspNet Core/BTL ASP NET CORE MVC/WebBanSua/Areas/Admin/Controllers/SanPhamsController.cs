using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using WebBanSua.Models;

namespace WebBanSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamsController : Controller
    {
        private readonly CuaHangBanSuaContext _context;

        public SanPhamsController(CuaHangBanSuaContext context)
        {
            _context = context;

        }


        public async Task<IActionResult> Index()
        {
            var cuaHangBanSuaContext = _context.SanPhams.Include(s => s.MaDmNavigation);
            return View(await cuaHangBanSuaContext.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

       
        public IActionResult Create()
        {
            ViewData["MaDm"] = new SelectList(_context.DanhMucSps, "MaDm", "TenDm");
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("MaSp,MaDm,TenSp,AnhSp,VideoSp,GiaSp,TrangThai,SoLuong,BestSeller,CreateDate,NgaySua,MotaSp")] SanPham sanPham)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(sanPham);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MaDm"] = new SelectList(_context.DanhMucSps, "MaDm", "TenDm", sanPham.MaDm);
        //    return View(sanPham);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,MaDm,TenSp,VideoSp,GiaSp,TrangThai,SoLuong,BestSeller,CreateDate,NgaySua,MotaSp")] SanPham sanPham, IFormFile anhSpFile)
        {
            if (ModelState.IsValid)
            {
                if (anhSpFile != null && anhSpFile.Length > 0)
                {
                    // Tạo tên file duy nhất cho ảnh
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhSpFile.FileName);

                    // Đường dẫn đến thư mục lưu trữ ảnh trong wwwroot
                    var uploadFolder = Path.Combine("wwwroot", "img/product");

                    // Đường dẫn đến ảnh hoàn chỉnh
                    var filePath = Path.Combine(uploadFolder, fileName);

                    // Lưu ảnh vào thư mục
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhSpFile.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn ảnh vào trường AnhSpPath của đối tượng SanPham
                    sanPham.AnhSp = "~/img/product/" + fileName;
                }

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDm"] = new SelectList(_context.DanhMucSps, "MaDm", "TenDm", sanPham.MaDm);
            return View(sanPham);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["MaDm"] = new SelectList(_context.DanhMucSps, "MaDm", "TenDm", sanPham.MaDm);
            return View(sanPham);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSp,MaDm,TenSp,AnhSp,VideoSp,GiaSp,TrangThai,SoLuong,BestSeller,CreateDate,NgaySua,MotaSp")] SanPham sanPham)
        {
            if (id != sanPham.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSp))
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
            ViewData["MaDm"] = new SelectList(_context.DanhMucSps, "MaDm", "TenDm", sanPham.MaDm);
            return View(sanPham);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound(); // Trả về lỗi nếu không tìm thấy sản phẩm
            }
            var chiTietDonHangs = _context.ChiTietDonHangs.Where(ct => ct.MaSp == id);
            _context.ChiTietDonHangs.RemoveRange(chiTietDonHangs);

            _context.SanPhams.Remove(sanPham);


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSp == id);
        }
    }
}
