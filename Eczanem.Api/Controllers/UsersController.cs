using Microsoft.AspNetCore.Mvc;
using Eczanem.Api.Data;
using Eczanem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Eczanem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GİRİŞ YAP (LOGIN)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            // Veritabanında bu kullanıcı adı ve şifreye sahip biri var mı?
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı!" });
            }

            // Kullanıcı bulundu, bilgileri geri döndür
            return Ok(user);
        }

        // KULLANICI EKLE (Patron için opsiyonel)
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}