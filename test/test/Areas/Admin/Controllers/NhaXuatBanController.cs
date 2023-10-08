using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Areas.Admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        private QLBanSachEntities3 db = new QLBanSachEntities3();

        // GET: Admin/NhaXuatBan
        public ActionResult Index(int? page)
        {
            int PageNum = (page ?? 1);
            int PageSize = 7;
            return View(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(PageNum, PageSize));
        }

        // GET: Admin/NhaXuatBan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nHAXUATBAN = db.NHAXUATBANs.Find(id);
            if (nHAXUATBAN == null)
            {
                return HttpNotFound();
            }
            return View(nHAXUATBAN);
        }

        // GET: Admin/NhaXuatBan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhaXuatBan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNXB,TenNXB,DiaChi,DienThoai")] NHAXUATBAN nHAXUATBAN)
        {
            if (ModelState.IsValid)
            {
                db.NHAXUATBANs.Add(nHAXUATBAN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nHAXUATBAN);
        }

        // GET: Admin/NhaXuatBan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nHAXUATBAN = db.NHAXUATBANs.Find(id);
            if (nHAXUATBAN == null)
            {
                return HttpNotFound();
            }
            return View(nHAXUATBAN);
        }

        // POST: Admin/NhaXuatBan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNXB,TenNXB,DiaChi,DienThoai")] NHAXUATBAN nHAXUATBAN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nHAXUATBAN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nHAXUATBAN);
        }

        // GET: Admin/NhaXuatBan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nHAXUATBAN = db.NHAXUATBANs.Find(id);
            if (nHAXUATBAN == null)
            {
                return HttpNotFound();
            }
            return View(nHAXUATBAN);
        }

        // POST: Admin/NhaXuatBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NHAXUATBAN nHAXUATBAN = db.NHAXUATBANs.Find(id);
            if (nHAXUATBAN == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var sach = db.SACHes.Where(ct => ct.MaNXB == id);
            if (sach.Count() > 0)
            {

                @ViewBag.ThongBao = "Nhà XB này đang có trong bảng sách <br>" + " Nếu muốn xóa thì phải xóa hết mã NXB này trong bảng SÁCH";
                return View(nHAXUATBAN);
            }
            if (nHAXUATBAN != null)
            {
                db.NHAXUATBANs.Remove(nHAXUATBAN);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
