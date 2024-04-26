using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Platillos
    {
        public int Id_platillo { get; set; }
        public int Id_menu { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public double precio { get; set; }
        public string Imagen { get; set; }

    }
}
