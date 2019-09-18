using BUSINESS;
using MODEL;
using System;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RegistroIMC.Controllers
{
    public class HomeController : Controller
    {
        // Método que autentica o usuário logado.
        private void Autenticacao(string id, string nome)
        {
            // Identidade do usuário
            var identity = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.Sid, id),
                    new Claim(ClaimTypes.Name, nome),
                },
            "ApplicationCookie");

            // Contexto da autenticação
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            // SignIn (autenticação de login)
            authManager.SignIn(identity);
        }

        // Método que pega o RedirectUrl de acordo com sucesso ou falha do login do usuário.
        private string GetRedirectUrl(string returnUrl)
        {
            // Caso o usuário tenha feito login
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            // Caso o usuário tenha um login inválido (volta para a mesma URL atual (action Login))
            return returnUrl;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // Verifica se o usuário está autenticado. Se estiver, desloga.
            if (User.Identity.IsAuthenticated)
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignOut("ApplicationCookie");
                return RedirectToAction("Index");
            }

            // Usuário tem a ReturnUrl de acordo com sua tentativa de login (se logou ou não logou)
            var usuario = new Usuario
            {
                ReturnUrl = returnUrl
            };

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioBusiness usuarioBusiness = new UsuarioBusiness();
                usuario.Senha = Encoding.ASCII.GetBytes(usuarioBusiness.FazerHash(usuario.SenhaHash));

                // Procura pelo usuário digitado. Se encontrar, loga-o.
                System.Data.DataTable dataTable = usuarioBusiness.BuscarDadosUsuario(usuario);
                if (dataTable.Rows.Count > 0)
                {
                    // Autentica o usuário.
                    string id = dataTable.Rows[0][0].ToString();
                    string nome = dataTable.Rows[0][1].ToString();
                    Autenticacao(id, nome);

                    // Redireciona o usuário para a action Index
                    return Redirect(GetRedirectUrl(usuario.ReturnUrl));
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                    return View();
                }
            }
            ModelState.AddModelError(string.Empty, "Preencha os campos corretamente.");
            return View(usuario);
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(UsuarioCadastro usuarioCadastro)
        {
            if (ModelState.IsValid)
            {
                UsuarioBusiness usuarioBusiness = new UsuarioBusiness();

                // Verifica se já existe um usuário com o e-mail informado.
                if (usuarioBusiness.BuscarSeEmailJaExiste(usuarioCadastro.Email).Rows.Count > 0)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "E-mail informado já existe.");
                    return View();
                }
                usuarioCadastro.Senha = Encoding.ASCII.GetBytes(usuarioBusiness.FazerHash(usuarioCadastro.SenhaHash));
                usuarioBusiness.CadastrarUsuario(usuarioCadastro);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Preencha os campos corretamente.");
            return View(usuarioCadastro);
        }
    }
}