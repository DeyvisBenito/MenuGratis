using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenuGratis.Controllers;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using CapaEntidad;

namespace MenuGratis.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Ingreso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autenticar(string Correo, string Contrasena)
        {
            List<Usuario> olista = new List<Usuario>();
            olista = new CD_Usuarios().Listar();

            foreach (var user in olista)
            {
                CD_Usuarios oCD_U = new CD_Usuarios();
                string hashedPassword = oCD_U.GetSHA256Hash(Contrasena);
                if (user.Correo == Correo && user.Contrasena == hashedPassword)
                {
                    // Si encontramos un usuario y contraseña correctos, almacenamos el usuario en la sesión
                    Session["Usuario"] = user;
                    // Redireccionar directamente a la acción Index del controlador Home
                    return RedirectToAction("Index", "Home");
                }
            }

            // Si no se encuentra un usuario y contraseña correctos, mostramos un mensaje de error
            TempData["Error"] = "Usuario inválido";
            return RedirectToAction("Ingreso", "Login");
        }

        

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Registrar_Restaurante()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AutentificarRegistro(string name, string lname, string phone, string sex, string email, string password, string restaurant)
        {
            try
            {
                Usuario oUsuarios = new Usuario();
                oUsuarios.Nombre = name;
                oUsuarios.Apellido=lname;
                oUsuarios.Telefono= phone;
                oUsuarios.Sexo= sex;
                oUsuarios.Correo=email;
                oUsuarios.Contrasena= password;
                oUsuarios.Restaurante= restaurant;
                oUsuarios.Ubicacion = 0;
                oUsuarios.Restablecer = false;
                oUsuarios.fec_registro = DateTime.Now;


                List<Usuario> olista = new List<Usuario>();
                olista = new CD_Usuarios().Listar();

                foreach (var user in olista)
                {
                    if (user.Correo == oUsuarios.Correo)
                    {
                        // Si encontramos un correo igual, mandaremos un mensaje de error
                        TempData["Registro"] = "Correo ya existente";
                        
                        // Redireccionar directamente a la misma vista
                        return RedirectToAction("Registrar", "Login");
                    }
                }
                //Guardando datos del usuario
                TempData["Usuario"] = oUsuarios;
                return RedirectToAction("Registrar_Restaurante", "Login");
            }
            catch
            {
                TempData["Registro"] = "Ha ocurrido un error";
                return RedirectToAction("Registrar", "Login");
            }
            
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(string pais, string dept, string city, string zone, string street)
        {
            try
            {
                Ubicacion oUbicacion = new Ubicacion();
                oUbicacion.Pais = pais;
                oUbicacion.Departamento=dept;
                oUbicacion.Municipio = city;
                oUbicacion.Zona = zone;
                oUbicacion.Calle=street;

                

                CD_Usuarios oCD_Usuarios = new CD_Usuarios();
                int ultimoIDUbi = oCD_Usuarios.InsertarUbicaciom(oUbicacion);
                Usuario oUsuario = new Usuario();
                oUsuario = TempData["Usuario"] as Usuario;

                oCD_Usuarios.InsertarUsuario(oUsuario, ultimoIDUbi);
                TempData["Confir"] = "Usuario inscrito correctamente";
                return RedirectToAction("Ingreso", "Login");

            }
            catch
            {
                TempData["Confir"] = "Ha ocurrido un error";
                return RedirectToAction("Registrar_Restaurante", "Login");
            }
        }
    }
    }