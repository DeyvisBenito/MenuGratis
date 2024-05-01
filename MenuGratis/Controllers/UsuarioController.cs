using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaEntidad;
using MenuGratis.Filtros;
using Microsoft.Ajax.Utilities;

namespace MenuGratis.Controllers
{
    public class UsuarioController : Controller
    {
        [ValidadSesion]
        // GET: Usuario
        public ActionResult ModUsuario() { 
                try
                {
                    CD_Usuarios cD_Usuarios = new CD_Usuarios();
                    List<Usuario> lista = cD_Usuarios.Listar();

                    // Verificar si la sesión "Usuario" está disponible y no es nula
                    if (Session["Usuario"] != null && Session["Usuario"] is Usuario)
                    {
                        Usuario oUser = Session["Usuario"] as Usuario;

                        foreach (Usuario usuariosList in lista)
                        {
                            if (oUser.ID == usuariosList.ID)
                            {
                                return View(usuariosList);
                            }
                        }
                    }

                    // Si no se encuentra el usuario en la lista, redirigir al login
                    return RedirectToAction("Ingreso", "Login");
                }
            
            catch
            {
                return RedirectToAction("Ingreso", "Login");
            }
        }

        [HttpPost]
        public JsonResult GuardarUpdate(string name, string lname, string sex, string phone, string restaurant)
        {
            try
            {
                Usuario oUser = Session["Usuario"] as Usuario;
                oUser.Nombre = name;
                oUser.Apellido = lname;
                oUser.Sexo = sex;
                oUser.Telefono = phone;
                oUser.Restaurante= restaurant;
                CD_Usuarios cD_Usuarios = new CD_Usuarios();
                bool conf=cD_Usuarios.ActualizarUsuario(oUser);

                if (conf)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
                
               

            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message }); 
            }

        }

        public ActionResult ModRestaurante() 
        {
            try
            {
                // Verificar si la sesión "Usuario" está disponible y no es nula
                if (Session["Usuario"] != null && Session["Usuario"] is Usuario)
                {
                    Usuario oUser = Session["Usuario"] as Usuario;
                    CD_Usuarios cD_Usuarios = new CD_Usuarios();
                    Ubicacion oUbi=cD_Usuarios.buscarUbicacion(oUser.Ubicacion);

                    return View(oUbi);
                }

                // Si no se encuentra el usuario en la lista, redirigir al login
                return RedirectToAction("Ingreso", "Login");
            }

            catch
            {
                return RedirectToAction("Ingreso", "Login");
            }
        }


        [HttpPost]
        public JsonResult GuardarUpdateUbica(string country, string state, string city, string adress, string street)
        {
            try
            {
                Usuario oUser = Session["Usuario"] as Usuario;
                Ubicacion oUbi = new Ubicacion();
                oUbi.Pais = country;
                oUbi.Departamento = state;
                oUbi.Municipio = city;
                oUbi.Zona = adress;
                oUbi.Calle = street;
                oUbi.Id_res = oUser.Ubicacion;
                CD_Usuarios oCD_U = new CD_Usuarios();
                bool conf=oCD_U.ActualizarUbicacion(oUbi);

                if (conf)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
               


            }
            catch(Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}