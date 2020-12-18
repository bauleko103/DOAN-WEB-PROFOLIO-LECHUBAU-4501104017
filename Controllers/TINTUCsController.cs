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
    public class TINTUCsController : Controller
    {
        private BAULEPROFOLIOEntities2 db = new BAULEPROFOLIOEntities2();

        // GET: TINTUCs
        public async Task<ActionResult> Index()
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI");
            var tINTUCs = db.TINTUCs.Include(t => t.THELOAITINTUC);
            return View(await tINTUCs.ToListAsync());
        }
        
        [HttpPost]
        public async Task<ActionResult> Index(string IDTHELOAI)
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI");
            var tINTUCs = db.TINTUCs.Include(t => t.THELOAITINTUC).Where(a => a.IDTHELOAI == IDTHELOAI);
            return View(await tINTUCs.ToListAsync());
        }


        // GET: TINTUCs/Details/5
        public async Task<ActionResult> Details(string id)
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

        // GET: TINTUCs/Create
        public ActionResult Create()
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI");
            return View();
        }

        // POST: TINTUCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,IDTHELOAI,TIEUDE,NOIDUNGPHU,HINHANH,NOIDUNGCHINH,NGAYTHANG")] TINTUC tINTUC)
        {
            if (ModelState.IsValid)
            {
                db.TINTUCs.Add(tINTUC);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI", tINTUC.IDTHELOAI);
            return View(tINTUC);
        }

        // GET: TINTUCs/Edit/5
        public async Task<ActionResult> Edit(string searchString)
        {
            var links = from l in db.THELOAITINTUCs // lấy toàn bộ liên kết
                        select l;
            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                links = links.Where(s => s.TENTHELOAI.Contains(searchString)); //lọc theo chuỗi tìm kiếm
            }

                           
            TINTUC tINTUC = await db.TINTUCs.FindAsync(searchString);
            if (tINTUC == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI", tINTUC.IDTHELOAI);
            return View(links);
        }

        // POST: TINTUCs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ID,IDTHELOAI,TIEUDE,NOIDUNGPHU,HINHANH,NOIDUNGCHINH,NGAYTHANG")] TINTUC tINTUC)
        //{
            
           
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tINTUC).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI", tINTUC.IDTHELOAI);
        //    return View(links);
        //}

        // GET: TINTUCs/Delete/5
        public async Task<ActionResult> Delete(string id)
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

        // POST: TINTUCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            TINTUC tINTUC = await db.TINTUCs.FindAsync(id);
            db.TINTUCs.Remove(tINTUC);
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
