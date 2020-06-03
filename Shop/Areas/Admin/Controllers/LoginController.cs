using Models.Dao;
using Shop.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Common;


namespace Shop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {               
                var dao = new UserDao();
                var res = dao.Login(loginModel.UserName, Encryptor.MD5Hash(loginModel.Password),true);
                if (res == 1)
                {
                    var user = dao.GetUserByUsername(loginModel.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredential(loginModel.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS,listCredentials);

                    Session.Add(CommonConstants.USER_SESSION,userSession);
                    return RedirectToAction("Index","Home");
                }
                else if(res == 0){
                    ModelState.AddModelError("", "Login Fail - Account is not exist");
                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Login Fail - Account is blocked");
                }
                else if (res == -2)
                {
                    ModelState.AddModelError("", "Login Fail - Password is not correct");
                }
                else if (res == -3)
                {
                    ModelState.AddModelError("", "Login Fail - Access Denied.");
                }
                else
                {
                    ModelState.AddModelError("","Login Fail");

                }
            }
          
            return View("Index");
        }
    }
}