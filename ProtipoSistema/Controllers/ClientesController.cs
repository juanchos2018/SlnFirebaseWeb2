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
    public class ClientesController : Controller
    {
        // GET: Clientes
        /*
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "4OUpc4rGJTetaxG8IBiE3uoXdSVNuBeRdJoo8Uju",
            BasePath = "https://fir-app-cf755.firebaseio.com/"

        };  

        IFirebaseClient client;
        */
        IFirebaseClient client;
        Conexion conexion;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetClientes()
        {
            conexion = new Conexion();
            client = new FireSharp.FirebaseClient(conexion.conec());
           // client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("ClientesAsp");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Cliente> lista1 = new List<Cliente>();
            foreach (var item in data)
            {
                lista1.Add(JsonConvert.DeserializeObject<Cliente>(((JProperty)item).Value.ToString()));

            }
            return Json(lista1, JsonRequestBehavior.AllowGet);
        }
    }
}