using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteEndereco.Web.Data;

namespace TesteEndereco.Web.Controllers
{
    public class EnderecosController : Controller
    {
        private readonly AppDbContext _context;

        public EnderecosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var enderecos = await _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId.Value)
                .ToListAsync();

            return View(enderecos);
        }
    }
}
