using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtipoSistema.Models
{
    public class VehiculoConductor
    {
        public string id_vehiculo_conductor { get; set; }
        public string id_conductor { get; set; }
        public string id_vehiculo { get; set; }
        public string nombre_conductor { get; set; }
        public string nombre_vehiculo { get; set; }
        public string fecha { get; set; }
        public string estado { get; set; }

    }
}