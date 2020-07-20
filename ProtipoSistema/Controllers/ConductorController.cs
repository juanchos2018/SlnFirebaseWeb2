using Firebase.Auth;
using Firebase.Database.Query;
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
    public class ConductorController : Controller
    {
        // GET: Conductor
        IFirebaseClient client;
        Conexion conexion;
        
        private static string Bucket = "fir-app-cf755.appspot.com";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetConductores()
        {
            conexion = new Conexion();
            client = new FireSharp.FirebaseClient(conexion.conec());
            FirebaseResponse response = client.Get("Conductores");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Conductor> lista = new List<Conductor>();
            if (data==null)
            {
                lista = null;
            }
            else
            {
                foreach (var item in data)
                {
                    lista.Add(JsonConvert.DeserializeObject<Conductor>(((JProperty)item).Value.ToString()));
                }
                return Json(lista, JsonRequestBehavior.AllowGet);
            }           
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> CreateConductor()
        {
            Conductor conductor = new Conductor();

            if (Request.Files.Count > 0)
            {
                try
                {
                    FileStream stream;
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    conductor.nombres_conductor = HttpContext.Request.Params["nombres_conductor"];
                    conductor.apellido_conductor = HttpContext.Request.Params["apellido_conductor"];
                    conductor.correo_conductor = HttpContext.Request.Params["correo_conductor"];
                    conductor.licencia_conductor = HttpContext.Request.Params["licencia_conductor"];
                    conductor.celular_conductor = HttpContext.Request.Params["celular_conductor"];
                    conductor.clave_conductor = HttpContext.Request.Params["clave_conductor"];
                    conductor.estado_conductor = HttpContext.Request.Params["estado_conductor"];
                    conductor.fecha_creacion = DateTime.Now.ToShortDateString();
                   
                    string fileName = file.FileName;
                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), file.FileName);
                    file.SaveAs(path);
                    stream = new FileStream(Path.Combine(path), FileMode.Open);
                    Directory.CreateDirectory(Server.MapPath("~/uploads/"));
                    Task task = Task.Run(() => Upload(stream, conductor, file.FileName));                 
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
           
            return Json(conductor, JsonRequestBehavior.AllowGet);
        }
        public async Task<bool> Upload(FileStream stream, Conductor obj, string filenanme)
        {
            conexion = new Conexion();       
            var auth = new FirebaseAuthProvider(new FirebaseConfig(conexion.Firekey()));
            var a = await auth.SignInWithEmailAndPasswordAsync(conexion.AthEmail(), conexion.AthPassword());
     
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
                .Child("FotosConductores")
                .Child(filenanme)
                .PutAsync(stream, cancellation.Token);
            try
            {
                string link = await task;               
                obj.rutafoto_conductor = link;
                Task tarea2 = Task.Run(() => SignUp(obj));
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }
            return true;
        }

        public async Task<ActionResult> SignUp(Conductor o)
        {
            try
            {
                conexion = new Conexion();
                var auth = new FirebaseAuthProvider(new FirebaseConfig(conexion.Firekey()));
                var a = await auth.CreateUserWithEmailAndPasswordAsync(o.correo_conductor, o.clave_conductor, o.nombres_conductor, true);
         
                var id = a.User.LocalId;  //para tener el id del usuario que esta registrado we :V
                client = new FireSharp.FirebaseClient(conexion.conec());
                var data = o;
                PushResponse response = client.Push("Conductores/", data);
                data.id_conductor = id;
                SetResponse setResponse = client.Set("Conductores/" + data.id_conductor, data);
                ModelState.AddModelError(string.Empty, "verifica tu coreo" +  "ids" + id);
              //  AddsUsuarios(o, id);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View();
        }

        public ActionResult Get(int id)
        {
            //  var product = _dbContext.Product.ToList().Find(x => x.Id == id);
            //return Json(product, JsonRequestBehavior.AllowGet);
            return View();
        }


        public async Task<ActionResult> GetBuscaConductor(string id)
        {
            //string id = "mRe3vsjLrnapslgaMWWqKDVBphJ3";

            var firebase = new Firebase.Database.FirebaseClient("https://fir-app-cf755.firebaseio.com/");
            var conductores = (await firebase
              .Child("Conductores")
              .OrderByKey()
              .OnceAsync<Conductor>()).Where(x=>x.Object.id_conductor==id).ToList();

            var conductores2 = (await firebase
             .Child("Conductores")
             .OrderByKey()
             .OnceAsync<Conductor>()).ToList().Find(x=>x.Object.id_conductor==id);

            var lista = new List<Conductor>();
           
          //  return Json(lista, JsonRequestBehavior.AllowGet);

            var list = new List<Conductor>();
            //  var query=list.Where()   .OnceAsync<ClsUsuarios>()).Where(a => a.Object.cargo_usuario == "Abogado").ToList(); // parabuscar segun el tipo
            foreach (var dino in conductores)     
            {
                Conductor o = new Conductor();
                o.nombres_conductor = dino.Object.nombres_conductor;             
                list.Add(o);
            }
            //   return View(list);
            string ser = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(conductores2);
            return Json(conductores2, JsonRequestBehavior.AllowGet);


        }
    }
}