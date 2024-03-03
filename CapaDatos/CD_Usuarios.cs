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

                    MySqlCommand cmd=new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using(MySqlDataReader dr= cmd.ExecuteReader())
                    {
                        while(dr.Read())
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
                                ) ;
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
            try
            {
                string contrasenaHA256 = GetSHA256Hash(oUsuario.Contrasena);
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "INSERT INTO `restaurantes` " +
                                 "(`NOMBRE`, `APELLIDO`, `TELEFONO`, `SEXO`, `CORREO`,`CONTRASENA`, `RESTAURANTE`, `UBICACION_RESTAURANTE`, `RESTABLECER`, `FEC_REGISTRO` ) " +
                                 "VALUES (@nombre, @apellido, @telefono, @sexo, @correo, @contrasena, @restaurante, @ubicacion, @restablecer, @fecha)";

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
                    cmd.ExecuteNonQuery();
                    oConexion.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Error al insertar usuario: " + ex.Message);
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
