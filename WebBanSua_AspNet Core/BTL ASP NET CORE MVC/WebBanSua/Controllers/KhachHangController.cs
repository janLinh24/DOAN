using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using WebBanSua.Models;
using Microsoft.EntityFrameworkCore;

namespace WebBanSua.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly CuaHangBanSuaContext _context;

        public KhachHangController(CuaHangBanSuaContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Forgot(string email)
        {
            var user = _context.KhachHangs.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ViewBag.Message = "Không có tài khoản nào được liên kết với email này.";
                return View();
            }

            var resetToken = Guid.NewGuid().ToString("N").Substring(0,8);
            var resetLink = Url.Action("Reset", "KhachHang", new { token = resetToken }, HttpContext.Request.Scheme);

            user.Password = resetToken;
            _context.SaveChanges();

            await SendPasswordResetEmail(email, resetLink);

            ViewBag.Message = "Chúng tôi đã gửi email reset mật khẩu cho bạn. Vui lòng kiểm tra hộp thư đến của bạn.";
            return View();
        }
        [HttpGet]
        public ActionResult Reset(string token)
        {
            var user = _context.KhachHangs.FirstOrDefault(u => u.Password == token);

            if (user == null)
            {
                ViewBag.Message = "Đường link reset mật khẩu không hợp lệ.";
                return View();
            }

            ViewBag.Token = token;
            return View();
        }
        [HttpPost]
        public ActionResult Reset(string token, string newPassword)
        {
            var user = _context.KhachHangs.FirstOrDefault(u => u.Password == token);

            if (user == null)
            {
                ViewBag.Message = "Đường link reset mật khẩu không hợp lệ.";
                return View();
            }

            user.Password = newPassword;
            _context.SaveChanges();

            ViewBag.Message = "Mật khẩu đã được cập nhật thành công.Vui lòng đăng nhập lại";
            return View();
        }

        private async Task SendPasswordResetEmail(string recipientEmail, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PozaaTea", "quanglinhsoserius@gmail.com"));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "Reset Password";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Chào bạn,</p><p>Bạn nhận được email này vì bạn (hoặc ai đó) đã yêu cầu đặt lại mật khẩu cho tài khoản của mình.</p><p>Nếu bạn không yêu cầu việc này, vui lòng bỏ qua email này.</p><p>Đường link để đặt lại mật khẩu của bạn là:</p><p><a href='{resetLink}'>{resetLink}</a></p>";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("quanglinhsoserius@gmail.com", "aqxpyhgwtytyouxh");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }


    }
}
