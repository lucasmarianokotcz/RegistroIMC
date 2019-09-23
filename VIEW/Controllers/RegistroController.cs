using BUSINESS;
using MODEL;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace VIEW.Controllers
{
    public class RegistroController : Controller
    {
        #region ID do Usuário logado
        private int PegarIdDoUsuarioLogado()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return int.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);
        }
        #endregion

        // GET: Registro
        public ActionResult Index()
        {
            RegistroBusiness registroBusiness = new RegistroBusiness();
            string ordem = Request.QueryString["ordem"] ?? "DataRegistro";
            switch (ordem)
            {
                case "DataRegistro":
                    ViewBag.Data = "selected";
                    break;
                case "Peso":
                    ViewBag.Peso = "selected";
                    break;
                case "Altura":
                    ViewBag.Altura = "selected";
                    break;
                default:
                    return View(registroBusiness.BuscarRegistrosDeUmUsuario(PegarIdDoUsuarioLogado(), "DataRegistro"));
            }
            return View(registroBusiness.BuscarRegistrosDeUmUsuario(PegarIdDoUsuarioLogado(), ordem));
        }

        // GET: Registro/Novo
        public ActionResult Novo()
        {
            return View();
        }

        // POST: Registro/Novo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Novo(Registro registro)
        {
            registro.CodUsuario = PegarIdDoUsuarioLogado();
            if (ModelState.IsValid)
            {
                if (registro.DataRegistro > DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Não é permitido selecionar uma data maior que a de hoje.");
                    return View(registro);
                }
                RegistroBusiness registroBusiness = new RegistroBusiness();
                registroBusiness.RegistarImc(registro);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Verifique campos inválidos.");
            return View(registro);
        }
    }
}