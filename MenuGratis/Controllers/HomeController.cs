using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaDatos;
using MenuGratis.Filtros;

namespace MenuGratis.Controllers
{
    public class HomeController : Controller
    {
        [ValidadSesion]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Platillos()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Ingreso", "Login");
        }

        public JsonResult ListarUsuarios() //Solo Coloque este Json para debugear si funciona la sentencia de llamado de usuario
        {
            List<Usuario> oLista = new List<Usuario>();

            oLista = new CD_Usuarios().Listar();

            return Json(oLista, JsonRequestBehavior.AllowGet);

        }
    }
}