using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuGratis.Filtros
{
    public class ValidadSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Verificar si el usuario tiene una sesión activa
            if (HttpContext.Current.Session["Usuario"] == null)
            {
                // Redirigir al usuario a la página de inicio de sesión
                filterContext.Result = new RedirectResult("~/Login/Ingreso");
                return;
            }
            else
            {
                // Lógica para obtener la lista de secciones
                Usuario oUser = filterContext.HttpContext.Session["Usuario"] as Usuario;
                CD_Secciones cd_s = new CD_Secciones();
                List<Secciones> list = cd_s.listarSec(oUser.ID);

                // Asignar la lista de secciones a ViewBag
                if (list != null)
                {
                    filterContext.Controller.ViewBag.ListaDeSecciones = list;
                }
                else
                {
                    filterContext.Controller.ViewBag.ListaDeSecciones = new List<Secciones>(); // Asignar una lista vacía
                }
            }

            

            base.OnActionExecuting(filterContext);
        }

    }
}