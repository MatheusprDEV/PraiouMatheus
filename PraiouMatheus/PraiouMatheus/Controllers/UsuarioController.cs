using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PraiouMatheus.Data;
using PraiouMatheus.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PraiouMatheus.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppCont _appCont;

        public UsuarioController(AppCont appCont)
        {
            _appCont = appCont;
        }

        // Action que exibe a página de cadastro
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = await _appCont.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("", "Email já está cadastrado.");
                    return View(usuario);
                }

                _appCont.Usuarios.Add(usuario);
                await _appCont.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        // Action GET para exibir a página de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuario = await _appCont.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario != null)
            {
                // Cria as claims do usuário
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Email)
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Faz login do usuário
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redireciona para a página de EnviarAvaliação
                return RedirectToAction("EnviarAvaliacao", "Avaliacao");
            }
            else
            {
                ViewBag.ErrorMessage = "Email ou senha inválidos.";
                return View();
            }
        }

        // Action GET para exibir o perfil do usuário
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var email = User.Identity.Name;
            if (email == null)
            {
                return RedirectToAction("Login");
            }

            var usuario = await _appCont.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        // Ação de logout
        public async Task<IActionResult> Logout()
        {
            // Limpa a autenticação
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
