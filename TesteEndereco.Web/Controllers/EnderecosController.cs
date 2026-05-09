using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteEndereco.Web.Data;
using TesteEndereco.Web.Models;
using TesteEndereco.Web.Services;

namespace TesteEndereco.Web.Controllers
{
    public class EnderecosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ViaCepService _viaCepService;

        public EnderecosController(AppDbContext context, ViaCepService viaCepService)
        {
            _context = context;
            _viaCepService = viaCepService;
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

            endereco.Cep = new string(endereco.Cep.Where(char.IsDigit).ToArray());

            if (!ModelState.IsValid)
            {
                return View(endereco);
            }

            endereco.UsuarioId = usuarioId.Value;

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarPorCep(string cep)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return Unauthorized();
            }

            var endereco = await _viaCepService.BuscarEnderecoPorCepAsync(cep);

            if (endereco == null || endereco.Erro)
            {
                return NotFound(new { mensagem = "CEP não encontrado." });
            }

            return Json(new
            {
                cep = endereco.Cep,
                logradouro = endereco.Logradouro,
                complemento = endereco.Complemento,
                bairro = endereco.Bairro,
                cidade = endereco.Localidade,
                uf = endereco.Uf
            });
        }
    }
}
