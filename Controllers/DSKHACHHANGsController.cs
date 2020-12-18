using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NATURALLIFE.Controllers;
using NATURALLIFE.Models;
namespace NATURALLIFE.Controllers
{
    public class DSKHACHHANGsController : Controller
    {
        private BAULEPROFOLIOEntities2 db = new BAULEPROFOLIOEntities2();

        // GET: DSKHACHHANGs
        public async Task<ActionResult> Index()
        {
            return View(await db.DSKHACHHANGs.ToListAsync());
        }

        // GET: DSKHACHHANGs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSKHACHHANG dSKHACHHANG = await db.DSKHACHHANGs.FindAsync(id);
            if (dSKHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(dSKHACHHANG);
        }

        // GET: DSKHACHHANGs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DSKHACHHANGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,TENKH,GMAIL")] DSKHACHHANG dSKHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.DSKHACHHANGs.Add(dSKHACHHANG);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dSKHACHHANG);
        }

        // GET: DSKHACHHANGs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSKHACHHANG dSKHACHHANG = await db.DSKHACHHANGs.FindAsync(id);
            if (dSKHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(dSKHACHHANG);
        }

        // POST: DSKHACHHANGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TENKH,GMAIL")] DSKHACHHANG dSKHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dSKHACHHANG).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dSKHACHHANG);
        }

        // GET: DSKHACHHANGs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSKHACHHANG dSKHACHHANG = await db.DSKHACHHANGs.FindAsync(id);
            if (dSKHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(dSKHACHHANG);
        }

        // POST: DSKHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            DSKHACHHANG dSKHACHHANG = await db.DSKHACHHANGs.FindAsync(id);
            db.DSKHACHHANGs.Remove(dSKHACHHANG);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
