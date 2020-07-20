using Firebase.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using ProtipoSistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProtipoSistema.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private static string key1 = "AIzaSyASBVEzU8ZqFHtMgdYW7-66ZQjrZGf-lAc";

        public static string correo;
        public static string id_user;
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returUrl)
        {
            try
            {
                if (this.Request.IsAuthenticated)
                {
                    // return this.Redi
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnurl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(key1));
                    var ab = await auth.SignInWithEmailAndPasswordAsync(model.Email, model.Password);
                    var token = ab.FirebaseToken;
                    id_user = ab.User.LocalId;
                     correo= ab.User.Email;                     

                    var user = ab.User;
                    if (token != "")
                    {
                        this.SignInUser(user.Email, token, false);
                        return this.RedirecToLocal(returnurl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "invalide usrname or password");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return View(model);
        }
        private void SignInUser(string email, string token, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimValueTypes.Email, email));
                claims.Add(new Claim(ClaimTypes.Authentication, token));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var autenticateManager = ctx.Authentication;

                autenticateManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);


            }

            catch (Exception ex)

            {

                throw;
            }
        }

        private ActionResult RedirecToLocal(string retunrUrl)
        {
            try
            {
                if (Url.IsLocalUrl(retunrUrl))
                {
                    return this.Redirect(retunrUrl);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return this.RedirectToAction("Index", "Inicio");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Login");

        }

    }
}