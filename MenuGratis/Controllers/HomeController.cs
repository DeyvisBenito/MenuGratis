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
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                CD_Secciones cd_s=new CD_Secciones();
                Secciones sec=cd_s.buscarSeccion(id.Value);

                ViewBag.Section = sec;

                List<Platillos> list = listarPlatillos();

                return View(list);
            }

            return View();
        }



        [HttpPost]
        [ValidadSesion]
        public ActionResult AgregarSeccion(string nom)
        {
            try
            {
                Usuario oUser = Session["Usuario"] as Usuario;
                CD_Secciones cd_p= new CD_Secciones();
                Secciones sec = new Secciones();

                sec.Nombre = nom;
                sec.Id_res = oUser.ID;

                bool resp=cd_p.Agregar(sec);

                if (resp)
                {
                    return RedirectToAction("ListarPlatillo", "Home");
                }

            }catch (Exception ex)
            {

            }
            return null;
        }

        

        [ValidadSesion]
        public ActionResult ListarPlatillo() //Vista de platillos, mostrara todos los platillos
        {


            return View();
        }

        [HttpGet]
        [ValidadSesion]
        public JsonResult ObtenerSecciones()
        {
            try
            {
                CD_Secciones cd_secciones = new CD_Secciones();
                Usuario oUser = Session["Usuario"] as Usuario;
                List<Secciones> secciones = cd_secciones.listarSec(oUser.ID);

                // Devolver las secciones en formato JSON
                return Json(secciones, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Manejar el error, puedes devolver un mensaje de error o un código de estado 500
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ValidadSesion]
        public JsonResult ObtenerSecciones2(int Id_platillo)
        {
            try
            {
                CD_Platillos cd_pla = new CD_Platillos();
                Platillos pla=cd_pla.ObtenerPlatillo(Id_platillo);


                CD_Secciones cd_secciones = new CD_Secciones();
                Usuario oUser = Session["Usuario"] as Usuario;
                List<Secciones> secciones = cd_secciones.listarSec(oUser.ID);

                var resultado = new
                {
                    Secciones = secciones,
                    Platillo = pla
                };

                // Devolver las secciones en formato JSON
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Manejar el error, puedes devolver un mensaje de error o un código de estado 500
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [ValidadSesion]
        [HttpGet]
        public JsonResult LlenarTablaPlatillo()
        {
            try
            {
                Usuario oUser = Session["Usuario"] as Usuario;
                CD_Usuarios cd_user = new CD_Usuarios();
                int id_menu = cd_user.BuscarMenu(oUser.ID);

                CD_Platillos p1 = new CD_Platillos();
                List<Platillos> lista = new List<Platillos>();
                lista = p1.listar(id_menu);


                return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            }catch (Exception ex) {
  
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidadSesion]
        public JsonResult detallarPlatillo(int Id_platillo) //Metodo para rellenar formulario de editar platillo
        {
            try
            {
                Usuario oUser = Session["Usuario"] as Usuario;
                CD_Usuarios cd_user = new CD_Usuarios();
                int id_menu = cd_user.BuscarMenu(oUser.ID);

                CD_Platillos cD_pla = new CD_Platillos();
                List<Platillos> lista = cD_pla.listar(id_menu);

                foreach (Platillos platilloList in lista)
                {
                    if (Id_platillo == platilloList.Id_platillo)
                    {
                        // Crear un objeto anónimo con las propiedades necesarias
                        var platilloDetalle = new
                        {
                            platilloList.Id_platillo,
                            platilloList.Id_menu,
                            platilloList.Nombre,
                            platilloList.Descripcion,
                            platilloList.Tipo,
                            platilloList.precio,
                            platilloList.Imagen
                        };

                        return Json(platilloDetalle, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [ValidadSesion]
        public JsonResult ActualizarPlatillo(string nombre, string descripcion, string precio, string opciones, HttpPostedFileBase imagen, int Id_platillo)
        {
            try
            {
                CD_Platillos cd_pla = new CD_Platillos();

                Platillos opla = new Platillos();
                opla.Id_platillo = Id_platillo;
                opla.Nombre = nombre;
                opla.Descripcion = descripcion;
                opla.precio = double.Parse(precio);
                opla.Tipo = opciones;


                // Si se envió una nueva imagen, actualizar la ruta de la imagen en el objeto Platillos
                if (imagen != null && imagen.ContentLength > 0)
                {
                    // Guardar la nueva imagen en el sistema de archivos del servidor
                    var file = imagen;
                    var filName = Path.GetFileNameWithoutExtension(file.FileName); // Nombre del archivo sin extensión
                    var fileExt = Path.GetExtension(file.FileName); // Extensión del archivo

                    // Generar un nombre único para la imagen combinando el nombre sin extensión y un identificador único (GUID)
                    string uniqueFileName = $"{filName}_{Guid.NewGuid()}{fileExt}";

                    string ruta = string.Format("/Images/Platillos/{0}", uniqueFileName); // Ruta completa de la imagen
                    string oPath = Server.MapPath("~" + ruta); // Ruta física en el servidor

                    file.SaveAs(oPath); // Guardar la imagen en el servidor

                    opla.Imagen = ruta; // Asignar la ruta única al objeto Platillos

                    // Obtener la ruta de la imagen del platillo antes de eliminarlo
                    string rutaImagen = cd_pla.ObtenerRutaImagen(Id_platillo);

                    // Eliminar la imagen del platillo del sistema de archivos
                    if (!string.IsNullOrEmpty(rutaImagen))
                    {
                        EliminarImagen(rutaImagen);
                    }
                }

               
                
                bool conf = cd_pla.modificarPlatillo(opla);

                

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

        [HttpPost]
        [ValidadSesion]
        public ActionResult AgregarPlatillos(string nombre, string descripcion, string precio, string opciones, HttpPostedFileBase imagen)
        {
            try
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    Usuario oUser = Session["Usuario"] as Usuario;
                    int id_user = oUser.ID;

                    CD_Usuarios cdUser = new CD_Usuarios();
                    int id_menu = cdUser.BuscarMenu(id_user);

                    var file = imagen;
                    var filName = Path.GetFileNameWithoutExtension(file.FileName); // Nombre del archivo sin extensión
                    var fileExt = Path.GetExtension(file.FileName); // Extensión del archivo

                    // Generar un nombre único para la imagen combinando el nombre sin extensión y un identificador único (GUID)
                    string uniqueFileName = $"{filName}_{Guid.NewGuid()}{fileExt}";

                    string ruta = string.Format("/Images/Platillos/{0}", uniqueFileName); // Ruta completa de la imagen

                    string oPath = Server.MapPath("~" + ruta); // Ruta física en el servidor

                    file.SaveAs(oPath); // Guardar la imagen en el servidor

                    Platillos pla = new Platillos();
                    pla.Id_menu = id_menu;
                    pla.Nombre = nombre;
                    pla.Descripcion = descripcion;
                    pla.precio = double.Parse(precio);
                    pla.Tipo = opciones;
                    pla.Imagen = ruta; // Asignar la ruta única al objeto Platillos

                    CD_Platillos oCDpla = new CD_Platillos();
                    bool band = oCDpla.GuardarPlatillo(pla);

                    if (band)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "No se ha cargado ninguna imagen." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpPost]
        [ValidadSesion]
        public JsonResult eliminarPlatillo(int Id_platillo)
        {
            try
            {
                CD_Platillos cd_pla = new CD_Platillos();

                // Obtener la ruta de la imagen del platillo antes de eliminarlo
                string rutaImagen = cd_pla.ObtenerRutaImagen(Id_platillo);

                bool resp = cd_pla.eliminarPlatillo(Id_platillo);

                if (resp)
                {
                    // Eliminar la imagen del platillo del sistema de archivos
                    if (!string.IsNullOrEmpty(rutaImagen))
                    {
                        EliminarImagen(rutaImagen);
                    }

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

        // Método para eliminar la imagen del sistema de archivos
        private void EliminarImagen(string rutaImagen)
        {
            try
            {
                if (System.IO.File.Exists(Server.MapPath(rutaImagen)))
                {
                    System.IO.File.Delete(Server.MapPath(rutaImagen));
                }
            }
            catch 
            {
                
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