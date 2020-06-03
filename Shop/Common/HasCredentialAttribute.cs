using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
namespace Shop.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userSession = (UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            if (userSession == null)
            {
                return false;
            }
            List<string> privilegeLevels = this.GetCredentialByLoggedInUser();
            if (privilegeLevels.Contains(this.RoleID) || userSession.GroupID == Common.CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<string> GetCredentialByLoggedInUser() 
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
            };
        }



    }
}