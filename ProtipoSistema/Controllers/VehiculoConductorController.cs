using Firebase.Database.Query;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtipoSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ProtipoSistema.Controllers
{
    public class VehiculoConductorController : Controller
    {
        // GET: VehiculoConductor
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "4OUpc4rGJTetaxG8IBiE3uoXdSVNuBeRdJoo8Uju",
            BasePath = "https://fir-app-cf755.firebaseio.com/"

        };

        IFirebaseClient client;
        ViewModelVehiculoConductor model = new ViewModelVehiculoConductor();
        Conexion conexion;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetConductores()
        {
            conexion = new Conexion();
            client = new FireSharp.FirebaseClient(conexion.conec());
            FirebaseResponse response = client.Get("VehiculoConductor2");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<VehiculoConductor> lista = new List<VehiculoConductor>();

            foreach (var item in data)
            {
                lista.Add(JsonConvert.DeserializeObject<VehiculoConductor>(((JProperty)item).Value.ToString()));
            }

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult addVehiculoConductor()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Vehiculos");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Vehiculo>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Vehiculo>(((JProperty)item).Value.ToString()));

            }

            FirebaseResponse conductor = client.Get("Conductores");
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(conductor.Body);
            var lista2 = new List<Conductor>();

            foreach (var item in data2)
            {
                lista2.Add(JsonConvert.DeserializeObject<Conductor>(((JProperty)item).Value.ToString()));
            }

            model.ListaVehiculos = list;
            model.ListaConductor = lista2;
            return View(model);
        
        }

        public async Task <ActionResult> Asignar(VehiculoConductor obj)
        {
           // FirebaseHelper firebaseHelper = new FirebaseHelper();
            VehiculoConductor o = new VehiculoConductor();
            o.id_conductor = obj.id_conductor;
            o.nombre_conductor = obj.nombre_conductor;
            o.id_vehiculo = obj.id_vehiculo;
            o.nombre_vehiculo = obj.nombre_vehiculo;
            o.estado = obj.estado;
            
            o.fecha = DateTime.Now.ToShortDateString();
           // client = new FireSharp.FirebaseClient(config);
           // var data = obj;
            //PushResponse response = client.Push("VehiculoConductor/", data);
            //data.id_conductor = obj.id_conductor;
            string id = obj.id_conductor;
          //  var key= response.Result.name;
        //       SetResponse setResponse = client.Set("VehiculoConductor/" + data.id_vehiculo, data);
            await Add(o, id);
            return Json(o, JsonRequestBehavior.AllowGet);
        }
        public async Task Add(VehiculoConductor obj,string id)
        {
            var data = obj;
            client = new FireSharp.FirebaseClient(config);
            var firebase = new Firebase.Database.FirebaseClient("https://fir-app-cf755.firebaseio.com/");
          //  PushResponse response = client.Push("VehiculoConductor/", data);
        //    var key = response.Result.name;
            await firebase
              .Child("VehiculoConductor").Child(id)
              .PostAsync(new VehiculoConductor() { id_conductor = obj.id_conductor, id_vehiculo = obj.id_vehiculo });

            await firebase
             .Child("VehiculoConductor2")
             .PostAsync(new VehiculoConductor() { id_conductor = obj.id_conductor,nombre_conductor= obj.nombre_conductor, id_vehiculo = obj.id_vehiculo ,nombre_vehiculo=obj.nombre_vehiculo,fecha=DateTime.Now.ToShortDateString(),estado=obj.estado});
        }
    }
}