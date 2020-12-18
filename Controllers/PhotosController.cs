using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NATURALLIFE.Models;

namespace NATURALLIFE.Controllers
{
    public class PhotosController : Controller
    {
        private BAULEPROFOLIOEntities2 db = new BAULEPROFOLIOEntities2();

        // GET: Photos
        public async Task<ActionResult> Index()
        {
            return View(await db.Photos.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHOTO pHOTO = await db.Photos.FindAsync(id);
            if (pHOTO == null)
            {
                return HttpNotFound();
            }
            return View(pHOTO);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SOTHUTU,HINHANH")] PHOTO pHOTO)
        {
            if (ModelState.IsValid)
            {
                db.Photos.Add(pHOTO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pHOTO);
        }

        // GET: Photos/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHOTO pHOTO = await db.Photos.FindAsync(id);
            if (pHOTO == null)
            {
                return HttpNotFound();
            }
            return View(pHOTO);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,SOTHUTU,HINHANH")] PHOTO pHOTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pHOTO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pHOTO);
        }

        // GET: Photos/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHOTO pHOTO = await db.Photos.FindAsync(id);
            if (pHOTO == null)
            {
                return HttpNotFound();
            }
            return View(pHOTO);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            PHOTO pHOTO = await db.Photos.FindAsync(id);
            db.Photos.Remove(pHOTO);
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
