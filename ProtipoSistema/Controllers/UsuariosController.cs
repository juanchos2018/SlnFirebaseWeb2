using Firebase.Auth;
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

namespace ProtipoSistema.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios         

        IFirebaseClient client;
        Conexion conexion;
       // private static string key1 = "AIzaSyASBVEzU8ZqFHtMgdYW7-66ZQjrZGf-lAc";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUsuarios()
        {
            conexion = new Conexion();
            client = new FireSharp.FirebaseClient(conexion.conec());
            FirebaseResponse response = client.Get("Usuarios");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Usuario> lista = new List<Usuario>();
            foreach (var item in data)
            {
                lista.Add(JsonConvert.DeserializeObject<Usuario>(((JProperty)item).Value.ToString()));
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateUsuario(Usuario o)
        {
            //para crear auntenticacion de correo electronico we
            try
            {
                conexion = new Conexion();
                var auth = new FirebaseAuthProvider(new FirebaseConfig(conexion.Firekey()));
                var a = await auth.CreateUserWithEmailAndPasswordAsync(o.correo_usuario, o.clave_usuario, o.nombre_usuario, true);
                var token = a.User.FederatedId;
                var id = a.User.LocalId;  //para tener el id del usuario que esta registrado we :V                        

                ModelState.AddModelError(string.Empty, "verifica tu coreo" + " token" + token + " ids" + id);
                AddsUsuarios(o, id);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }
        private void AddsUsuarios(Usuario collection, string idcurrent_user)
        {
            conexion = new Conexion();
            client = new FireSharp.FirebaseClient(conexion.conec());
            var data = collection;
            PushResponse response = client.Push("Usuarios/", data);            
            data.id_usuario = idcurrent_user;
            SetResponse setResponse = client.Set("Usuarios/" + data.id_usuario, data);

        }

        [HttpPost]
        public ActionResult Create(Usuario user)
        {
            conexion = new Conexion();
            client = new FireSharp.FirebaseClient(conexion.conec());
            var data = user;
            PushResponse response = client.Push("Usuarios/", data);
            data.id_usuario = response.Result.name;
            SetResponse setResponse = client.Set("Usuarios/" + data.id_usuario, data);
            return Json(user, JsonRequestBehavior.AllowGet);


        }
    }
}