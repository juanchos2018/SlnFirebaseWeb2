using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtipoSistema.Models
{
    public class Conductor
    {
        public string id_conductor { get; set; }
        public string nombres_conductor { get; set; }
        public string apellido_conductor { get; set; }
        public string correo_conductor { get; set; }
        public string licencia_conductor { get; set; }
        public string celular_conductor { get; set; }
        public string clave_conductor { get; set; }
        public string estado_conductor { get; set; }
        public string rutafoto_conductor { get; set; }
        public double lat_conductor { get; set; }
        public double lon_conductor { get; set; }
       public string fecha_creacion { get; set; }


    }
}