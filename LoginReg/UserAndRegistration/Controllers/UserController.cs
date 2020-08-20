using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using UserAndRegistration.Models;

namespace UserAndRegistration.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            if(ModelState.IsValid)
            {
                using(LoginDatabaseEntities1 lde = new LoginDatabaseEntities1())
                {
                    lde.Users.Add(user);
                    lde.SaveChanges();
                    ModelState.Clear();
                    user = null;
                    ViewBag.Message = "Registered Successfully";
                }
            }
            return View();
        }
        //Login
        [HttpGet]
        public ActionResult Login()
        {          
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user,UserLogin userLogin)
        {
           
                using(LoginDatabaseEntities1 lde = new LoginDatabaseEntities1())
                {
                    var v = lde.Users.Where(a => a.EmailID.Equals(user.EmailID) && a.Password.Equals(user.Password)).FirstOrDefault();
                    if(v!=null)
                    {
                        int timeout = userLogin.RememberMe ? 30 : 1;//30 days
                        var cookie = FormsAuthentication.GetAuthCookie(userLogin.EmailID, userLogin.RememberMe);
                        // Since they want to be remembered, set the expiration for 30 days
                        cookie.Expires = DateTime.Now.AddDays(timeout);
                        // Store the cookie in the Response
                        Response.Cookies.Add(cookie);
                        Session["LoggedUserID"] = user.EmailID.ToString();
                        return RedirectToAction("AfterLogin");
                    }
                    else
                    {
                        userLogin.logingErrorMessage = "Invalid UserID or Password";
                        return View("Login",userLogin);
                    }
                }
            
            
        }
        [Authorize]
        public ActionResult AfterLogin()
        {
            if(Session["LoggedUserID"]!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","User");
        }
    }
}