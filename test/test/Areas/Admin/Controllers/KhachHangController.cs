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
    public class KhachHangController : Controller
    {
        private QLBanSachEntities3 db = new QLBanSachEntities3();

        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            int PageNum = (page ?? 1);
            int PageSize = 7;
            KHACHHANG kh = (KHACHHANG)Session["GioiTinh"];
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(PageNum, PageSize));
        }

        // GET: Admin/KhachHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // GET: Admin/KhachHang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,HoTenKH,DiaChiKH,DienThoaiKH,TenDN,MatKhau,NgaySinh,GioiTinh,Email,DaDuyet")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANGs.Add(kHACHHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kHACHHANG);
        }

        // GET: Admin/KhachHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // POST: Admin/KhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,HoTenKH,DiaChiKH,DienThoaiKH,TenDN,MatKhau,NgaySinh,GioiTinh,Email,DaDuyet")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHACHHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kHACHHANG);
        }

        // GET: Admin/KhachHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // POST: Admin/KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var cTDATHANGs = db.DONDATHANGs.Where(ct => ct.MaKH == id);
            if (cTDATHANGs.Count() > 0)
            {

                @ViewBag.ThongBao = "Khách hàng này đang có trong bảng đơn đặt hàng <br>" + " Nếu muốn xóa thì phải xóa hết mã đơn hàng này trong bảng đơn đặt hàng";
                return View(kHACHHANG);
              
            }

            if (kHACHHANG != null)
            {
                db.KHACHHANGs.Remove(kHACHHANG);
                db.SaveChanges();
            }
           
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
