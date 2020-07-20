using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtipoSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProtipoSistema.Controllers
{
    public class EntregasController : Controller
    {
        // GET: Entregas
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "4OUpc4rGJTetaxG8IBiE3uoXdSVNuBeRdJoo8Uju",
            BasePath = "https://fir-app-cf755.firebaseio.com/"

        };

        IFirebaseClient client;
        Conexion conexion;
         ViewModelEntregas model = new ViewModelEntregas();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AgregarEntrega()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("VehiculoConductor2");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<VehiculoConductor>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<VehiculoConductor>(((JProperty)item).Value.ToString()));

            }

            FirebaseResponse conductor = client.Get("ClientesAsp");
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(conductor.Body);
            var lista2 = new List<Cliente>();

            foreach (var item in data2)
            {
                lista2.Add(JsonConvert.DeserializeObject<Cliente>(((JProperty)item).Value.ToString()));
            }
            model.ListaVehiculoConductor = list;
            model.ListaClientes = lista2;
            return View(model);
        }

        public ActionResult add(Entregas o)
        {

            return View();
        }
    }
}