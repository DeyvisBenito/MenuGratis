using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaEntidad;
using MenuGratis.Filtros;

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
                    return RedirectToAction("Index", "Login");
                }
            
            catch
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult GuardarUpdate(string name, string lname, string sex, string phone, string restaurant)
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
                string conf=cD_Usuarios.ActualizarUsuario(oUser);

                TempData["Actualizacion"]= conf;
                return RedirectToAction("Index", "Home");

            }
            catch
            {
                return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            }

            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult GuardarUpdateUbica(string country, string state, string city, string adress, string street)
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
                string conf=oCD_U.ActualizarUbicacion(oUbi);

                return RedirectToAction("Index", "Home");

            }catch(Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}