using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bes.Models;
using Bes.Models.BesEntity;

namespace Bes.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        BESEntities _db = new BESEntities();
        public ActionResult Index()
        {

            int id = Convert.ToInt32(Session["ID"]);
            int storeid = Convert.ToInt32(Session["storeId"]);
            var tarih = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(-3);
            var list = _db.invoiceTable.Where(v => v.store_id == storeid && v.invoiceDate>=tarih).ToList();

            ViewBag.kullaniciSayisi = ((from adet in _db.userTable where adet.store_id == storeid select adet)).Count();
            ViewBag.nakitGelirler = ((from adet in _db.invoiceTable where adet.invoiceDate>=tarih && adet.store_id == storeid select adet)).DefaultIfEmpty().Sum(x => x.cash ?? 0);
            ViewBag.krediGelirler = ((from adet in _db.invoiceTable where adet.invoiceDate >= tarih && adet.store_id == storeid  select adet)).DefaultIfEmpty().Sum(x => x.credit??0);
            ViewBag.toplamMasraf = ((from adet in _db.invoiceTable where adet.invoiceDate >= tarih && adet.store_id == storeid select adet)).DefaultIfEmpty().Sum(x => x.amount??0);

            ViewBag.magazaAdi = ((from magaza in _db.storeTable where magaza.storeID == storeid select magaza.storeName)).FirstOrDefault();

            return View(list);
        }
        public ActionResult LogOut(string id)
        {
            int userid = Convert.ToInt32(Session["ID"]);
            var result = _db.userTable.FirstOrDefault(p => p.userID == userid);
            result.isOnline = false;
            result.logoutDate = DateTime.Now;
            _db.SaveChanges();
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Security");
        }

        //[HttpPost]
        //public JsonResult LogoutJS()
        //{
        //    int userid = Convert.ToInt32(Session["ID"]);

        //    var result = _db.userTable.FirstOrDefault(p => p.userID == userid);
        //    result.isOnline = false;
        //    _db.SaveChanges();
        //    return Json("", JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public JsonResult SessionLogout()
        {
            int userid = Convert.ToInt32(Session["ID"]);
            var result = _db.userTable.FirstOrDefault(p => p.userID == userid);
            result.isOnline = false;
            result.logoutDate = DateTime.Now;
            _db.SaveChanges();
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult InvoiceIndex()
        {
            //int id = Convert.ToInt32(Session["ID"]);
            //int storeid = Convert.ToInt32(Session["storeId"]);
            //var store = _db.storeTable.Where(v => v.storeID == storeid).Select(b => b.storeID).FirstOrDefault();
            //var list = _db.invoiceTable.Where(v => v.store_id == store).ToList();


            int store_id = Convert.ToInt32(Session["storeId"]);
            var list = _db.invoiceTable.Where(p => p.store_id == store_id).ToList();
            ViewBag.LoginUserId = Convert.ToInt32(Session["ID"]);

            return View(list);
        }

        [HttpGet]
        public ActionResult AddInvoice()
        {
            int storeid = Convert.ToInt32(Session["storeId"]);

            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.Where(p => p.storeID == storeid).ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeCode,
                                                    Value = deger.storeID.ToString()
                                                }).ToList();

            List<SelectListItem> masraf_kodu = (from deger in _db.expenceTable.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.expenceDescription,
                                                    Value = deger.expenceID.ToString()
                                                }).ToList();

            List<SelectListItem> banka_kodu = (from deger in _db.bankTable.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = deger.bankDescription,
                                                   Value = deger.bankID.ToString()
                                               }).ToList();

            List<SelectListItem> odeme_tip = (from deger in _db.odemeTuruTablosu.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = deger.odemeTur,
                                                  Value = deger.ID.ToString()
                                              }).ToList();


            ViewBag.magazaKodu = magaza_kodu;
            ViewBag.masrafKodu = masraf_kodu;
            ViewBag.bankaKodu = banka_kodu;
            ViewBag.odemeTipPesin = odeme_tip;

            return View();
        }

        [HttpPost]
        public JsonResult AddInvoice(List<addUserData> data2)
        {
            if (data2 == null)
            {
                return Json("HATA");
            }
            else
            {


                invoiceTable invoiceTable = new invoiceTable();

                string storecode = data2[0].store_id;
                invoiceTable.store_id = _db.storeTable.FirstOrDefault(p => p.storeCode == storecode).storeID;

                string expencecode = data2[0].expence_id;
                invoiceTable.expence_id = _db.expenceTable.FirstOrDefault(p => p.expenceDescription == expencecode).expenceID;

                string bankcode = data2[0].bank_id;
                invoiceTable.bank_id = _db.bankTable.FirstOrDefault(p => p.bankDescription == bankcode).bankID;

                string pesincode = data2[0].odemeTuruTablosu_id;
                invoiceTable.odemeTuruTablosu_id = _db.odemeTuruTablosu.FirstOrDefault(p => p.odemeTur == pesincode).ID;


                invoiceTable.amount = Convert.ToDecimal(data2[0].amonut);
                invoiceTable.cash = Convert.ToDecimal(data2[0].cash);
                invoiceTable.credit = Convert.ToDecimal(data2[0].credit);
                invoiceTable.expenceDescription = data2[0].expenceDescription;
                invoiceTable.invoiceDate = Convert.ToDateTime(data2[0].invoiceDate);
                invoiceTable.taksitSayisi = Convert.ToByte(data2[0].taksitSayisi);

                invoiceTable.user_id = Convert.ToInt32(Session["ID"]);

                _db.invoiceTable.Add(invoiceTable);
                _db.SaveChanges();
                return Json("");
            }
        }
        public ActionResult EditInvoice(int id)
        {

            invoiceTable edit = _db.invoiceTable.FirstOrDefault(p => p.invoiceID == id);

            int storeid = edit.store_id;


            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.Where(p => p.storeID == storeid).ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeCode,
                                                    Value = deger.storeID.ToString()
                                                }).ToList();

            List<SelectListItem> masraf_kodu = (from deger in _db.expenceTable.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.expenceBool == true ? ("-" + deger.expenceDescription) : deger.expenceDescription,
                                                    Value = deger.expenceID.ToString()
                                                }).ToList();

            List<SelectListItem> banka_kodu = (from deger in _db.bankTable.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = deger.bankDescription,
                                                   Value = deger.bankID.ToString()
                                               }).ToList();

            List<SelectListItem> odeme_tip = (from deger in _db.odemeTuruTablosu.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = deger.odemeTur,
                                                  Value = deger.ID.ToString()
                                              }).ToList();

            ViewBag.magazaKodu = magaza_kodu;
            ViewBag.masrafKodu = masraf_kodu;
            ViewBag.bankaKodu = banka_kodu;
            ViewBag.odemeTipPesin = odeme_tip;

            return View("EditInvoice", edit);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInvoice(invoiceTable invoice)
        {

            invoiceTable edit = _db.invoiceTable.Find(invoice.invoiceID);
            invoiceLogTable invoiceLogTable = new invoiceLogTable();
            storeTable magaza_kodu = new storeTable();
            expenceTable masraf_kodu = new expenceTable();
            bankTable banka_kodu = new bankTable();
            //pesinTable pesin_kodu = new pesinTable();
            //taksitTable taksit_kodu = new taksitTable();


            magaza_kodu = _db.storeTable.FirstOrDefault(m => m.storeID == invoice.store_id);
            masraf_kodu = _db.expenceTable.FirstOrDefault(m => m.expenceID == invoice.expence_id);
            banka_kodu = _db.bankTable.FirstOrDefault(m => m.bankID == invoice.bank_id);
            //pesin_kodu = _db.pesinTable.FirstOrDefault(m => m.pesinID == invoice.pesinTable.pesinID);
            //if (invoice.pesinTable.pesinID != 1)
            //    taksit_kodu = _db.taksitTable.FirstOrDefault(m => m.taksitID == invoice.taksitTable.taksitID);


            invoiceLogTable.UserId = Convert.ToInt32(Session["ID"]);
            invoiceLogTable.UpdateTime = DateTime.Now;
            invoiceLogTable.invoiceId = edit.invoiceID;
            invoiceLogTable.newstore_id = magaza_kodu.storeID;
            invoiceLogTable.store_id = edit.store_id;
            invoiceLogTable.newbank_id = banka_kodu.bankID;
            invoiceLogTable.bank_id = edit.bank_id;
            invoiceLogTable.newexpence_id = masraf_kodu.expenceID;
            invoiceLogTable.expence_id = edit.expence_id;
            invoiceLogTable.newcash = invoice.cash == null ? 0 : invoice.cash;
            invoiceLogTable.cash = edit.cash;
            invoiceLogTable.newcredit = invoice.credit == null ? 0 : invoice.credit;
            invoiceLogTable.credit = edit.credit;
            invoiceLogTable.newamount = invoice.amount == null ? 0 : Convert.ToDecimal(invoice.amount);
            invoiceLogTable.amount = Convert.ToDecimal(edit.amount);
            invoiceLogTable.newinvoiceDate = invoice.invoiceDate;
            invoiceLogTable.invoiceDate = edit.invoiceDate;
            invoiceLogTable.Admin = false;
            invoiceLogTable.UserName = HttpContext.User.Identity.Name.ToString();

            _db.invoiceLogTable.Add(invoiceLogTable);

            edit.store_id = magaza_kodu.storeID;
            edit.bank_id = banka_kodu.bankID;
            edit.expence_id = masraf_kodu.expenceID;
            edit.cash = invoice.cash == null ? 0 : invoice.cash;
            edit.credit = invoice.credit == null ? 0 : invoice.credit;
            edit.amount = invoice.amount == null ? 0 : invoice.amount;
            edit.expenceDescription = invoice.expenceDescription;
            //edit.odemePesin_id = pesin_kodu.pesinID;
            //edit.odemeTaksit_id = taksit_kodu.taksitID == 13 ? 0 : taksit_kodu.taksitID;
            edit.invoiceDate = invoice.invoiceDate;

            ViewBag.magazaKodu = magaza_kodu;
            ViewBag.masrafKodu = masraf_kodu;
            ViewBag.bankaKodu = banka_kodu;
            //ViewBag.odemeTipPesin = pesin_kodu;
            //ViewBag.odemeTipTaksit = taksit_kodu;
            _db.SaveChanges();

            return RedirectToAction("Index");

            //var magaza_kodu = _db.storeTable.Where(m => m.storeID == invoice.storeTable.storeID).FirstOrDefault();
            //var masraf_kodu = _db.expenceTable.Where(m => m.expenceID == invoice.expenceTable.expenceID).FirstOrDefault();
            //var banka_kodu = _db.bankTable.Where(m => m.bankID == invoice.bankTable.bankID).FirstOrDefault();
            //var odemeTuru = _db.odemeTuruTablosu.Where(m => m.ID == invoice.odemeTuruTablosu.ID).FirstOrDefault();

            //edit.store_id = magaza_kodu.storeID;
            //edit.bank_id = banka_kodu.bankID;
            //edit.expence_id = masraf_kodu.expenceID;
            //edit.cash = invoice.cash;
            //edit.credit = invoice.credit;
            //edit.amount = invoice.amount;
            //edit.expenceDescription = invoice.expenceDescription;
            //edit.odemeTuruTablosu_id = odemeTuru.ID;
            //edit.taksitSayisi = Convert.ToByte(invoice.taksitSayisi);
            //edit.user_id = Convert.ToInt32(Session["ID"]);

            //_db.SaveChanges();

            //ViewBag.magazaKodu = magaza_kodu;
            //ViewBag.masrafKodu = masraf_kodu;
            //ViewBag.bankaKodu = banka_kodu;
            //ViewBag.odemeTipPesin = odemeTuru;
            //invoiceLogTable.UserId = Convert.ToInt32(Session["ID"]);
            //invoiceLogTable.UpdateTime = DateTime.Now;
            //invoiceLogTable.invoiceId = edit.invoiceID;
            //invoiceLogTable.newstore_id = magaza_kodu.storeID;
            //invoiceLogTable.store_id = edit.store_id;
            //invoiceLogTable.newbank_id = banka_kodu.bankID;
            //invoiceLogTable.bank_id = edit.bank_id;
            //invoiceLogTable.newexpence_id = masraf_kodu.expenceID;
            //invoiceLogTable.expence_id = edit.expence_id;
            //invoiceLogTable.newcash = invoice.cash;
            //invoiceLogTable.cash = edit.cash;
            //invoiceLogTable.newcredit = invoice.credit;
            //invoiceLogTable.credit = edit.credit;
            //invoiceLogTable.newamount = Convert.ToDecimal(invoice.amount);
            //invoiceLogTable.amount = Convert.ToDecimal(edit.amount);
            //invoiceLogTable.newinvoiceDate = invoice.invoiceDate;
            //invoiceLogTable.invoiceDate = edit.invoiceDate;
            //invoiceLogTable.Admin = false;
            //invoiceLogTable.UserName = Session["Name"].ToString();

            //_db.invoiceLogTable.Add(invoiceLogTable);

            //return RedirectToAction("InvoiceIndex");

        }
        public ActionResult AddFiles()
        {
            var tarih = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(-3);

            int userid = Convert.ToInt32(Session["ID"]);
            int store_id = Convert.ToInt32(Session["storeId"]);

            ViewBag.LoginUserId = Convert.ToInt32(Session["ID"]);

            var result = _db.FileTables.Where(p => p.user_id == userid && p.Status == true && p.AddedDate >= tarih).ToList();

            //var list = _db.FileTables.Where(p => p.store_id == store_id).ToList();


            return View(result);

        }

        [HttpPost]
        public ActionResult AddFiles(int id)
        {
            string fName = "";
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here
                fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));

                    string pathString = Path.Combine(originalDirectory.ToString(), "files");

                    var fileName1 = Path.GetFileName(file.FileName);

                    bool isExists = Directory.Exists(pathString);

                    if (!isExists)
                        Directory.CreateDirectory(pathString);

                    var path = string.Format("{0}\\{1}", pathString, file.FileName);
                    file.SaveAs(path);

                    FileTables fileTables = new FileTables();
                    fileTables.fileName = fName;
                    fileTables.user_id = Convert.ToInt32(Session["ID"]);
                    fileTables.store_id = Convert.ToInt32(Session["storeId"]);
                    fileTables.AddedDate = DateTime.Now;
                    fileTables.Status = true;
                    fileTables.fileURL = "-";
                    _db.FileTables.Add(fileTables);
                    _db.SaveChanges();
                }

            }

            return RedirectToAction("AddFiles");

        }

        public ActionResult FileIndex()
        {
            var tarih = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(-3);
            int userid = Convert.ToInt32(Session["ID"]);
            var result = _db.FileTables.Where(p => p.user_id == userid && p.Status == true && p.AddedDate >= tarih).ToList();
                                          
            return View(result);
        }
        public ActionResult gunSonuGelir()
        {
            int store_id = Convert.ToInt32(Session["storeId"]);
            var list = _db.invoiceTable.Where(p => p.store_id == store_id).ToList();
            ViewBag.LoginUserId = Convert.ToInt32(Session["ID"]);
            ViewBag.magazaAdi = ((from magaza in _db.storeTable where magaza.storeID == store_id select magaza.storeName)).FirstOrDefault();
            return View(list);
        }
        public ActionResult gunSonuGider()
        {
            int store_id = Convert.ToInt32(Session["storeId"]);
            var list = _db.invoiceTable.Where(p => p.store_id == store_id && p.expence_id !=1).ToList();
            ViewBag.LoginUserId = Convert.ToInt32(Session["ID"]);
            ViewBag.magazaAdi = ((from magaza in _db.storeTable where magaza.storeID == store_id select magaza.storeName)).FirstOrDefault();
            return View(list);
        }
        public ActionResult GunsonuIndex()
        {
            int store_id = Convert.ToInt32(Session["storeId"]);
            var list = _db.invoiceTable.Where(p => p.store_id == store_id).ToList();
            ViewBag.LoginUserId = Convert.ToInt32(Session["ID"]);
            ViewBag.magazaAdi = ((from magaza in _db.storeTable where magaza.storeID == store_id select magaza.storeName)).FirstOrDefault();
            return View(list);
        }
        public ActionResult FileDownload(int fileid)
        {
            var result = _db.FileTables.FirstOrDefault(p => p.fileID == fileid);
            string path = Server.MapPath("~/Uploads/files");

            byte[] fileBytes = System.IO.File.ReadAllBytes(path + @"\" + result.fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, result.fileName);

        }
        public ActionResult DeleteFile(int fileid)
        {
            var result = _db.FileTables.FirstOrDefault(p => p.fileID == fileid);
            result.Status = false;
            result.DeletedDate = DateTime.Now;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ResfundIndex()
        {
            int store_id = Convert.ToInt32(Session["storeId"]);
            var list = _db.invoiceTable.Where(p => p.store_id == store_id).ToList();
            ViewBag.LoginUserId = Convert.ToInt32(Session["ID"]);

            return View(list);
        }

        [HttpGet]
        public ActionResult AddResfund()
        {
            int storeid = Convert.ToInt32(Session["storeId"]);

            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.Where(p => p.storeID == storeid).ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeCode,
                                                    Value = deger.storeID.ToString()
                                                }).ToList();

            List<SelectListItem> banka_kodu = (from deger in _db.bankTable.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = deger.bankDescription,
                                                   Value = deger.bankID.ToString()
                                               }).ToList();

            List<SelectListItem> iade_tipi = (from deger in _db.resfundTypeTable.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = deger.resfundTypeName,
                                                  Value = deger.resfundTypeID.ToString()
                                              }).ToList();

            List<SelectListItem> user_kodu = (from deger in _db.userTable.Where(p => p.store_id == storeid).ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.username,
                                                    Value = deger.userID.ToString()
                                                }).ToList();

            ViewBag.magazaKodu = magaza_kodu;
            ViewBag.userKodu = user_kodu;
            ViewBag.bankaKodu = banka_kodu;
            ViewBag.iadeTuru = iade_tipi;

            return View();
        }

        [HttpPost]
        public JsonResult AddResfund(List<addRefundData> data3)
        {
            if (data3 == null)
            {
                return Json("HATA");
            }
            else
            {
                resfundTable resfundTable = new resfundTable();

                string storecode = data3[0].store_id;
                resfundTable.store_id = _db.storeTable.FirstOrDefault(p => p.storeCode == storecode).storeID;

                string usercode = data3[0].user_id;
                resfundTable.user_id = _db.userTable.FirstOrDefault(p => p.username == usercode).userID;

                string bankcode = data3[0].bank_id;
                resfundTable.bank_id = _db.bankTable.FirstOrDefault(p => p.bankDescription == bankcode).bankID;

                string iadeTuru = data3[0].refundType;
                resfundTable.resfundType = _db.resfundTypeTable.FirstOrDefault(p => p.resfundTypeName == iadeTuru).resfundTypeID;

                resfundTable.resfundCash = Convert.ToDecimal(data3[0].refundCash);
                resfundTable.resfundCredit = Convert.ToDecimal(data3[0].refundCredit);
                resfundTable.resfundDescription = data3[0].refundDescription;
                resfundTable.resfundDate = Convert.ToDateTime(data3[0].refundDate);
                resfundTable.user_id = Convert.ToInt32(Session["ID"]);

                _db.resfundTable.Add(resfundTable);
                _db.SaveChanges();

                return Json("");
            }
        }
    }
    public class addUserData
    {
        public string store_id { get; set; }
        public string user_id { get; set; }
        public string expence_id { get; set; }
        public string bank_id { get; set; }
        public string cash { get; set; }
        public string credit { get; set; }
        public string amonut { get; set; }
        public string invoiceDate { get; set; }
        public string expenceDescription { get; set; }
        public string odemeTuruTablosu_id { get; set; }
        public string taksitSayisi { get; set; }
    }

    public class addRefundData
    {
        public string store_id { get; set; }
        public string user_id { get; set; }
        public string bank_id { get; set; }
        public string refundCash { get; set; }
        public string refundCredit { get; set; }
        public string refundType { get; set; }
        public string refundDate { get; set; }
        public string refundDescription { get; set; }
    }

}