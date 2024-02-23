using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using MySql.Data.MySqlClient;
using System.Data;

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

        
    }
}
