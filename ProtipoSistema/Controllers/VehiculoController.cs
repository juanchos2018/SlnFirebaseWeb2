using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtipoSistema.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProtipoSistema.Controllers
{
    public class VehiculoController : Controller
    {
        // GET: Vehiculo

        IFirebaseClient client;
        Conexion conexion;
        private static string AuthEmail = "storage@gmail.com";
        private static string AuthPassword = "2014049452";
        private static string Bucket = "fir-app-cf755.appspot.com";
        private static string ApiKey = "AIzaSyASBVEzU8ZqFHtMgdYW7-66ZQjrZGf-lAc";

        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "4OUpc4rGJTetaxG8IBiE3uoXdSVNuBeRdJoo8Uju",
            BasePath = "https://fir-app-cf755.firebaseio.com/"

        };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetVehiculos()
        {
            conexion = new Conexion();
            client = new FireSharp.FirebaseClient(conexion.conec());
            FirebaseResponse response = client.Get("Vehiculos");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Vehiculo> lista = new List<Vehiculo>();
        
                foreach (var item in data)
                {
                    lista.Add(JsonConvert.DeserializeObject<Vehiculo>(((JProperty)item).Value.ToString()));
                }
           
          
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    

        public async Task<ActionResult> Create2()
        {
            Vehiculo vehiculo = new Vehiculo();
            
            if (Request.Files.Count > 0)
            {
                try
                {
                    FileStream stream;
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    var placa = HttpContext.Request.Params["placa_vehiculo"];
                    var marca = HttpContext.Request.Params["marca_vehiculo"];
                    var modelo = HttpContext.Request.Params["modelo_vehiculo"];
                    var color = HttpContext.Request.Params["color_vehiculo"];
                    var anio = HttpContext.Request.Params["anio_vehiculo"];
                    var estado = HttpContext.Request.Params["estado_vehiculo"];

                    vehiculo.placa_vehiculo = placa;
                    vehiculo.marca_vehiculo = marca;
                    vehiculo.modelo_vehiculo = modelo;
                    vehiculo.color_vehiculo = color;
                    vehiculo.anio_vehiculo = anio;
                    vehiculo.estado_vehiculo = estado;
                    // string recibe = Request.Params["placa_vehiculo"];
                    string fileName = file.FileName;
                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), file.FileName);
                    file.SaveAs(path);
                    stream = new FileStream(Path.Combine(path), FileMode.Open);
        
                    //await Task.Run(() => Upload(stream, vehiculo, file.FileName));

                    Directory.CreateDirectory(Server.MapPath("~/uploads/"));
                    Task task   =  Task.Run(() => Upload (stream, vehiculo, file.FileName));                
                  //  Thread.Sleep(4000);
                   task.Wait();
                    object es = task.Status;
                    if (task.IsCompleted)
                    {
                        return Json(es);
                    }  
                    
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            // return Json("no files were selected !");
            return Json(vehiculo, JsonRequestBehavior.AllowGet);
        }

        public async Task<bool> Upload(FileStream stream,Vehiculo obj, string filenanme)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            // you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
                .Child("FotosVehiculos")
                .Child(filenanme)
                .PutAsync(stream, cancellation.Token);
            try
            {
                string link = await task;
                /*  Vehiculo o = new Vehiculo();
                  o.placa_vehiculo = obj.placa_vehiculo;
                  o.marca_vehiculo = obj.marca_vehiculo;
                  o.modelo_vehiculo = obj.modelo_vehiculo;
                  o.color_vehiculo = obj.color_vehiculo;
                  o.anio_vehiculo = obj.anio_vehiculo;
                  o.estado_vehiculo = obj.estado_vehiculo;
                  o.rutafoto_vehiculo = link;
                  AddVehiuclo(o);
                  */
                obj.rutafoto_vehiculo = link;
                client = new FireSharp.FirebaseClient(config);
                var data = obj;
                PushResponse response = client.Push("Vehiculos/", data);
                data.id_vehiculo = response.Result.name;
                SetResponse setResponse = client.Set("Vehiculos/" + data.id_vehiculo, data);
               
            }
           
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }
            return true;
        }

      
    }
}