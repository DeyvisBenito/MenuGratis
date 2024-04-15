using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaDatos;
using MenuGratis.Filtros;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace MenuGratis.Controllers
{
    public class HomeController : Controller
    {
        [ValidadSesion]

        public ActionResult Index()
        {
            return View();
        }

        

        [ValidadSesion]
        public ActionResult ListarPlatillo() //Vista de platillos, mostrara todos los platillos
        {
           
            return View();
        }

        [ValidadSesion]
        public JsonResult LlenarTablaPlatillo()
        {
            Usuario oUser= Session["Usuario"] as Usuario;
            CD_Usuarios cd_user = new CD_Usuarios();
            int id_menu = cd_user.BuscarMenu(oUser.ID);

            CD_Platillos p1 = new CD_Platillos();
            List<Platillos> lista = new List<Platillos>();
            lista = p1.listar(id_menu);


            foreach (var platillo in lista)
            {
                // Convertir los bytes de la imagen a una cadena base64
                if (platillo.Imagen != null)
                {
                    platillo.ImagenBase64 = Convert.ToBase64String(platillo.Imagen);
                }
            }


            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [ValidadSesion]
        public JsonResult detallarPlatillo(int Id_platillo) //Metodo para rellenar formulario de editar platillo
        {
            try
            {
                Usuario oUser= Session["Usuario"] as Usuario;
                CD_Usuarios cd_user = new CD_Usuarios();
                int id_menu=cd_user.BuscarMenu(oUser.ID);

                CD_Platillos cD_pla = new CD_Platillos();
                List<Platillos> lista = cD_pla.listar(id_menu);


                foreach (Platillos platilloList in lista)
                {
                    if (Id_platillo == platilloList.Id_platillo)
                    {
                        // Convertir los bytes de la imagen a una cadena base64
                        if (platilloList.Imagen != null)
                        {
                            platilloList.ImagenBase64 = Convert.ToBase64String(platilloList.Imagen);
                        }
                        return Json(platilloList, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            catch
            {
                return Json(null);
            }
            return Json(null);
        }

        [HttpPost]
        [ValidadSesion]
        public JsonResult ActualizarPlatillo(string nombre, string descripcion, string precio, string opciones, HttpPostedFileBase imagen, int Id_platillo)
        {
            try
            {
                Platillos opla = new Platillos();
                opla.Id_platillo= Id_platillo;
                opla.Nombre = nombre;
                opla.Descripcion = descripcion;
                opla.precio =double.Parse(precio);
                opla.Tipo = opciones;
                byte[] imagenBytes = null;

                if(imagen != null) { 
                // Leer los datos del archivo en un array de bytes
                using (var binaryReader = new BinaryReader(imagen.InputStream))
                {
                    imagenBytes = binaryReader.ReadBytes(imagen.ContentLength);
                }

                opla.Imagen = imagenBytes;
                }

                CD_Platillos cd_pla = new CD_Platillos();
                bool conf=cd_pla.modificarPlatillo(opla);

                if(conf)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
                
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [ValidadSesion]
        public ActionResult AgregarPlatillos(string nombre, string descripcion, string precio, string opciones, HttpPostedFileBase imagen)
        {
            try
            {
                Usuario oUser=Session["Usuario"] as Usuario;
                int id_user = oUser.ID;
                CD_Usuarios cdUser = new CD_Usuarios();
                int id_menu = cdUser.BuscarMenu(id_user);

                Platillos pla = new Platillos();
                pla.Id_menu = id_menu;
                pla.Nombre = nombre;
                pla.Descripcion = descripcion;
                pla.precio = double.Parse(precio);
                pla.Tipo = opciones;
                byte[] imagenBytes = null;

                // Leer los datos del archivo en un array de bytes
                using (var binaryReader = new BinaryReader(imagen.InputStream))
                {
                    imagenBytes = binaryReader.ReadBytes(imagen.ContentLength);
                }

                pla.Imagen = imagenBytes;

                

                CD_Platillos oCDpla = new CD_Platillos();

                bool band=oCDpla.GuardarPlatillo(pla);


                if (band)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }

                
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [ValidadSesion]
        public JsonResult eliminarPlatillo(int Id_platillo)
        {
            try
            {
                CD_Platillos cd_pla = new CD_Platillos();
                bool resp = cd_pla.eliminarPlatillo(Id_platillo);

                if (resp)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }

            }catch
            {
                return Json(new { success = false });
            }
        }

        [ValidadSesion]
        public ActionResult mostrarDe()
        {
            try { 
            List<Platillos> list = listarPlatillos();

            return View(list);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [ValidadSesion]
        public ActionResult mostEnt()
        {
            try
            {
                List<Platillos> list = listarPlatillos();

                return View(list);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [ValidadSesion]
        public ActionResult mostPlaF()
        {
            try
            {
                List<Platillos> list = listarPlatillos();

                return View(list);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [ValidadSesion]
        public ActionResult mostPost()
        {
            try
            {
                List<Platillos> list = listarPlatillos();

                return View(list);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [ValidadSesion]
        public ActionResult mostBebi()
        {
            try
            {

                List<Platillos> list = listarPlatillos();

                return View(list);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [ValidadSesion]
        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Ingreso", "Login");
        }

        [ValidadSesion]
        private List<Platillos> listarPlatillos()
        {
            CD_Platillos cd_pla = new CD_Platillos();
            CD_Usuarios cd_user = new CD_Usuarios();
            Usuario user = Session["Usuario"] as Usuario;
            int id_menu = cd_user.BuscarMenu(user.ID);

            List<Platillos> list = new List<Platillos>();
            list = cd_pla.listar(id_menu);

            return list;
        }
       
    }
}