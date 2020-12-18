using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Net;
using NATURALLIFE.Models;
using System.Threading.Tasks;

namespace NATURALLIFE.Controllers
{
    public class HomeController : Controller
    {
        private BAULEPROFOLIOEntities2 db = new BAULEPROFOLIOEntities2();

        public static string getGUID()
        {
            string rs = "";
            char replace = '-';
            char to = '_';
            try
            {
                rs = Guid.NewGuid().ToString();
                rs = rs.Replace(replace, to);
            }
            catch (Exception ex)
            {
                string mess = ex.Message.ToString();
            }
            return rs;
        }
        
        [ChildActionOnly]
        public ActionResult HOME()
        {
            var data = (from s in db.HOMEs select s).ToList();
            return PartialView("_HOME", data);
        }

        public async Task<ActionResult> MODEL()
        {
            var data = (from s in db.TRANGMODELs select s).ToList();
            return PartialView("_MODEL", data);
        }
        public async Task<ActionResult> RECENTPOSTMODEL()
        {
            var data = (from s in db.TRANGMODELs select s).ToList();
            return PartialView("_RECENTPOSTMODEL", data);
        }

        
        public async Task<ActionResult> MODELDETAIL1(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANGMODEL tRANGMODEL = await db.TRANGMODELs.FindAsync(id);
            if (tRANGMODEL == null)
            {
                return HttpNotFound();
            }
            return View(tRANGMODEL);
        }

        //------------trang gallary----------//

        public async Task<ActionResult> GALLARY()
        {
            var data = (from s in db.THUVIENANHs select s).ToList();
            return PartialView("_GALLARY", data);
        }
        public async Task<ActionResult> TAGGALLARY()
        {
            var data = (from s in db.THELOAITHUVIENs select s).ToList();
            return PartialView("_TAGGALLARY", data);
        }
        //------------trang blog-------------//
        public async Task<ActionResult> BLOG()
        {
            var data = (from s in db.TINTUCs select s).ToList();
            return PartialView("_BLOG", data);
        }
        public async Task<ActionResult> RECENTPOSTBLOG()
        {
            var data = (from s in db.TINTUCs select s).ToList();
            return PartialView("_RECENTPOSTBLOG", data);
        }

        
        public async Task<ActionResult> BLOGDETAIL1(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TINTUC tINTUC = await db.TINTUCs.FindAsync(id);
            if (tINTUC == null)
            {
                return HttpNotFound();
            }
            return View(tINTUC);
        }

        //=----------trang LOGO KHÁCH HÀNG------------//
        public async Task<ActionResult> LOGOCLIENT()
        {
            var data = (from s in db.LOGOKHACHHANGs select s).ToList();
            return PartialView("_LOGOCLIENT", data);
        }

        //--------------- trang LIÊN HỆ---------------//
        public async Task<ActionResult> CONTACTPAGE()
        {
            var data = (from s in db.LIENHEs select s).ToList();
            return PartialView("_CONTACTPAGE", data);
        }

        public async Task<ActionResult> CONTACTFOOT()
        {
            var data = (from s in db.LIENHEs select s).ToList();
            return PartialView("_CONTACTFOOT", data);
        }

        //---------logo chan trang-----------//
        public async Task<ActionResult> LOGOFOOTPAGE()
        {
            var data = (from s in db.CHANTRANGs select s).ToList();
            return PartialView("_LOGOFOOTPAGE", data);
        }

        //--------TRANG PHOTO---------///
        public async Task<ActionResult> PHOTOPAGE()
        {
            var data = (from s in db.Photos select s).ToList();
            return PartialView("_PHOTOPAGE", data);
        }

        //----------trang đường link
        public async Task<ActionResult> LINKPAGE()
        {
            var data = (from s in db.MUCLINKs select s).ToList();
            return PartialView("_LINKPAGE", data);
        }

        public async Task<ActionResult> LINKPAGETOP()
        {
            var data = (from s in db.MUCLINKs select s).ToList();
            return PartialView("_LINKPAGETOP", data);
        }

        public async Task<ActionResult> TRANGNHAP()
        {
            var data = (from s in db.DSKHACHHANGs select s).ToList();
            return PartialView("_TRANGNHAP", data);
        }
        public ActionResult NHANTHONGTIN()
        {
            return View();
        }

        // POST: DSKHACHHANGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NHANTHONGTIN([Bind(Include = "ID,TENKH,GMAIL")] DSKHACHHANG dSKHACHHANG)
        {
            dSKHACHHANG.ID = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.DSKHACHHANGs.Add(dSKHACHHANG);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dSKHACHHANG);
        }
        //public PartialViewResult HOME()
        //{
        //    return PartialView("_HOME");
        //}
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult BlogDetail()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ModelDetail()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Footer()
        {
            ViewBag.Message = "Your contact page.";

            return PartialView("_Footer");

        }
      
    }
}