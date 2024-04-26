using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web;
using System.Security.Cryptography;
using Mysqlx.Cursor;
using MySqlX.XDevAPI;
using Mysqlx.Session;
using System.Security.AccessControl;

namespace CapaDatos
{
    public class CD_Platillos
    {

        public CD_Platillos() { }




        public bool GuardarPlatillo(Platillos platillo)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "INSERT INTO `platillos` " +
                                 "(`ID_MENU`, `NOMBRE`, `DESCRIPCION`, `PRECIO`, `FECHA_CREADO`, `IMAGEN_PLATILLO`, `TIPO` ) " +
                                 "VALUES (@id_menu, @nombre, @descri, @precio, @fecha, @imagen, @tipo )";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_menu", platillo.Id_menu);
                    cmd.Parameters.AddWithValue("@nombre", platillo.Nombre);
                    cmd.Parameters.AddWithValue("@descri", platillo.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", platillo.precio);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@imagen", platillo.Imagen); // Guardar la ruta de la imagen
                    cmd.Parameters.AddWithValue("@tipo", platillo.Tipo);

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    oConexion.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Platillos> listar(int id_menu)
        {
            List<Platillos> lista = new List<Platillos>();
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "SELECT `ID_PLATILLO`, `ID_MENU`, `NOMBRE`, `DESCRIPCION`, `PRECIO`, `TIPO`, `IMAGEN_PLATILLO` FROM `platillos` WHERE `ID_MENU` = @id_menu ";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_menu", id_menu);

                    oConexion.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            
                                lista.Add(
                                    new Platillos()
                                    {
                                        Id_platillo = Convert.ToInt32(dr["ID_PLATILLO"]),
                                        Id_menu = Convert.ToInt32(dr["ID_MENU"]),
                                        Nombre = dr["NOMBRE"].ToString(),
                                        Descripcion = dr["DESCRIPCION"].ToString(),
                                        Tipo = dr["TIPO"].ToString(),
                                        precio = Convert.ToDouble(dr["PRECIO"]),
                                        Imagen = dr["IMAGEN_PLATILLO"].ToString(),

                                    }
                                );
                            
                        }
                    }
                }
                
            }
            catch
            {
                lista = new List<Platillos>();
            }

            return lista;
        }

        public bool modificarPlatillo(Platillos pla)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "UPDATE `platillos` " +
                                 "SET `NOMBRE` = @nombre, " +
                                 "`DESCRIPCION` = @desc, " +
                                 "`PRECIO` = @precio, " +
                                 "`TIPO` = @tipo  " ;
                                


                    
                         
                    
                    // Agregar la parte de la consulta para la imagen solo si no es nula
                    if (pla.Imagen != null)
                    {
                        sql += ", `IMAGEN_PLATILLO` = @imagen ";
                    }

                    sql += " WHERE `ID_PLATILLO` = @id ";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@nombre", pla.Nombre);
                    cmd.Parameters.AddWithValue("@desc", pla.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", pla.precio);
                    cmd.Parameters.AddWithValue("@tipo", pla.Tipo);

                    if (pla.Imagen != null)
                    {
                        cmd.Parameters.AddWithValue("@imagen", pla.Imagen);
                    }

                    cmd.Parameters.AddWithValue("@id", pla.Id_platillo);

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    oConexion.Close();
                

                return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool eliminarPlatillo(int Id_platillo)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "DELETE FROM `platillos` " +
                                 " WHERE `ID_PLATILLO` = @id ";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id", Id_platillo);

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    oConexion.Close();


                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Platillos ObtenerPlatillo(int Id_platillo)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "SELECT * FROM `platillos` WHERE `ID_PLATILLO` = @idPla";
                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.Parameters.AddWithValue("@idPla", Id_platillo);

                    oConexion.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Platillos pla = new Platillos();
                            pla.Id_platillo = Convert.ToInt32(dr["ID_PLATILLO"]);
                            pla.Id_menu = Convert.ToInt32(dr["ID_MENU"]);
                            pla.Nombre = dr["NOMBRE"].ToString();
                            pla.Descripcion = dr["DESCRIPCION"].ToString();
                            pla.precio = Convert.ToDouble(dr["PRECIO"]);
                            pla.Imagen = dr["IMAGEN_PLATILLO"].ToString();
                            pla.Tipo = dr["TIPO"].ToString();

                            return pla;
                        }
                    }
                    oConexion.Close();

                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // Método para obtener la ruta de la imagen del platillo
        public string ObtenerRutaImagen(int Id_platillo)
        {
            try
            {
                string Imagen;
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "SELECT `IMAGEN_PLATILLO` FROM `platillos` WHERE `ID_PLATILLO` = @id_platillo ";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_platillo", Id_platillo);

                    oConexion.Open();
                    

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read()) 
                        {
                            Imagen = dr["IMAGEN_PLATILLO"].ToString();
                        }
                        else
                        {
                            Imagen = null;
                        }

                    }

                    oConexion.Close();
                    return Imagen;
                }
            }
            catch
            {
                return null;
            }
        }


    }
}
