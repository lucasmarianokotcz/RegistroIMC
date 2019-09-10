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

                // Faz o hash de senha.
                usuario.Senha = Encoding.ASCII.GetBytes(usuarioBusiness.FazerHash(usuario.SenhaHash));

                if (usuarioBusiness.BuscarDadosUsuario(usuario).Rows.Count > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario não encontrado.");
                    return View(usuario);
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

                // Faz o hash de senha.
                usuarioCadastro.Senha = Encoding.ASCII.GetBytes(usuarioBusiness.FazerHash(usuarioCadastro.SenhaHash));

                usuarioBusiness.CadastrarUsuario(usuarioCadastro);

                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Preencha os campos corretamente.");
            return View(usuarioCadastro);
        }
    }
}