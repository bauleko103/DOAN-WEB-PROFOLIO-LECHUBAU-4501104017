using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using NATURALLIFE.Models;
using System.Threading.Tasks;

namespace NATURALLIFE.Controllers
{
    public class WebmasterController : Controller
    {
        private BAULEPROFOLIOEntities2 db = new BAULEPROFOLIOEntities2();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(NATURALLIFE.Models.TAIKHOAN userModel)
        {
            using (BAULEPROFOLIOEntities2 db = new BAULEPROFOLIOEntities2())
            {
                var userDetails = db.TAIKHOANs.Where(x => x.IDTAIKHOAN == userModel.IDTAIKHOAN && x.PASS == userModel.PASS).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View( userModel);
                }
                else
                {
                    Session["userID"] = userDetails.IDTAIKHOAN;
                    Session["userName"] = userDetails.PASS;
                    return RedirectToAction("HOMEVIEW", "Webmaster");
                }
            }
        }
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

        // GET: Webmaster
        /////////PAGE HOME
        [HttpGet]
        public ActionResult HOMEVIEW()
        {
            
            if (Session["UserID"] != null)
            {
                return View(db.HOMEs.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Webmaster");
            }
        }
        public ActionResult HOMEEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOME SUAHOME = db.HOMEs.Find(id);
            if (SUAHOME == null)
            {
                return HttpNotFound();
            }
            return View(SUAHOME);
        }

        
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult HOMEEDIT([Bind(Include = "ID,TUADE1,TUADE2,TUADE3,TENNUT,HINHANHNEN,HINHANHNENFile")] HOME SUAHOME)
        {
            var dulieuHOME = new BAULEPROFOLIOEntities2(); // anh tạo 1 chuổi dulieucoán anh gọi nguyên cái DB của em lại. đổ vào đó
            var HINHANHCUAHOME = dulieuHOME.HOMEs.Single(e => e.ID == SUAHOME.ID); // sau đó anh dùng biến HINHANHcosan để hứng dữ liệu model tại nơi mà ID của nó bằng ID của thằng SUAMODEL, để lấy ra đúng cái hình
            if (SUAHOME.HINHANHNENFile == null)// nếu file nó null
            {
                SUAHOME.HINHANHNEN = HINHANHCUAHOME.HINHANHNEN; //nó sẽ fill dữ liệu từ data vào lại để edit
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(SUAHOME.HINHANHNENFile.FileName);
                string extension = Path.GetExtension(SUAHOME.HINHANHNENFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                SUAHOME.HINHANHNEN = fileName;
                fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);//coi kỹ lại chỗ này nha.
                SUAHOME.HINHANHNENFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Entry(SUAHOME).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HOMEVIEW");
            }
            return View(SUAHOME);
        }
        //--------- MODEL --------
        public async Task<ActionResult> MODELVIEW()
        {
            return View(await db.TRANGMODELs.ToListAsync());
        }

        // TRANG THÊM MODEL
        public ActionResult MODELADD()
        {
            return View();
        }
        
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult MODELADD([Bind(Include = "ID,TUADE,NOIDUNG,HINHANH,HINHANHFile")] TRANGMODEL THEMMODEL)
        {
            string fileName = Path.GetFileNameWithoutExtension(THEMMODEL.HINHANHFile.FileName);
            string extension = Path.GetExtension(THEMMODEL.HINHANHFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            THEMMODEL.HINHANH = fileName;
            fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);
            THEMMODEL.HINHANHFile.SaveAs(fileName);
            THEMMODEL.ID = getGUID().ToString();//cái này đã chỉ chưa, đã nói chưa???
            if (ModelState.IsValid)
            {
                db.TRANGMODELs.Add(THEMMODEL);
                db.SaveChanges();
                return RedirectToAction("MODELVIEW");
            }

            return View(THEMMODEL);
        }

        //TRANG SỬA MODEL
        public ActionResult MODELEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANGMODEL SUAMODEL = db.TRANGMODELs.Find(id);
            if (SUAMODEL == null)
            {
                return HttpNotFound();
            }

            return View(SUAMODEL);
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult MODELEDIT([Bind(Include = "ID,TUADE,NOIDUNG,HINHANH,HINHANHFile")] TRANGMODEL SUAMODEL)
        {
            
            var dulieucosan = new BAULEPROFOLIOEntities2(); // anh tạo 1 chuổi dulieucoán anh gọi nguyên cái DB của em lại. đổ vào đó
            var HINHANHcosan = dulieucosan.TRANGMODELs.Single(e => e.ID == SUAMODEL.ID); // sau đó anh dùng biến HINHANHcosan để hứng dữ liệu model tại nơi mà ID của nó bằng ID của thằng SUAMODEL, để lấy ra đúng cái hình
            if (SUAMODEL.HINHANHFile == null)// nếu file nó null
            {
                SUAMODEL.HINHANH = HINHANHcosan.HINHANH; //nó sẽ fill dữ liệu từ data vào lại để edit
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(SUAMODEL.HINHANHFile.FileName);
                string extension = Path.GetExtension(SUAMODEL.HINHANHFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                SUAMODEL.HINHANH = fileName;
                fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);//coi kỹ lại chỗ này nha.
                SUAMODEL.HINHANHFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Entry(SUAMODEL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MODELVIEW");
            }
            return View(SUAMODEL);
        }


        //delete model
        public ActionResult MODELDELETE(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANGMODEL MODELDEL = db.TRANGMODELs.Find(id);
            if (MODELDEL == null)
            {
                return HttpNotFound();
            }
            return View(MODELDEL);
        }

        // POST: MODELs/Delete/5
        [HttpPost, ActionName("MODELDELETE")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TRANGMODEL MODELDEL = db.TRANGMODELs.Find(id);
            db.TRANGMODELs.Remove(MODELDEL);
            db.SaveChanges();
            return RedirectToAction("MODELVIEW");
        }


        /// <summary>
        /// TRANG THU VIEN
        /// //-------------THU VIEN-------------
        public async Task<ActionResult> THUVIENVIEW()
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH");
            var tHUVIENANHs = db.THUVIENANHs.Include(t => t.THELOAITHUVIEN);
            return View(await tHUVIENANHs.ToListAsync());
        }
        [HttpPost]
        public async Task<ActionResult> THUVIENVIEW(string IDTHELOAI)
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH");
            var tHUVIENANHs = db.THUVIENANHs.Include(t => t.THELOAITHUVIEN).Where(b => b.IDTHELOAI == IDTHELOAI);
            return View(await tHUVIENANHs.ToListAsync());
        }
        
        // THU VIEN ADD
        public ActionResult THUVIENADD()
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> THUVIENADD([Bind(Include = "ID,IDTHELOAI,HINHANH,HINHANHFile")] THUVIENANH tHUVIENANH)
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH", tHUVIENANH.IDTHELOAI);
            string fileName = Path.GetFileNameWithoutExtension( tHUVIENANH.HINHANHFile.FileName);
            string extension = Path.GetExtension( tHUVIENANH.HINHANHFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            tHUVIENANH.HINHANH = fileName;
            fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);
            tHUVIENANH.HINHANHFile.SaveAs(fileName);
            tHUVIENANH.ID = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.THUVIENANHs.Add(tHUVIENANH);
                await db.SaveChangesAsync();
                return RedirectToAction("THUVIENVIEW");
            }
            
            return View(tHUVIENANH);
        }
       

        // GET: THUVIEN/Edit/5
        public async Task<ActionResult> THUVIENEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUVIENANH  tHUVIENANH = await db.THUVIENANHs.FindAsync(id);
            if ( tHUVIENANH == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH",  tHUVIENANH.IDTHELOAI);
            return View( tHUVIENANH);
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> THUVIENEDIT([Bind(Include = "ID,IDTHELOAI,HINHANH,HINHANHFile")] THUVIENANH  tHUVIENANH)
        {

            var dulieucuagallary = new BAULEPROFOLIOEntities2();
            var modelgallary = dulieucuagallary.THUVIENANHs.Single(g => g.ID ==  tHUVIENANH.ID);           
            if ( tHUVIENANH.HINHANHFile == null)
            {
                 tHUVIENANH.HINHANH = modelgallary.HINHANH;
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension( tHUVIENANH.HINHANHFile.FileName);
                string extension = Path.GetExtension( tHUVIENANH.HINHANHFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                 tHUVIENANH.HINHANH = fileName;
                fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);
                 tHUVIENANH.HINHANHFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Entry( tHUVIENANH).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("THUVIENVIEW");
            }
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH",  tHUVIENANH.IDTHELOAI);
            return View( tHUVIENANH);
        }

        public async Task<ActionResult> THUVIENDEL(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUVIENANH  tHUVIENANH = await db.THUVIENANHs.FindAsync(id);
            if ( tHUVIENANH == null)
            {
                return HttpNotFound();
            }
            return View( tHUVIENANH);
        }

        // POST: THUVIENANHs/Delete/5
        [HttpPost, ActionName("THUVIENDEL")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed1(string id)
        {
            THUVIENANH  tHUVIENANH = await db.THUVIENANHs.FindAsync(id);
            db.THUVIENANHs.Remove( tHUVIENANH);
            await db.SaveChangesAsync();
            return RedirectToAction("THUVIENVIEW");
        }
        /// </summary>

      
        // CHỈNH SỬA THỂ LOẠI THƯ VIỆN
        public async Task<ActionResult> THELOAITHUVIENVIEW()
        {
            return View(await db.THELOAITHUVIENs.ToListAsync());
        }
        public async Task<ActionResult> THELOAITHUVIENEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THELOAITHUVIEN tHELOAITHUVIEN = await db.THELOAITHUVIENs.FindAsync(id);
            if (tHELOAITHUVIEN == null)
            {
                return HttpNotFound();
            }
            return View(tHELOAITHUVIEN);
        }

        // POST: THELOAITHUVIENs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> THELOAITHUVIENEDIT([Bind(Include = "IDTHELOAI,THELOAIANH")] THELOAITHUVIEN tHELOAITHUVIEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHELOAITHUVIEN).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("THELOAITHUVIENVIEW");
            }
            return View(tHELOAITHUVIEN);
        }
        //thêm thư viện
        public ActionResult THELOAITHUVIENADD()
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> THELOAITHUVIENADD([Bind(Include = "IDTHELOAI,THELOAIANH")] THELOAITHUVIEN tHELOAITHUVIEN)
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH", tHELOAITHUVIEN.IDTHELOAI);

            tHELOAITHUVIEN.IDTHELOAI = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.THELOAITHUVIENs.Add(tHELOAITHUVIEN);
                await db.SaveChangesAsync();
                return RedirectToAction("THELOAITHUVIENVIEW");
            }
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITHUVIENs, "IDTHELOAI", "THELOAIANH");
            return View(tHELOAITHUVIEN);
        }

        // TRANG TIN TỨC
        //public ActionResult TIMKIEM(string searchString)
        //{
        //    var links = from l in db.TINTUCs // lấy toàn bộ liên kết
        //                select l;
        //    if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
        //   {
        //        links = links.Where(s => s.TIEUDE.Contains(searchString)); //lọc theo chuỗi tìm kiếm
        //   }

        //   return View(links); //trả về kết quả
        //}
        //public ActionResult TIMKIEM(string IDTHELOAI)
        //{
        //    BAULEPROFOLIOEntities1 db = new BAULEPROFOLIOEntities2();
        //  /*  ViewBag.CompanyId = new SelectList(db.TINTUCs, "CompanyId", "CompanyName", CompanyID); */   // preselect item in selectlist by CompanyID param
        //    ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI");
        //    if (!String.IsNullOrWhiteSpace(IDTHELOAI))
        //    {
        //        return View();
        //    }

        //    return View(db.TINTUCs.Where(x => x.IDTHELOAI == IDTHELOAI).Take(50));
        //}
        //public async Task<ActionResult> TINTUCVIEW(string searchString)
        //{
        //    var links = from l in db.TINTUCs // lấy toàn bộ liên kết
        //                select l;
        //    if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
        //    {
        //        links = links.Where(s => s.TIEUDE.Contains(searchString)); //lọc theo chuỗi tìm kiếm
        //    }
        //    return View(links); //trả về kết quả

        //}

        public async Task<ActionResult> TINTUCVIEW()
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI");
            var tINTUCs = db.TINTUCs.Include(t => t.THELOAITINTUC);
            return View(await tINTUCs.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> TINTUCVIEW(string IDTHELOAI)
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI");
            var tINTUCs = db.TINTUCs.Include(t => t.THELOAITINTUC).Where(a => a.IDTHELOAI == IDTHELOAI);
            return View(await tINTUCs.ToListAsync());
        }
        public async Task<ActionResult> TINTUCCHITIET(string id)
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
        public ActionResult TINTUCADD()
        {
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI");
            return View();
        }

        // POST: TINTUCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TINTUCADD([Bind(Include = "ID,IDTHELOAI,TIEUDE,NOIDUNGPHU,HINHANH,NOIDUNGCHINH,NGAYTHANG, HINHANHFile")] TINTUC tINTUC)
        {
            string fileName = Path.GetFileNameWithoutExtension(tINTUC.HINHANHFile.FileName);
            string extension = Path.GetExtension(tINTUC.HINHANHFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            tINTUC.HINHANH = fileName;
            fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);
            tINTUC.HINHANHFile.SaveAs(fileName);
            tINTUC.ID = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.TINTUCs.Add(tINTUC);
                await db.SaveChangesAsync();
                return RedirectToAction("TINTUCVIEW");
            }

            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI", tINTUC.IDTHELOAI);
            return View(tINTUC);
        }

        public async Task<ActionResult> TINTUCEDIT(string id)
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
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI", tINTUC.IDTHELOAI);
            return View(tINTUC);
        }

        // POST: TINTUCs/Edit/5

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TINTUCEDIT([Bind(Include = "ID,IDTHELOAI,TIEUDE,NOIDUNGPHU,HINHANH,NOIDUNGCHINH,NGAYTHANG, HINHANHFile")] TINTUC tINTUC)
        {
            var dulieucuatintuc = new BAULEPROFOLIOEntities2();
            var modeltintuc = dulieucuatintuc.TINTUCs.Single(t => t.ID == tINTUC.ID);
            if (tINTUC.HINHANHFile == null)
            {
                tINTUC.HINHANH = modeltintuc.HINHANH;
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(tINTUC.HINHANHFile.FileName);
                string extension = Path.GetExtension(tINTUC.HINHANHFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                tINTUC.HINHANH = fileName;
                fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);
                tINTUC.HINHANHFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Entry(tINTUC).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("TINTUCVIEW");
            }
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI", tINTUC.IDTHELOAI);
            return View(tINTUC);
        }


        // GET: TINTUCs/Delete/5
        public async Task<ActionResult> TINTUCDEL(string id)
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
        [HttpPost, ActionName("TINTUCDEL")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed2(string id)
        {
            TINTUC tINTUC = await db.TINTUCs.FindAsync(id);
            db.TINTUCs.Remove(tINTUC);
            await db.SaveChangesAsync();
            return RedirectToAction("TINTUCVIEW");
        }
        
        //THẺ LOẠI TIN TỨC
        public async Task<ActionResult> THELOAITINTUCVIEW()
        {
            return View(await db.THELOAITINTUCs.ToListAsync());
        }

       
        // GET: THELOAITINTUCs/Create
        public ActionResult THELOAITINTUCADD()
        {
            return View();
        }

        // POST: THELOAITINTUCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> THELOAITINTUCADD([Bind(Include = "IDTHELOAI,TENTHELOAI")] THELOAITINTUC tHELOAITINTUC)
        {
            tHELOAITINTUC.IDTHELOAI = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.THELOAITINTUCs.Add(tHELOAITINTUC);
                await db.SaveChangesAsync();
                return RedirectToAction("THELOAITINTUCVIEW");
            }
            ViewBag.IDTHELOAI = new SelectList(db.THELOAITINTUCs, "IDTHELOAI", "TENTHELOAI", tHELOAITINTUC.IDTHELOAI);
            return View(tHELOAITINTUC);
        }

        // GET: THELOAITINTUCs/Edit/5
        public async Task<ActionResult> THELOAITINTUCEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THELOAITINTUC tHELOAITINTUC = await db.THELOAITINTUCs.FindAsync(id);
            if (tHELOAITINTUC == null)
            {
                return HttpNotFound();
            }
            return View(tHELOAITINTUC);
        }

        // POST: THELOAITINTUCs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> THELOAITINTUCEDIT([Bind(Include = "IDTHELOAI,TENTHELOAI")] THELOAITINTUC tHELOAITINTUC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHELOAITINTUC).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("THELOAITINTUCVIEW");
            }
            return View(tHELOAITINTUC);
        }

        // GET: THELOAITINTUCs/Delete/5
        public async Task<ActionResult> THELOAITINTUCDEL(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THELOAITINTUC tHELOAITINTUC = await db.THELOAITINTUCs.FindAsync(id);
            if (tHELOAITINTUC == null)
            {
                return HttpNotFound();
            }
            return View(tHELOAITINTUC);
        }

        // POST: THELOAITINTUCs/Delete/5
        [HttpPost, ActionName("THELOAITINTUCDEL")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed3(string id)
        {
            THELOAITINTUC tHELOAITINTUC = await db.THELOAITINTUCs.FindAsync(id);
            db.THELOAITINTUCs.Remove(tHELOAITINTUC);
            await db.SaveChangesAsync();
            return RedirectToAction("THELOAITINTUCVIEW");
        }

        //TRANG LIÊN HỆ
        public ActionResult CONTACTVIEW()
        {
            return View(db.LIENHEs.ToList());
        }
        
        // GET: CONTACTs/Edit/5
        public ActionResult CONTACTEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LIENHE cONTACT = db.LIENHEs.Find(id);
            if (cONTACT == null)
            {
                return HttpNotFound();
            }
            return View(cONTACT);
        }

        // POST: CONTACTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CONTACTEDIT([Bind(Include = "ID,TUADE,HOTLINE,ADDRESSS,PHONE,FAX,WEB,EMAIL,TUADECHANTRANG")] LIENHE cONTACT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cONTACT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CONTACTVIEW");
            }
            return View(cONTACT);
        }

        //TRANG DANH SÁCH LIÊN HỆ
        public ActionResult LISTCONTACTVIEW()
        {
            return View(db.DSKHACHHANGs.ToList());
        }
        public async Task<ActionResult> LISTCONTACTEDIT(string id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LISTCONTACTEDIT([Bind(Include = "ID,TENKH,GMAIL")] DSKHACHHANG dSKHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dSKHACHHANG).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("LISTCONTACTVIEW");
            }
            return View(dSKHACHHANG);
        }
        // GET: DSKHACHHANGs/Delete/5
        public async Task<ActionResult> LISTCONTACTDEL(string id)
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
        [HttpPost, ActionName("LISTCONTACTDEL")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed5(string id)
        {
            DSKHACHHANG dSKHACHHANG = await db.DSKHACHHANGs.FindAsync(id);
            db.DSKHACHHANGs.Remove(dSKHACHHANG);
            await db.SaveChangesAsync();
            return RedirectToAction("LISTCONTACTVIEW");
        }


        public ActionResult LISTCONTACTADD()
        {
            return View();
        }

        // POST: DSKHACHHANGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LISTCONTACTADD([Bind(Include = "ID,TENKH,GMAIL")] DSKHACHHANG dSKHACHHANG)
        {
            dSKHACHHANG.ID = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.DSKHACHHANGs.Add(dSKHACHHANG);
                await db.SaveChangesAsync();
                return RedirectToAction("LISTCONTACTVIEW");
            }

            return View(dSKHACHHANG);
        }
        //TRANG HÌNH ẢNH KHÁCH HÀNG

        public async Task<ActionResult> CLIENTVIEW()
        {
            return View(await db.LOGOKHACHHANGs.ToListAsync());
        }

        
        // GET: LOGOKHACHHANGs/Create
        public ActionResult CLIENTADD()
        {
            return View();
        }

        // POST: LOGOKHACHHANGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CLIENTADD([Bind(Include = "ID,HINHANH,LINKLOGO,TENLOGO,HINHANHFile")] LOGOKHACHHANG lOGOKHACHHANG)
        {
            string fileName = Path.GetFileNameWithoutExtension(lOGOKHACHHANG.HINHANHFile.FileName);
            string extension = Path.GetExtension(lOGOKHACHHANG.HINHANHFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            lOGOKHACHHANG.HINHANH = fileName;
            fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);
            lOGOKHACHHANG.HINHANHFile.SaveAs(fileName);
            lOGOKHACHHANG.ID = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.LOGOKHACHHANGs.Add(lOGOKHACHHANG);
                await db.SaveChangesAsync();
                return RedirectToAction("CLIENTVIEW");
            }

            return View(lOGOKHACHHANG);
        }

        // GET: LOGOKHACHHANGs/Edit/5
        public async Task<ActionResult> CLIENTEDIT(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGOKHACHHANG lOGOKHACHHANG = await db.LOGOKHACHHANGs.FindAsync(id);
            if (lOGOKHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(lOGOKHACHHANG);
        }

        // POST: LOGOKHACHHANGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CLIENTEDIT([Bind(Include = "ID,HINHANH,LINKLOGO,TENLOGO,HINHANHFile")] LOGOKHACHHANG lOGOKHACHHANG)
        {
            var dulieulogoclient = new BAULEPROFOLIOEntities2(); // anh tạo 1 chuổi dulieucoán anh gọi nguyên cái DB của em lại. đổ vào đó
            var HINHANHlogo = dulieulogoclient.LOGOKHACHHANGs.Single(e => e.ID == lOGOKHACHHANG.ID); // sau đó anh dùng biến HINHANHcosan để hứng dữ liệu model tại nơi mà ID của nó bằng ID của thằng SUAMODEL, để lấy ra đúng cái hình
            if (lOGOKHACHHANG.HINHANHFile == null)// nếu file nó null
            {
                lOGOKHACHHANG.HINHANH = HINHANHlogo.HINHANH; //nó sẽ fill dữ liệu từ data vào lại để edit
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(lOGOKHACHHANG.HINHANHFile.FileName);
                string extension = Path.GetExtension(lOGOKHACHHANG.HINHANHFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                lOGOKHACHHANG.HINHANH = fileName;
                fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);//coi kỹ lại chỗ này nha.
                lOGOKHACHHANG.HINHANHFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Entry(lOGOKHACHHANG).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("CLIENTVIEW");
            }
            return View(lOGOKHACHHANG);
        }

        // GET: LOGOKHACHHANGs/Delete/5
        public async Task<ActionResult> CLIENTDEL(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOGOKHACHHANG lOGOKHACHHANG = await db.LOGOKHACHHANGs.FindAsync(id);
            if (lOGOKHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(lOGOKHACHHANG);
        }

        // POST: LOGOKHACHHANGs/Delete/5
        [HttpPost, ActionName("CLIENTDEL")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed4(string id)
        {
            LOGOKHACHHANG lOGOKHACHHANG = await db.LOGOKHACHHANGs.FindAsync(id);
            db.LOGOKHACHHANGs.Remove(lOGOKHACHHANG);
            await db.SaveChangesAsync();
            return RedirectToAction("CLIENTVIEW");
        }

        //CHÂN TRANG
        public async Task<ActionResult> FOOTVIEW()
        {
            return View(await db.CHANTRANGs.ToListAsync());
        }
        
        public async Task<ActionResult> FOOTEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHANTRANG cHANTRANG = await db.CHANTRANGs.FindAsync(id);
            if (cHANTRANG == null)
            {
                return HttpNotFound();
            }
            return View(cHANTRANG);
        }

        // POST: CHANTRANGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FOOTEDIT([Bind(Include = "ID,NOIDUNG,HINHANHLOGO,HINHANHLOGOFile")] CHANTRANG cHANTRANG)
        {
            var dulieuchantrang = new BAULEPROFOLIOEntities2(); 
            var hinhanhchantrang = dulieuchantrang.CHANTRANGs.Single(c=> c.ID == cHANTRANG.ID); 
            if (cHANTRANG.HINHANHLOGOFile == null)
            {
                cHANTRANG.HINHANHLOGO = hinhanhchantrang.HINHANHLOGO;
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(cHANTRANG.HINHANHLOGOFile.FileName);
                string extension = Path.GetExtension(cHANTRANG.HINHANHLOGOFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                cHANTRANG.HINHANHLOGO = fileName;
                fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);//coi kỹ lại chỗ này nha.
                cHANTRANG.HINHANHLOGOFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Entry(cHANTRANG).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("FOOTVIEW");
            }
            return View(cHANTRANG);
        }
        //PHOTO
        public ActionResult PHOTOVIEW()
        {
            return View(db.Photos.ToList());
        }



        // GET: Photos/Edit/5
        public ActionResult PHOTOEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHOTO PHOTOEDIT = db.Photos.Find(id);
            if (PHOTOEDIT == null)
            {
                return HttpNotFound();
            }
            return View(PHOTOEDIT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PHOTOEDIT([Bind(Include = "ID,SOTHUTU,HINHANH,HINHANHFile")] PHOTO pHOTO)
        {
            var dulieuphoto = new BAULEPROFOLIOEntities2(); // anh tạo 1 chuổi dulieucoán anh gọi nguyên cái DB của em lại. đổ vào đó
            var modelclient = dulieuphoto.Photos.Single(e => e.ID == pHOTO.ID); // sau đó anh dùng biến modelimagecosan để hứng dữ liệu model tại nơi mà ID của nó bằng ID của thằng editmodel, để lấy ra đúng cái hình
            if (pHOTO.HINHANHFile == null)// nếu file nó null
            {
                pHOTO.HINHANH = modelclient.HINHANH; //nó sẽ fill dữ liệu từ data vào lại để edit
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(pHOTO.HINHANHFile.FileName);
                string extension = Path.GetExtension(pHOTO.HINHANHFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                pHOTO.HINHANH = fileName;
                fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);//coi kỹ lại chỗ này nha.
                pHOTO.HINHANHFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Entry(pHOTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PHOTOVIEW");
            }
            return View(pHOTO);
        }
        //PHOTOADD
        public ActionResult PHOTOADD()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PHOTOADD([Bind(Include = "ID,SOTHUTU,HINHANH,HINHANHFile")] PHOTO pHOTO)
        {
            
            string fileName = Path.GetFileNameWithoutExtension(pHOTO.HINHANHFile.FileName);
            string extension = Path.GetExtension(pHOTO.HINHANHFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            pHOTO.HINHANH = fileName;
            fileName = Path.Combine(Server.MapPath("~/ContentWebmaster/img/"), fileName);
            pHOTO.HINHANHFile.SaveAs(fileName);
            pHOTO.ID = getGUID().ToString();
            if (ModelState.IsValid)
            {
                db.Photos.Add(pHOTO);
                await db.SaveChangesAsync();
                return RedirectToAction("PHOTOVIEW");
            }

            return View(pHOTO);
        }

        //PHOTODEL
        public async Task<ActionResult> PHOTOTDEL(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHOTO lOGOKHACHHANG = await db.Photos.FindAsync(id);
            if (lOGOKHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(lOGOKHACHHANG);
        }

        // POST: LOGOKHACHHANGs/Delete/5
        [HttpPost, ActionName("PHOTODEL")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed45(string id)
        {
            PHOTO lOGOKHACHHANG = await db.Photos.FindAsync(id);
            db.Photos.Remove(lOGOKHACHHANG);
            await db.SaveChangesAsync();
            return RedirectToAction("PHOTOVIEW");
        }

        //SUBBODY
        public ActionResult SUBBODYVIEW()
        {
            return View(db.MUCLINKs.ToList());
        }
        public ActionResult SUBBODYEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MUCLINK sUBBODY = db.MUCLINKs.Find(id);
            if (sUBBODY == null)
            {
                return HttpNotFound();
            }
            return View(sUBBODY);
        }

        // POST: SUBBODies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SUBBODYEDIT([Bind(Include = "ID,EMAIL,PHONE,LINKFB,LINKPRIN,LINKTWI,LINKINS,LINKYB,LINKINK")] MUCLINK sUBBODY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sUBBODY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SUBBODYVIEW");
            }
            return View(sUBBODY);
        }
        // GET: TAIKHOANs
        public async Task<ActionResult> TAIKHOANVIEW()
        {
            return View(await db.TAIKHOANs.ToListAsync());
        }
        // GET: TAIKHOANs/Edit/5
        public async Task<ActionResult> TAIKHOANEDIT(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = await db.TAIKHOANs.FindAsync(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }

        // POST: TAIKHOANs/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TAIKHOANEDIT([Bind(Include = "IDTAIKHOAN,PASS")] TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAIKHOAN).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("TAIKHOANVIEW");
            }
            return View(tAIKHOAN);
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