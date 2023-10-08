using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using PagedList;
using System.IO;
using System.Net;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Runtime.Serialization;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace test.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        private QLBanSachEntities3 db = new QLBanSachEntities3();

        // GET: Admin/SACH
        public ActionResult Index(int? page)
        {
            int PageNum = (page ?? 1);
            int PageSize = 7;
            return View(db.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(PageNum, PageSize));
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHUDE cHUDE = db.CHUDEs.Find(id);
            if (cHUDE == null)
            {
                return HttpNotFound();
            }
            return View(cHUDE);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MACD, TenChuDe")] CHUDE CD)
        {
            if (ModelState.IsValid)
            {
                db.CHUDEs.Add(CD);
                db.SaveChanges();
                return RedirectToAction("Index","ChuDe");
            }
            return View(CD);
        }  // GET: Admin/CHUDEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHUDE cHUDE = db.CHUDEs.Find(id);
            if (cHUDE == null)
            {
                return HttpNotFound();
            }
            return View(cHUDE);
        }

        // POST: Admin/CHUDEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCD,TenChuDe")] CHUDE cHUDE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cHUDE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cHUDE);
        }

        // GET: Admin/CHUDEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHUDE cHUDE = db.CHUDEs.Find(id);
            if (cHUDE == null)
            {
                return HttpNotFound();
            }
            return View(cHUDE);
        }

        // POST: Admin/CHUDEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CHUDE cHUDE = db.CHUDEs.Find(id);
            db.CHUDEs.Remove(cHUDE);
            db.SaveChanges();
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