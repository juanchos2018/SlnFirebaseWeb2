using Firebase.Database.Query;
using FireSharp;
using ProtipoSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProtipoSistema.Controllers
{
    public class CarpetasController : Controller
    {
        // GET: Carpetas
        Carpetas o = new Carpetas();
       // public ActionResult Index()
        //{
          //  return View(o);
       // }

        public async Task<ActionResult> Index()
        {
            string id = LoginController.id_user;


            var firebase = new Firebase.Database.FirebaseClient("https://fir-app-cf755.firebaseio.com/");
            var dinos = await firebase
              .Child("Archivos").Child(id)
              .OrderByKey()
              .OnceAsync<Carpetas>();

            
            var list = new List<Carpetas>();
            foreach (var dino in dinos)
            {

                Carpetas o = new Carpetas();
                o.id_carpeta = dino.Object.id_carpeta;
                o.nombre_carpeta = dino.Object.nombre_carpeta;
                list.Add(o);

            }
            return View(list);


        }

    }
}