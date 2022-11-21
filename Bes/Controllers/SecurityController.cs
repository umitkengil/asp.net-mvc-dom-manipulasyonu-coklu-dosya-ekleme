using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services;
using Bes.Models.BesEntity;

namespace Bes.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        // GET: Security

        BESEntities _db = new BESEntities();
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(userTable user)
        {
            var giris = _db.userTable.FirstOrDefault(c => c.username == user.username && c.password == user.password);

            //if (giris.isOnline == true)
            //{
            //    TimeSpan fark = DateTime.Now.Subtract(giris.loginDate.Value);
            //    if (fark.Minutes <= 5)
            //    {
            //        ViewBag.Message = "Bu kullanıcı sistemde online";
            //        return View();
            //    }
            //}


            if (giris != null && giris.roleTable.roleName == "admin")
            {
                if (giris.isOnline == true)
                {
                    TimeSpan fark = DateTime.Now.Subtract(giris.loginDate.Value);
                    if (fark.Minutes <= 60)
                    {
                        ViewBag.Message = "Bu kullanıcı sistemde online";
                        return View();
                    }
                }
                FormsAuthentication.SetAuthCookie(giris.username.ToString(), false);
                Session["ID"] = giris.userID;
                Session["Name"] = giris.username;
                Session["storeId"] = 0;

                giris.isOnline = true;
                giris.loginDate = DateTime.Now;
                _db.SaveChanges();

                return RedirectToAction("Index", "Admin");
            }

            else if (giris != null && giris.roleTable.roleName == "user")
            {
                if (giris.isOnline == true)
                {
                    TimeSpan fark = DateTime.Now.Subtract(giris.loginDate.Value);
                    if (fark.Minutes <= 5)
                    {
                        ViewBag.Message = "Bu kullanıcı sistemde online";
                        return View();
                    }
                }
                Session["ID"] = giris.userID;
                Session["Name"] = giris.username;
                Session["storeId"] = giris.store_id;

                giris.isOnline = true;
                giris.loginDate = DateTime.Now;
                _db.SaveChanges();
                FormsAuthentication.SetAuthCookie(giris.username.ToString(), false);
                return RedirectToAction("Index", "User");

            }
            //else if (giris == null)
            //{
            //    ViewBag.Message = "Lütfen Kullanıcı Adınızı/Şifrenizi Kontrol Ediniz...";
            //    return RedirectToAction("Page404", "Error");
            //    //return View();


            //}
            else
            {
                ViewBag.Message = "Lütfen Kullanıcı Adınızı / Şifrenizi Kontrol Ediniz...";
                return View();
            }

        }

        [HttpPost]
        public JsonResult LogoutJS()
        {
            int userid = Convert.ToInt32(Session["ID"]);

            var result = _db.userTable.FirstOrDefault(p => p.userID == userid);
            result.isOnline = false;
            _db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);

        }

    }
}