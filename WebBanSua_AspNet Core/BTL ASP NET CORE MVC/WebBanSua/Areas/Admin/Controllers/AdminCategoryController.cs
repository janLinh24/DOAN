using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanSua.Models;

namespace WebBanSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly CuaHangBanSuaContext _context;

        public AdminCategoryController(CuaHangBanSuaContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.DanhMucSps.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSp = await _context.DanhMucSps
                .FirstOrDefaultAsync(m => m.MaDm == id);
            if (danhMucSp == null)
            {
                return NotFound();
            }

            return View(danhMucSp);
        }


        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("MaDm,TenDm,AnhDm,MoTaDm,TrangThai")] DanhMucSp danhMucSp)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(danhMucSp);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(danhMucSp);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDm,TenDm,AnhDm,MoTaDm,TrangThai")] DanhMucSp danhMucSp, IFormFile anhDmFile)
        {
            if (ModelState.IsValid)
            {
                if (anhDmFile != null && anhDmFile.Length > 0)
                {
                    // Tạo tên file duy nhất cho ảnh
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhDmFile.FileName);

                    // Đường dẫn đến thư mục lưu trữ ảnh trong wwwroot
                    var uploadFolder = Path.Combine("wwwroot", "img/category");

                    // Đường dẫn đến ảnh hoàn chỉnh
                    var filePath = Path.Combine(uploadFolder, fileName);

                    // Lưu ảnh vào thư mục
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhDmFile.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn ảnh vào trường AnhDm của đối tượng DanhMucSp
                    danhMucSp.AnhDm = "~/img/category/" + fileName;
                }

                _context.Add(danhMucSp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhMucSp);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSp = await _context.DanhMucSps.FindAsync(id);
            if (danhMucSp == null)
            {
                return NotFound();
            }
            return View(danhMucSp);
        }
      


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDm,TenDm,AnhDm,MoTaDm,TrangThai")] DanhMucSp danhMucSp)
        {
            if (id != danhMucSp.MaDm)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMucSp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucSpExists(danhMucSp.MaDm))
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
            return View(danhMucSp);
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var danhmuc = await _context.DanhMucSps
        //        .FirstOrDefaultAsync(m => m.MaDm == id);
        //    if (danhmuc == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(danhmuc);
        //}


  
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhmuc = await _context.DanhMucSps.FindAsync(id);
            if (danhmuc == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có sản phẩm nào thuộc danh mục này không
            var hasProducts = await _context.SanPhams.AnyAsync(sp => sp.MaDm == id);
            if (hasProducts)
            {
                TempData["ErrorMessage"] = "Không thể xóa danh mục vì có sản phẩm thuộc danh mục này.";
                return RedirectToAction(nameof(Index));
            }

            _context.DanhMucSps.Remove(danhmuc);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }


        private bool DanhMucSpExists(int id)
        {
            return _context.DanhMucSps.Any(e => e.MaDm == id);
        }
    }
}
