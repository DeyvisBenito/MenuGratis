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

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn)) {
                    string sql = "SELECT `ID`, `NOMBRE`, `APELLIDO`, `TELEFONO`, `SEXO`, `CORREO`, `CONTRASENA`, `RESTAURANTE`, `UBICACION_RESTAURANTE`, `RESTABLECER` FROM `restaurantes`";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Usuario()
                                {
                                    ID = Convert.ToInt32(dr["ID"]),
                                    Nombre = dr["NOMBRE"].ToString(),
                                    Apellido = dr["APELLIDO"].ToString(),
                                    Telefono = dr["TELEFONO"].ToString(),
                                    Sexo = dr["SEXO"].ToString(),
                                    Correo = dr["CORREO"].ToString(),
                                    Contrasena = dr["CONTRASENA"].ToString(),
                                    Restaurante = dr["RESTAURANTE"].ToString(),
                                    Ubicacion = Convert.ToInt32(dr["UBICACION_RESTAURANTE"]),
                                    Restablecer = Convert.ToBoolean(dr["RESTABLECER"])
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Usuario>();
            }

            return lista;
        }

        public int InsertarUbicaciom(Ubicacion oUbicacion) //Haciendo insercion de la ubicacion del restaurante a la BD
        {
            int ultimoID = 0;
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "INSERT INTO `ubicaciones` " +
                                 "(`PAIS`, `DEPARTAMENTO`, `MUNICIPIO`, `ZONA`, `CALLE`) " +
                                 "VALUES (@pais, @departamento, @municipio, @zona, @calle); SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@pais", oUbicacion.Pais);
                    cmd.Parameters.AddWithValue("@departamento", oUbicacion.Departamento);
                    cmd.Parameters.AddWithValue("@municipio", oUbicacion.Municipio);
                    cmd.Parameters.AddWithValue("@zona", oUbicacion.Zona);
                    cmd.Parameters.AddWithValue("@calle", oUbicacion.Calle);

                    oConexion.Open();
                    ultimoID = Convert.ToInt32(cmd.ExecuteScalar());
                    oConexion.Close();

                    return ultimoID;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Error al insertar usuario: " + ex.Message);
            }
            return ultimoID;
        }

        public void InsertarUsuario(Usuario oUsuario, int idUbicacion) //Haciendo insercion del usuario a la base de datos
        {
            int ultimoIDr = 0;
            try
            {
                string contrasenaHA256 = GetSHA256Hash(oUsuario.Contrasena);
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "INSERT INTO `restaurantes` " +
                                 "(`NOMBRE`, `APELLIDO`, `TELEFONO`, `SEXO`, `CORREO`,`CONTRASENA`, `RESTAURANTE`, `UBICACION_RESTAURANTE`, `RESTABLECER`, `FEC_REGISTRO` ) " +
                                 "VALUES (@nombre, @apellido, @telefono, @sexo, @correo, @contrasena, @restaurante, @ubicacion, @restablecer, @fecha); SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", oUsuario.Apellido);
                    cmd.Parameters.AddWithValue("@telefono", oUsuario.Telefono);
                    cmd.Parameters.AddWithValue("@sexo", oUsuario.Sexo);
                    cmd.Parameters.AddWithValue("@correo", oUsuario.Correo);
                    cmd.Parameters.AddWithValue("@contrasena", contrasenaHA256);
                    cmd.Parameters.AddWithValue("@restaurante", oUsuario.Restaurante);
                    cmd.Parameters.AddWithValue("@ubicacion", idUbicacion);
                    cmd.Parameters.AddWithValue("@restablecer", oUsuario.Restablecer);
                    cmd.Parameters.AddWithValue("@fecha", oUsuario.fec_registro);

                    oConexion.Open();
                    ultimoIDr = Convert.ToInt32(cmd.ExecuteScalar());
                    InsertarMenu(ultimoIDr, oUsuario.Restaurante);
                    oConexion.Close();

                }
            }
            catch (Exception ex)
            {

            }

        }


        public void InsertarMenu(int id, string rest) //Haciendo insercion del menu
        {
            try
            {
                string nombre = string.Concat(rest, "_menu");

                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "INSERT INTO `menus` " +
                                 "(`ID_RES`, `NOMBRE`, `FECHA_INICIO` ) " +
                                 "VALUES (@id_res, @nombre, @fecha)";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_res", id);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    oConexion.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public int BuscarMenu(int id) //Buscando el menu dependiendo el Id del restaurante
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "SELECT * FROM `menus` WHERE `ID_RES` = @idRes";
                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.Parameters.AddWithValue("@idRes", id);

                    oConexion.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Menus oMenus = new Menus();
                            oMenus.Id_menu = Convert.ToInt32(dr["ID_MENU"]);
                            oMenus.Id_res = Convert.ToInt32(dr["ID_RES"]);
                            oMenus.Nombre = dr["NOMBRE"].ToString();
                            
                            return oMenus.Id_menu;
                        }
                    }
                    oConexion.Close();

                }
                return 0;
            }
            catch
            {
                return 0;
            }

            
        }

        public bool ActualizarUsuario(Usuario oUsuario)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "UPDATE `restaurantes` " +
                    "SET `NOMBRE` = @nombre, " +
                    "`APELLIDO` = @apellido, " +
                    "`TELEFONO` = @telefono, " +
                    "`SEXO` = @sexo, " +
                    "`RESTAURANTE` = @restaurante " +
                    "WHERE `ID` = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", oUsuario.Apellido);
                    cmd.Parameters.AddWithValue("@telefono", oUsuario.Telefono);
                    cmd.Parameters.AddWithValue("@sexo", oUsuario.Sexo);
                    cmd.Parameters.AddWithValue("@restaurante", oUsuario.Restaurante);
                    cmd.Parameters.AddWithValue("@id", oUsuario.ID);

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

        public bool ActualizarUbicacion(Ubicacion oUbicacion)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "UPDATE `ubicaciones` " +
                    "SET `PAIS` = @pais, " +
                    "`DEPARTAMENTO` = @depa, " +
                    "`MUNICIPIO` = @muni, " +
                    "`ZONA` = @zona, " +
                    "`CALLE` = @calle " +
                    "WHERE `ID_RES` = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@pais", oUbicacion.Pais);
                    cmd.Parameters.AddWithValue("@depa", oUbicacion.Departamento);
                    cmd.Parameters.AddWithValue("@muni", oUbicacion.Municipio);
                    cmd.Parameters.AddWithValue("@zona", oUbicacion.Zona);
                    cmd.Parameters.AddWithValue("@calle", oUbicacion.Calle);
                    cmd.Parameters.AddWithValue("@id", oUbicacion.Id_res);

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    oConexion.Close();

                    return true;

                }

            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Ubicacion buscarUbicacion(int id)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "SELECT * FROM `ubicaciones` WHERE `ID_RES` = @idUbicacion";
                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.Parameters.AddWithValue("@idUbicacion", id);

                    oConexion.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Ubicacion oUbicacion = new Ubicacion();
                            oUbicacion.Id_res = Convert.ToInt32(dr["ID_RES"]);
                            oUbicacion.Pais = dr["PAIS"].ToString();
                            oUbicacion.Departamento = dr["DEPARTAMENTO"].ToString();
                            oUbicacion.Municipio = dr["MUNICIPIO"].ToString();
                            oUbicacion.Zona = dr["ZONA"].ToString();
                            oUbicacion.Calle = dr["CALLE"].ToString();
                            return (oUbicacion);
                        }
                    }
                    oConexion.Close();

                }
                return null;
            }catch(Exception e)
            {
                return null;
            }
        }


    

        public string GetSHA256Hash(string input) //Convertira una contrasena en SHA256
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }



    }
}
