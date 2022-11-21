using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services;
using Bes.Models.BesEntity;

namespace Bes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        //protected void Application_BeginRequest()
        //{
        //    //NOTE: Stopping IE from being a caching whore
        //    HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
        //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    HttpContext.Current.Response.Cache.SetNoStore();
        //    Response.Cache.SetExpires(DateTime.Now);
        //    Response.Cache.SetValidUntilExpires(true);
        //}
        protected void Session_OnEnd(object sender, EventArgs e)
        {
            using (BESEntities _db = new BESEntities())
            {
                int userid = Convert.ToInt32(Session["ID"]);
                var result = _db.userTable.FirstOrDefault(p => p.userID == userid);
                result.isOnline = false;
                result.logoutDate = DateTime.Now;
                _db.SaveChanges();
            }
        }

        [WebMethod]
        public static void AbandonSession()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
}
