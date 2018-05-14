using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Loja.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private AccountController ac = new AccountController();

        //[Authorize]
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public AdminController()
        {
        }

        public ActionResult UsuariosLista()
        {
            var model = ac.PegaLista();
            return View("Usuarios", model);
        }

        [HttpGet]
        public ActionResult EditaUsuario(int ID)
        {
            return RedirectToAction("EditaUsuario", new RouteValueDictionary(
                            new { controller = "Account", action = "EditaUsuario", ID = ID }));
        }

        [HttpGet]
        public ActionResult MudaSituacao(int ID)
        {
            return RedirectToAction("MudaSituacao", new RouteValueDictionary(
                            new { controller = "Account", action = "MudaSituacao", ID = ID }));
        }
        
    }
}