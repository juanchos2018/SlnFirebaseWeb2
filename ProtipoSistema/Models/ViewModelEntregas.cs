using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtipoSistema.Models
{
    public class ViewModelEntregas
    {
        public List<Cliente> ListaClientes { get; set; }
        public List<VehiculoConductor> ListaVehiculoConductor { get; set; }
    }
}