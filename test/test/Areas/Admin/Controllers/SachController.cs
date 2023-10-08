using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test.Models;
using PagedList.Mvc;
using PagedList;
using System.IO;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace test.Areas.Admin.Controllers
{
    public class SachController : Controller
    {
        private QLBanSachEntities3 db = new QLBanSachEntities3();

        // GET: Admin/SACH
        public ActionResult Index(int? page)
        {
            int PageNum = (page ?? 1);
            int PageSize = 7;
            var sACHes = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN);
            return View(db.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(PageNum, PageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, FormCollection f, HttpPostedFileBase fFileUpload)
        {

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/sach"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.TenSach = f["sTenSach"];
                    sach.MoTa = f["sMoTa"].Replace("<p>","").Replace("</p>", "\n");
                    sach.HinhMinhHoa = sFileName;
                    sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sach.DonGia = decimal.Parse(f["mGiaBan"]);
                    sach.MaCD = int.Parse(f["MaCD"]);
                    sach.MaNXB = int.Parse(f["MaNXB"]);
                    db.SACHes.Add(sach);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();

            }
        }

        // GET: Admin/SACH/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sACH);
        }


        // POST: Admin/SACH/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Admin/SACH/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sach = db.SACHes.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSach,TenSach,MaCD,DonGia,HinhMinhHoa,MaNXB,MoTa,SoLuongXem,SoLuongBan")] SACH book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Sach");
             }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", book.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", book.MaNXB);
            return View(book);
        }


        // GET: Admin/SACH/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sach = db.SACHes.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SACH sach = db.SACHes.Find(id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CTDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {

                @ViewBag.ThongBao = "Sách này đang có trong bảng Chi tiết đặt hàng <br>" + " Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hang";
                return View(sach);
            }
            var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                db.SACHes.Remove(sach);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }
}
