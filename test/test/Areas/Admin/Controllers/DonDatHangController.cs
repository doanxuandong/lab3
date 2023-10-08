using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using test.Models;

namespace test.Areas.Admin.Controllers
{
    public class DonDatHangController : Controller
    {
        private QLBanSachEntities3 db = new QLBanSachEntities3();

        // GET: Admin/DonDatHang
        public ActionResult Index(int? page)
        {
            int PageNum = (page ?? 1);
            int PageSize = 7;
            var dONDATHANGs = db.DONDATHANGs.Include(d => d.KHACHHANG);
            return View(dONDATHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(PageNum, PageSize));
        }

        // GET: Admin/DonDatHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONDATHANG);
        }

        // GET: Admin/DonDatHang/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTenKH");
            return View();
        }

        // POST: Admin/DonDatHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoDH,MaKH,NgayDH,TriGia,DaGiao,NgayGiaoHang,TenNguoiNhan,DiaChiNhan,DienThoaiNhan,HTThanhToan,HTGiaoHang")] DONDATHANG dONDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.DONDATHANGs.Add(dONDATHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTenKH", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // GET: Admin/DonDatHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTenKH", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // POST: Admin/DonDatHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoDH,MaKH,NgayDH,TriGia,DaGiao,NgayGiaoHang,TenNguoiNhan,DiaChiNhan,DienThoaiNhan,HTThanhToan,HTGiaoHang")] DONDATHANG dONDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dONDATHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTenKH", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // GET: Admin/DonDatHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONDATHANG);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CTDATHANGs.Where(ct => ct.SoDH == id);
            if (ctdh.Count() > 0)
            {

                @ViewBag.ThongBao = "Đơn hàng này đang có trong bảng Chi tiết đặt hàng <br>" + " Nếu muốn xóa thì phải xóa hết mã đơn hàng này trong bảng Chi tiết đặt hang";
                return View(dONDATHANG);
            }
            var KH = db.KHACHHANGs.Where(vs => vs.MaKH == id).ToList();
            if (KH != null)
            {
                db.DONDATHANGs.Remove(dONDATHANG);
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
