using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtipoSistema.Models
{
    public class Cliente
    {
        public string id_cliente { get; set; }
        public string nombre_cliente { get; set; }
        public string apellido_cliente { get; set; }
        public string correo_cliente { get; set; }
      //  public string direccion_cliente { get; set; }
        public double lat_cliente { get; set; }
        public double log_cliente { get; set; }
        public string rutafoto_cliente { get; set; }
        public string estado_cliente { get; set; }
        public string fecha_creacion { get; set; }


    }
}