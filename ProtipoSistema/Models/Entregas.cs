using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtipoSistema.Models
{
    public class Entregas
    {
        public string descripcion { get; set; }
        public string tipo { get; set; }
        public string fecha { get; set; }
        public string estado { get; set; }
        public string id_cliente { get; set; }
        public string id_vehiculo_conductor { get; set; }


    }
}