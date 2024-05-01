using CapaEntidad;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Secciones
    {
        public List<Secciones> listarSec(int id)
        {
            List<Secciones> list = new List<Secciones>();

            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "SELECT `ID_SE`, `NOMBRE` FROM `secciones` WHERE `ID_RES` = @id_res ";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_res", id);

                    oConexion.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(
                                new Secciones()
                                {
                                    Id_se = Convert.ToInt32(dr["ID_SE"]),
                                    Nombre = dr["NOMBRE"].ToString(),
                                    Id_res=id,

                                }
                            );

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Registra cualquier otra excepción para diagnosticar el problema
                Console.WriteLine("Error: " + ex.Message);
            }

            return list;
        }

        public bool Agregar(Secciones sec)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "INSERT INTO `secciones` " +
                                 "( `NOMBRE`, `ID_RES` ) " +
                                 "VALUES ( @nombre, @id_res )";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@nombre", sec.Nombre);
                    cmd.Parameters.AddWithValue("@id_res", sec.Id_res);



                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    oConexion.Close();
                    return true;
                }


            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public Secciones buscarSeccion(int id)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "SELECT * FROM `secciones` WHERE `ID_SE` = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.Parameters.AddWithValue("@id", id);

                    oConexion.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Secciones oSec = new Secciones();
                            oSec.Id_se = Convert.ToInt32(dr["ID_SE"]);
                            oSec.Nombre = dr["NOMBRE"].ToString();
                            oSec.Id_res = Convert.ToInt32(dr["ID_RES"]);
                            return (oSec);
                        }
                    }
                    oConexion.Close();
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public bool editarSeccion(Secciones sec)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "UPDATE `secciones` " +
                        "SET `NOMBRE` = @nombre " +
                        "WHERE `ID_SE` = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.Parameters.AddWithValue("@nombre", sec.Nombre);
                    cmd.Parameters.AddWithValue("@id", sec.Id_se);

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

        public bool eliminarSeccion(int Id_se)
        {
            try
            {
                using (MySqlConnection oConexion = new MySqlConnection(Conexion.cn))
                {
                    string sql = "DELETE FROM `secciones` " +
                                 " WHERE `ID_SE` = @id ";

                    MySqlCommand cmd = new MySqlCommand(sql, oConexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id", Id_se);

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
    }
}
