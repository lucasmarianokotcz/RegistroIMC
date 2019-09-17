using BUSINESS;
using MODEL;
using System.Text;
using System.Web.Mvc;

namespace RegistroIMC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioBusiness usuarioBusiness = new UsuarioBusiness();
                usuario.Senha = Encoding.ASCII.GetBytes(usuarioBusiness.FazerHash(usuario.SenhaHash));

                // Procura pelo usuário digitado. Se encontrar, loga-o.
                if (usuarioBusiness.BuscarDadosUsuario(usuario).Rows.Count > 0)
                {
                    return RedirectToAction("Index");
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