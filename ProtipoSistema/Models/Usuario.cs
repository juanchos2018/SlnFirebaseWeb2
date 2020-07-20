using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtipoSistema.Models
{
    public class Usuario
    {
        public string id_usuario { get; set; }
        public string nombre_usuario { get; set; }
        public string apellido_usuario { get; set; }
        public string dni_usuario { get; set; }
        public string correo_usuario { get; set; }
        public string clave_usuario { get; set; }
    }
}