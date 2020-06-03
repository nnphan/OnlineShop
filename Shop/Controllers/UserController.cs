using BotDetect.Web.Mvc;
using Facebook;
using Models.Dao;
using Models.EF;
using Shop.Common;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }



        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var userDao = new UserDao();
                if (userDao.CheckUserNameExist(registerModel.UserName))
                {
                    ModelState.AddModelError("","User name is existed !");
                }
                else if (userDao.CheckEmailExist(registerModel.Email))
                {
                    ModelState.AddModelError("", "Email is existed !");
                }
                else
                {
                    var user = new User();
                    user.Name = registerModel.Name;
                    user.Username = registerModel.UserName;
                    user.Password = Encryptor.MD5Hash(registerModel.Password) ;
                    user.Phone = registerModel.Phone;
                    user.Email = registerModel.Email;
                    user.Address = registerModel.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;
                    var result = userDao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Regist successfully";
                        registerModel = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("","Regist fail");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var userDao = new UserDao();
                var res = userDao.Login(loginModel.UserName,Encryptor.MD5Hash(loginModel.Password),true);
                if (res == 1)
                {
                    var user = userDao.GetUserByUsername(loginModel.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.Name;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION,userSession);
                    return Redirect("/");
                }
                else if (res == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (res == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token",new {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;

            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                var user = new User();
                user.Email = email;
                user.Username = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.Name;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }
    }
}