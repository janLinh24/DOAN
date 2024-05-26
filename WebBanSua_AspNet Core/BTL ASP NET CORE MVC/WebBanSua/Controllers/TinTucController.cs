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
    public class TinTucController : Controller
    {
        private readonly CuaHangBanSuaContext _context;

        public TinTucController(CuaHangBanSuaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TinTucs.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs
                .FirstOrDefaultAsync(m => m.MaTt == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }

       
    }
}
