using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using PagedList.Mvc;
using PagedList;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        QLBanSachEntities3 db = new QLBanSachEntities3();

        private List<SACH> Laysachmoi(int count)
        {
            return db.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index(int? page, string searchString)
        {
            var books = from l in db.SACHes select l;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.TenSach.Contains(searchString));
            }

            int PageNum = (page ?? 1);
            int PageSize = 6;

            ViewBag.Keyword = searchString;

            return View(books.OrderBy(n => n.MaSach).ToPagedList(PageNum, PageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public ActionResult SubjectChude()
        {
            return PartialView(db.CHUDEs.ToList());
        }
        [ChildActionOnly]
        public ActionResult SubjectNXB()
        {
            return PartialView(db.NHAXUATBANs.ToList());
        }
        [ChildActionOnly]
        public ActionResult SubjectSach()
        {
            return PartialView(db.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(6).ToList());
        }

        public ActionResult Sachtheochude(int id, int? page)
        {
            foreach (var item in db.CHUDEs)
            {
                if (item.MaCD == id)
                {
                    ViewBag.tenchude = item.TenChuDe.ToString();
                }
            }
            ViewBag.MACD = id;
            int Size = 6;
            int PageNum = (page ?? 1);
            var sach = (from s in db.SACHes where s.MaCD == id select s).ToList();
            return View(sach.ToPagedList(PageNum, Size));
        }
        public ActionResult SachtheoNXB(int? id, int? page)
        {
            foreach (var item in db.NHAXUATBANs)
            {
                if (item.MaNXB == id)
                {
                    ViewBag.tenchude = item.TenNXB.ToString();
                }
            }
            ViewBag.MACD = id;
            int Size = 6;
            int PageNum = (page ?? 1);
            var sach = (from s in db.SACHes where s.MaCD == id select s).ToList();
            return View(sach.ToPagedList(PageNum, Size));
        }
        public ActionResult Chitietsach(int id)
        {

            var sach = from s in db.SACHes where s.MaSach == id select s;
            ViewBag.s = "sdfsdfdsf";
            return View(sach.Single());
        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogout");

        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    
     
    }
}