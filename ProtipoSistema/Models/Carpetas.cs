using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtipoSistema.Models
{
    public class Carpetas
    {
       
        public   string id_carpeta { get; set; }
        public string nombre_carpeta { get; set; }
        public string fecha_creacion { get; set; }
        public string cantidad_archivos { get; set; }
    }
}