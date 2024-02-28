using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenuGratis.Controllers;
using MySql.Data.MySqlClient;

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
                if (user.Correo == Correo && user.Contrasena == Contrasena)
                {
                    // Si encontramos un usuario y contraseña correctos, almacenamos el usuario en la sesión
                    Session["Usuario"] = user;
                    // Redireccionar directamente a la acción Index del controlador Home
                    return RedirectToAction("Index", "Home");
                }
            }

            // Si no se encuentra un usuario y contraseña correctos, mostramos un mensaje de error
            ViewData["Mensaje"] = "Usuario invalido";
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Registrar_Restaurante()
        {
            return View();
        }

       // [HttpPost]
       // public ActionResult Registrar(Usuario oUsuario)
        //{
          //  try
           // {
             //   using (MySqlConnection oConexion= new MySqlConnection(Conexion.cn))
               // {

              //  }
           // }
            
       // }
    }
    }