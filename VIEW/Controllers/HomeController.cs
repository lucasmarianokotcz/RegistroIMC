using BUSINESS;
using MODEL;
using System;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VIEW.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        #region Autenticação do usuário
        /// <summary>
        /// Autentica o usuário logado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nome"></param>
        private void AutenticarUsuario(Usuario usuario, string nome)
        {
            // Identidade do usuário
            var identity = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.Sid, usuario.CodUsuario.ToString()),
                    new Claim(ClaimTypes.Name, nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                },
            "ApplicationCookie");

            // Contexto da autenticação
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            // SignIn (autenticação de login)
            authManager.SignIn(identity);
        }

        /// <summary>
        /// Pega o RedirectUrl de acordo com sucesso ou falha do login do usuário.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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
        #endregion

        public ActionResult Index()
        {
            return View();
        }

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
        public ActionResult Login(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioBusiness usuarioBusiness = new UsuarioBusiness();
                usuario.Senha = Encoding.ASCII.GetBytes(usuarioBusiness.FazerHashDaSenha(usuario.SenhaHash));

                // Procura pelo usuário digitado. Se encontrar, loga-o.
                System.Data.DataTable dataTable = usuarioBusiness.BuscarDadosUsuario(usuario);
                if (dataTable.Rows.Count > 0)
                {
                    // Autentica o usuário.
                    usuario.CodUsuario = Convert.ToInt32(dataTable.Rows[0][0]);
                    string nome = dataTable.Rows[0][1].ToString();
                    usuario.Email = dataTable.Rows[0][2].ToString();
                    AutenticarUsuario(usuario, nome);

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
                usuarioCadastro.Senha = Encoding.ASCII.GetBytes(usuarioBusiness.FazerHashDaSenha(usuarioCadastro.SenhaHash));
                usuarioBusiness.CadastrarUsuario(usuarioCadastro);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Preencha os campos corretamente.");
            return View(usuarioCadastro);
        }
    }
}