using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebBanSua.Models;
using MoreLinq;
using System.Collections.Generic;
namespace WebBanSua.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
       

        private readonly CuaHangBanSuaContext _context;

        public HomeController(CuaHangBanSuaContext context)
        {
            _context = context;
          
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AdminProfile(int id)
        {
            var maKH = HttpContext.Session.GetString("MaKh");
            id = int.Parse(maKH);
            var customerUser = await _context.KhachHangs.FindAsync(id);

            return View(customerUser);
        }
       
  
        [HttpPost]
        public IActionResult AdminProfile(KhachHang customer, Account user)
        {
            if (string.IsNullOrEmpty(customer.TenKh) == true)
            {
                ModelState.AddModelError("", "Tên không được để trống");
                return View(customer);
            }
            _context.Update(customer);
            var check = _context.SaveChanges();
            if (check > 0)
            {
                return RedirectToAction("UserDashboard");
            }
            else
            {
                ModelState.AddModelError("", "Lỗi lưu dữ liệu");
                return View(customer);
            }
        }

       

        
       



        public async Task<IActionResult> AdminChangePassword(int id)
        {
            var maKH = HttpContext.Session.GetString("MaKh");
            id = int.Parse(maKH);
            var customerUser = await _context.KhachHangs.FindAsync(id);

            return View(customerUser);
        }

        [HttpPost]
        public IActionResult AdminChangePassword(KhachHang customer)
        {
            if (string.IsNullOrEmpty(customer.Password) == true)
            {
                ModelState.AddModelError("", "Mật khẩu không được để trống");
                return View(customer);
            }
            var check = _context.SaveChanges();
            if (check > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Lỗi lưu dữ liệu");
                return View(customer);
            }
        }

    }
}
