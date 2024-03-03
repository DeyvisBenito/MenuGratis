using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {

        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Sexo { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Restaurante { get; set; }
        public int Ubicacion { get; set; }
        public bool Restablecer { get; set; }
        public DateTime fec_registro { get; set; }

        public string confirmarContrasena { get; set; }
    }
}
