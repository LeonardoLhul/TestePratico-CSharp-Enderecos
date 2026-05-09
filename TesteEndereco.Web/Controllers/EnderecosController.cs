using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteEndereco.Web.Data;
using TesteEndereco.Web.Models;

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

        [HttpGet]
        public IActionResult Create()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Endereco endereco)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(endereco);
            }

            endereco.UsuarioId = usuarioId.Value;

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
