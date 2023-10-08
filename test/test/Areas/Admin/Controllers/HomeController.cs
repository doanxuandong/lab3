using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;
using test.Areas;
using System.IO;
using PagedList.Mvc;
using PagedList;


namespace test.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QLBanSachEntities3 db = new QLBanSachEntities3();
        // GET: Admin/Home
        public ActionResult Index(int? page)
        {
            int PageNum = (page ?? 1);
            int PageSize = 7;
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            ADMIN ad = db.ADMINs.SingleOrDefault(n=> n.TenDNAdmin == sTenDN && n.MatKhauAdmin == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}