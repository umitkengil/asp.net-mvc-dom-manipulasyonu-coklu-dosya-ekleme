using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bes.Models.BesEntity;
using Bes.Models;
using System.IO;
using System.Web.Services;


namespace Bes.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        BESEntities _db = new BESEntities();

        public ActionResult Index()
        {
            var list = _db.invoiceTable.ToList();
            ViewBag.magazaAdedi = ((from adet in _db.storeTable where adet.storeCode != null select adet)).Count();
            ViewBag.kullaniciSayisi = ((from adet in _db.userTable select adet)).Count();
            ViewBag.nakitGelirler = ((from adet in _db.invoiceTable where adet.cash >=0 || adet.cash<0 || adet.cash == null select adet)).DefaultIfEmpty().Sum(x => x.cash??0);
            ViewBag.krediGelirler = ((from adet in _db.invoiceTable where adet.credit >=0 || adet.credit<0 || adet.credit == null select adet)).DefaultIfEmpty().Sum(x => x.credit??0);
            ViewBag.toplamMasraf = ((from adet in _db.invoiceTable where adet.amount >=0 || adet.amount<0 || adet.amount == null select adet)).DefaultIfEmpty().Sum(x => x.amount??0);
            return View(list);
        }
        public ActionResult AddStore()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStore(storeTable magaza)
        {

            if (!ModelState.IsValid)
            {
                return View("AddStore");
            }

            _db.storeTable.Add(magaza); ;
            _db.SaveChanges();

            return RedirectToAction("StoreIndex");
        }
        public ActionResult StoreIndex()
        {
            var list = _db.storeTable.ToList();
            return View(list);
        }
        public ActionResult EditStore(int id)
        {
            var edit = _db.storeTable.Find(id);
            return View("EditStore", edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStore(storeTable magaza)
        {
            var guncelle = _db.storeTable.Find(magaza.storeID);

            guncelle.storeCode = magaza.storeCode;
            guncelle.storeName = magaza.storeName;
            guncelle.storeDescription = magaza.storeDescription;

            _db.SaveChanges();
            return RedirectToAction("StoreIndex");
        }
        public ActionResult ExpenceIndex()
        {
            var list = _db.expenceTable.ToList();
            return View(list);
        }
        public ActionResult AddExpence()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpence(expenceTable expence)
        {
            if (!ModelState.IsValid)
            {

                return View("AddExpence");
            }

            _db.expenceTable.Add(expence);
            _db.SaveChanges();

            return RedirectToAction("ExpenceIndex");

        }
        public ActionResult EditExpence(int id)
        {
            var edit = _db.expenceTable.Find(id);
            return View("EditExpence", edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExpence(expenceTable expence)
        {
            expenceTable edit = _db.expenceTable.Find(expence.expenceID);

            edit.expenceCode = expence.expenceCode;
            edit.expenceDescription = expence.expenceDescription;

            _db.SaveChanges();
            return RedirectToAction("ExpenceIndex");
        }
        public ActionResult AddBank()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBank(bankTable bank)
        {
            if (!ModelState.IsValid)
            {
                return View("AddBank");
            }

            _db.bankTable.Add(bank);
            _db.SaveChanges();

            return RedirectToAction("BankIndex");
        }
        public ActionResult EditBank(int id)
        {
            var edit = _db.bankTable.Find(id);
            return View("EditBank", edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBank(bankTable bank)
        {
            bankTable edit = _db.bankTable.Find(bank.bankID);

            edit.bankCode = bank.bankCode;
            edit.bankName = bank.bankName;
            edit.bankDescription = bank.bankDescription;


            _db.SaveChanges();
            return RedirectToAction("BankIndex");
        }
        public ActionResult BankIndex()
        {
            var list = _db.bankTable.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult AddInvoice()
        {
            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeName,
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
        public JsonResult AddInvoice(List<data> datas)
        {
            if (datas == null)
            {
                return Json("HATA");
            }
            else
            {
                invoiceTable invoiceTable = new invoiceTable();

                string storecode = datas[0].store_id;
                invoiceTable.store_id = _db.storeTable.FirstOrDefault(p => p.storeName == storecode).storeID;

                string expencecode = datas[0].expence_id;
                invoiceTable.expence_id = _db.expenceTable.FirstOrDefault(p => p.expenceDescription == expencecode).expenceID;

                string bankcode = datas[0].bank_id;
                invoiceTable.bank_id = _db.bankTable.FirstOrDefault(p => p.bankDescription == bankcode).bankID;

                string pesincode = datas[0].odemeTuruTablosu_id;
                invoiceTable.odemeTuruTablosu_id = _db.odemeTuruTablosu.FirstOrDefault(p => p.odemeTur == pesincode).ID;

                invoiceTable.amount = Convert.ToInt32(datas[0].amonut);
                invoiceTable.cash = Convert.ToInt32(datas[0].cash);
                invoiceTable.credit = Convert.ToInt32(datas[0].credit);
                invoiceTable.invoiceDate = Convert.ToDateTime(datas[0].invoiceDate);
                invoiceTable.expenceDescription = datas[0].expenceDescription;
                invoiceTable.taksitSayisi = Convert.ToByte(datas[0].taksitSayisi);

                invoiceTable.user_id= Convert.ToInt32(Session["ID"]);

                _db.invoiceTable.Add(invoiceTable);
                _db.SaveChanges();
                return Json("");
            }
        }

        [HttpGet]
        public ActionResult EditInvoice(int id)
        {
            invoiceTable edit = _db.invoiceTable.FirstOrDefault(p => p.invoiceID == id);


            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeName,
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
                                                   Text = deger.bankName,
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

            int invoieid = Convert.ToInt32(invoice.invoiceID);
            invoiceTable edit = _db.invoiceTable.FirstOrDefault(p=>p.invoiceID==invoieid);
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

            //    taksit_kodu = _db.taksitTable.Where(m => m.taksitID == invoice.taksitTable.taksitID).FirstOrDefault();

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
            invoiceLogTable.Admin = true;
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
            //edit.odemeTaksit_id = taksit_kodu.taksitID != 13 ? taksit_kodu.taksitID : 0;
            edit.invoiceDate = invoice.invoiceDate;

            ViewBag.magazaKodu = magaza_kodu;
            ViewBag.masrafKodu = masraf_kodu;
            ViewBag.bankaKodu = banka_kodu;
            //ViewBag.odemeTipPesin = pesin_kodu;
            //ViewBag.odemeTipTaksit = taksit_kodu;
            _db.SaveChanges();


            //var magaza_kodu = _db.storeTable.Where(m => m.storeID == invoice.storeTable.storeID).FirstOrDefault();
            //var masraf_kodu = _db.expenceTable.Where(m => m.expenceID == invoice.expenceTable.expenceID).FirstOrDefault();
            //var banka_kodu = _db.bankTable.Where(m => m.bankID == invoice.bankTable.bankID).FirstOrDefault();
            //var odemeTuru = _db.odemeTuruTablosu.Where(m => m.ID == invoice.odemeTuruTablosu.ID).FirstOrDefault();

            //edit.store_id = magaza_kodu.storeID;
            //edit.bank_id = banka_kodu.bankID;
            //edit.expence_id = masraf_kodu.expenceID;
            //edit.cash = invoice.cash == null ? 0 : invoice.cash;
            //edit.credit = invoice.credit == null ? 0 : invoice.credit;
            //edit.amount = invoice.amount == null ? 0 : invoice.amount;
            //edit.expenceDescription = invoice.expenceDescription;
            //edit.odemeTuruTablosu_id = odemeTuru.ID;
            //edit.taksitSayisi = Convert.ToByte(invoice.taksitSayisi);
            //edit.user_id = Convert.ToInt32(Session["ID"]);

            //ViewBag.magazaKodu = magaza_kodu;
            //ViewBag.masrafKodu = masraf_kodu;
            //ViewBag.bankaKodu = banka_kodu;
            //ViewBag.odemeTipPesin = odemeTuru;


            //_db.SaveChanges();


            //invoiceLogTable.UserId = Convert.ToInt32(Session["ID"]);
            //invoiceLogTable.UpdateTime = DateTime.Now;
            //invoiceLogTable.invoiceId = edit.invoiceID;
            //invoiceLogTable.newstore_id = magaza_kodu.storeID;
            //invoiceLogTable.store_id = edit.store_id;
            //invoiceLogTable.newbank_id = banka_kodu.bankID;
            //invoiceLogTable.bank_id = edit.bank_id;
            //invoiceLogTable.newexpence_id = masraf_kodu.expenceID;
            //invoiceLogTable.expence_id = edit.expence_id;
            //invoiceLogTable.newcash = invoice.cash == null ? 0 : invoice.cash;
            //invoiceLogTable.cash = edit.cash;
            //invoiceLogTable.newcredit = invoice.credit == null ? 0 : invoice.credit;
            //invoiceLogTable.credit = edit.credit;
            //invoiceLogTable.newamount = invoice.amount == null ? 0 : Convert.ToDecimal(invoice.amount);
            //invoiceLogTable.amount = Convert.ToDecimal(edit.amount);
            //invoiceLogTable.newinvoiceDate = invoice.invoiceDate;
            //invoiceLogTable.invoiceDate = edit.invoiceDate;
            //invoiceLogTable.Admin = true;
            //invoiceLogTable.UserName = Session["Name"].ToString();

            //_db.invoiceLogTable.Add(invoiceLogTable);

            return RedirectToAction("InvoiceIndex");

        }
        public ActionResult InvoiceIndex()
        {
            var list = _db.invoiceTable.ToList();
            return View(list);
        }
        public ActionResult UsersIndex()
        {
            //var list = _db.userTable.ToList();
            var list = (from i in _db.userTable where i.store_id != null select i).ToList();
            return View(list);
        }
        public ActionResult AddUsers()
        {
            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeCode,
                                                    Value = deger.storeID.ToString()
                                                }).ToList();

            List<SelectListItem> user_role = ((from deger in _db.roleTable.ToList()
                                               where deger.roleID > 1
                                               select new SelectListItem
                                               {
                                                   Text = (deger.roleName).ToString(),
                                                   Value = (deger.roleID).ToString()
                                               })).ToList();
            ViewBag.magazaKodu = magaza_kodu;
            ViewBag.role = user_role;

            return View();
        }

        [HttpPost]
        public JsonResult AddUsers(List<dataUser> dataUsers)
        {
            if (dataUsers == null)
            {
                return Json("HATA");
            }
            else
            {
                userTable userTable = new userTable();

                string storecode = dataUsers[0].store_id;
                userTable.store_id = _db.storeTable.FirstOrDefault(p => p.storeCode == storecode).storeID;

                string role = dataUsers[0].role_id;
                //userTable.role_id = _db.roleTable.FirstOrDefault(p => p.roleName == role).roleID;
                userTable.role_id = _db.roleTable.Where(p => p.roleName != "admin").FirstOrDefault().roleID;
                //var list = (from i in _db.userTable where i.store_id != null select i).ToList();

                userTable.username = dataUsers[0].username;
                userTable.password = dataUsers[0].password;
                //userTable.role_id = dataUsers[0].role_id;
                userTable.user_position = dataUsers[0].user_position;

                _db.userTable.Add(userTable);
                _db.SaveChanges();
                return Json("");
            }
        }
        public ActionResult EditUsers(int id)
        {
            var edit = _db.userTable.Find(id);

            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeName,
                                                    Value = deger.storeID.ToString()
                                                }).ToList();

            List<SelectListItem> role = (from deger in _db.roleTable.ToList()
                                         select new SelectListItem
                                         {
                                             Text = deger.roleName,
                                             Value = deger.roleID.ToString()
                                         }).ToList();
            ViewBag.magazaKodu = magaza_kodu;
            ViewBag.role = role;
            return View("EditUsers", edit);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsers(userTable userTable)
        {
            userTable edit = _db.userTable.Find(userTable.userID);

            var magaza_kodu = _db.storeTable.Where(m => m.storeID == userTable.storeTable.storeID).FirstOrDefault();

            edit.store_id = magaza_kodu.storeID;
            edit.username = userTable.username;
            edit.password = userTable.password;
            edit.user_position = userTable.user_position;

            ViewBag.magazaKodu = magaza_kodu;

            _db.SaveChanges();

            return RedirectToAction("UsersIndex");
        }
        public ActionResult FileIndex()
        {
            var result = _db.FileTables.ToList();
            return View(result);
        }
        public ActionResult FileDownload(int fileid)
        {
            var result = _db.FileTables.FirstOrDefault(p => p.fileID == fileid);
            string path = Server.MapPath("~/Uploads/files");

            byte[] fileBytes = System.IO.File.ReadAllBytes(path + @"\" + result.fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, result.fileName);

        }
        public ActionResult Reports()
        {
            DateTime bastar = new DateTime(1900, 12, 01);
            DateTime bittar = new DateTime(2100, 12, 31);
            var result = _db.raporHazirlamaSP(bastar, bittar).ToList();
            List<Rapor1> listRapor = new List<Rapor1>();
            foreach (var item in result)
            {
                Rapor1 rapor1 = new Rapor1();
                rapor1.magazaAdi = item.Mağaza_Adı;
                rapor1.kart = (decimal)item.Banka_Kredi_Kartı;
                rapor1.masraf = (decimal)item.Masraf;
                rapor1.durum = (decimal)item.Durum;
                rapor1.nakit = (decimal)item.Nakit;
                listRapor.Add(rapor1);
            }
            return View(listRapor);
        }
        public ActionResult FilterReports(DateTime start, DateTime end)
        {
            var result = _db.raporHazirlamaSP(start, end).ToList();
            List<Rapor1> listRapor = new List<Rapor1>();
            foreach (var item in result)
            {
                Rapor1 rapor1 = new Rapor1();
                rapor1.magazaAdi = item.Mağaza_Adı;
                rapor1.kart = (decimal)item.Banka_Kredi_Kartı;
                rapor1.masraf = (decimal)item.Masraf;
                rapor1.durum = (decimal)item.Durum;
                rapor1.nakit = (decimal)item.Nakit;
                listRapor.Add(rapor1);
            }
            return PartialView("_rapor1Partial", listRapor);
        }
        public ActionResult GetLog()
        {
            var result = _db.vwInvoiceLog.ToList();
            return View(result);
        }
        public ActionResult ExpenceReports()
        {
            DateTime bastar = new DateTime(1900, 12, 01);
            DateTime bittar = new DateTime(2100, 12, 31);
            var result = _db.masrafRaporHazirlamaSP(bastar, bittar).ToList();
            List<MasrafRapor> listMasrafRapor = new List<MasrafRapor>();
            foreach (var item in result)
            {
                MasrafRapor masrafRapor = new MasrafRapor();
                masrafRapor.magazaAdi = item.Mağaza_Adı;
                masrafRapor.masrafKodu = item.Masraf_Kodu;
                masrafRapor.masrafAciklama = item.Masraf_Açıklama;
                masrafRapor.masraf = (decimal)item.Masraf;
                listMasrafRapor.Add(masrafRapor);
            }
            return View(listMasrafRapor);
        }
        public ActionResult ExpenceFilterReports(DateTime start, DateTime end)
        {
            var result = _db.masrafRaporHazirlamaSP(start, end).ToList();
            List<MasrafRapor> listMasrafRapor = new List<MasrafRapor>();
            foreach (var item in result)
            {
                MasrafRapor masrafRapor = new MasrafRapor();
                masrafRapor.magazaAdi = item.Mağaza_Adı;
                masrafRapor.masrafKodu = item.Masraf_Kodu;
                masrafRapor.masrafAciklama = item.Masraf_Açıklama;
                masrafRapor.masraf = (decimal)item.Masraf;

                listMasrafRapor.Add(masrafRapor);
            }
            return PartialView("_raporMasrafPartial", listMasrafRapor);
        }
        public ActionResult SummaryExpenceReports()
        {
            List<SelectListItem> magaza_kodu = (from deger in _db.storeTable.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = deger.storeCode,
                                                    Value = deger.storeID.ToString()
                                                }).ToList();

            ViewBag.magazaKodu = magaza_kodu;

            DateTime bastar = new DateTime(1900, 12, 01);
            DateTime bittar = new DateTime(2100, 12, 31);


            //var result = _db.ozetMasrafRaporHazirlamaSP(bastar, bittar, store_id).ToList();

            List<OzetMasraf> listOzetMasrafRapor = new List<OzetMasraf>();

            //foreach (var item in result)
            //{
            //    OzetMasraf ozetMasrafRapor = new OzetMasraf();
            //    ozetMasrafRapor.magazaAdi = item.Mağaza_Adı;
            //    ozetMasrafRapor.masrafAciklama = item.Masraf_Açıklama;
            //    ozetMasrafRapor.masraf = (decimal)item.Masraf;

            //    listOzetMasrafRapor.Add(ozetMasrafRapor);
            //}
            return View(listOzetMasrafRapor);
        }
        
        [HttpPost]
        public ActionResult SummaryExpenceFilterReports(DateTime start, DateTime end, int store_id)
        {
            var result = _db.ozetMasrafRaporHazirlamaSP(start, end, store_id).ToList();
            List<OzetMasraf> listOzetMasrafRapor = new List<OzetMasraf>();
            foreach (var item in result)
            {
                OzetMasraf ozetMasrafRapor = new OzetMasraf();
                ozetMasrafRapor.magazaAdi = item.Mağaza_Adı;
                ozetMasrafRapor.masrafAdi = item.Masraf_Adı;
                ozetMasrafRapor.masrafAciklama = item.Masraf_Açıklama;

                ozetMasrafRapor.masraf = (decimal)item.Masraf;

                listOzetMasrafRapor.Add(ozetMasrafRapor);
            }
            return PartialView("_raporOzetMasrafPartial", listOzetMasrafRapor);
        }
        public ActionResult Logout()
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

        [HttpPost]
        public JsonResult SessionLogout(int id)
        {
            int userid = Convert.ToInt32(Session["ID"]);
            var result = _db.userTable.FirstOrDefault(p => p.userID == userid);
            result.isOnline = false;
            result.logoutDate = DateTime.Now;
            _db.SaveChanges();
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

    }
    public class data
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
    public class dataUser
    {
        public string store_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role_id { get; set; }
        public string user_position { get; set; }

    }

}