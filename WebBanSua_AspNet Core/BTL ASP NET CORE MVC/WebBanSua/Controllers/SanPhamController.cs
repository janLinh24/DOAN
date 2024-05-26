using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanSua.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using X.PagedList;
using PagedList;
using static WebBanSua.Models.DanhMucSp;
namespace WebBanSua.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly CuaHangBanSuaContext _context;

        public SanPhamController(CuaHangBanSuaContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> SapXepTheoGia(string sortOrder)
        {
            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            var sanPhams = from s in _context.SanPhams
                           select s;

            switch (sortOrder)
            {
                case "price_desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.GiaSp);
                    break;
                case "price_asc":
                    sanPhams = sanPhams.OrderBy(s => s.GiaSp);
                    break;
                case "price_high_to_low":
                    sanPhams = sanPhams.OrderByDescending(s => s.GiaSp);
                    break;
                default:
                    sanPhams = sanPhams.OrderBy(s => s.GiaSp);
                    break;
            }

            return View("Index", await sanPhams.ToListAsync());
        }

        public async Task<IActionResult> Index()
        {


           

            var cuaHangBanSuaContext = _context.SanPhams.Include(s => s.MaDmNavigation);
            return View(await cuaHangBanSuaContext.ToListAsync());


        }
        public async Task<IActionResult> DanhMucMenu()
        {
            var danhMucs = await _context.DanhMucSps.ToListAsync();
            ViewBag.MenuDM = danhMucs;
            return View("Index", danhMucs);

        }
        public async Task<IActionResult> TimKiem(string searchString)
        {
            var sanPhams = from s in _context.SanPhams
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.TenSp.Contains(searchString));
                ViewBag.TT = sanPhams.Count();

            }
            return View("Index", await sanPhams.ToListAsync());
        }



        public async Task<IActionResult> Details(int? id, string ten)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            ViewBag.tenSP = ten;
            ViewBag.iD = id;
            var sanPham = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }
            
            return View(sanPham);
        }

       
        public async Task<IActionResult> SanPhamTheoDanhMuc(int? id, string tenDm)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucSps.FindAsync(id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            ViewBag.Ten = tenDm;

            // Lấy danh sách sản phẩm thuộc danh mục
            var sanPhams = await _context.SanPhams.Where(s => s.MaDm == id).ToListAsync();
            if (sanPhams.Any() == false)
            {
                ViewBag.sp = "Không tìm thấy sản phẩm nào khớp với yêu cầu của bạn";
            }
            else
            {
                ViewBag.sum = sanPhams.Count();
            }
            return View(sanPhams);
        }




        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSp == id);
        }
    }
}
