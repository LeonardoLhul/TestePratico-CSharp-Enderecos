using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteEndereco.Web.Data;
using TesteEndereco.Web.Models;
using TesteEndereco.Web.Services;
using System.Text;

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

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId.Value);

            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Endereco endereco)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != endereco.Id)
            {
                return NotFound();
            }

            endereco.Cep = new string(endereco.Cep.Where(char.IsDigit).ToArray());

            if (!ModelState.IsValid)
            {
                return View(endereco);
            }

            var enderecoExistente = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId.Value);

            if (enderecoExistente == null)
            {
                return NotFound();
            }

            enderecoExistente.Cep = endereco.Cep;
            enderecoExistente.Logradouro = endereco.Logradouro;
            enderecoExistente.Complemento = endereco.Complemento;
            enderecoExistente.Bairro = endereco.Bairro;
            enderecoExistente.Cidade = endereco.Cidade;
            enderecoExistente.Uf = endereco.Uf;
            enderecoExistente.Numero = endereco.Numero;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId.Value);

            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId.Value);

            if (endereco == null)
            {
                return NotFound();
            }

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ExportarCsv()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var enderecos = await _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId.Value)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Uf");

            foreach (var endereco in enderecos)
            {
                csv.AppendLine(
                    $"\"{endereco.Cep}\",\"{endereco.Logradouro}\",\"{endereco.Numero}\",\"{endereco.Complemento}\",\"{endereco.Bairro}\",\"{endereco.Cidade}\",\"{endereco.Uf}\"");
            }

            var bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
                .ToArray();
            var nomeArquivo = $"enderecos-{DateTime.Now:yyyyMMddHHmmss}.csv";

            return File(bytes, "text/csv; charset=utf-8", nomeArquivo);
        }
    }
}
