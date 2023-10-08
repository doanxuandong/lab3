using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using test.Models;
using static System.Collections.Specialized.BitVector32;

namespace test.Controllers
{
    public class GiohangController : Controller
    {
        QLBanSachEntities3 db = new QLBanSachEntities3();
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }
        public List<GioHang> LayGiohang()
        {
            List<GioHang> lstgiohang = Session["GioHang"] as List<GioHang>;
            if (lstgiohang == null)
            {
                lstgiohang = new List<GioHang>();
                Session["GioHang"] = lstgiohang;
            }
            return lstgiohang;
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGiohang = LayGiohang();
            GioHang sp = lstGiohang.Find(n => n.Masach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGiohang.Add(sp);
            }
            else
            {
                sp.Soluong++;
            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int sum = 0;
            List<GioHang> lstgiohang = Session["GioHang"] as List<GioHang>;
            if (lstgiohang != null)
            {
                sum = lstgiohang.Sum(n => n.Soluong);
            }
            return sum;
        }
        private double TongTien()
        {
            double sum = 0;
            List<GioHang> lstgiohang = Session["GioHang"] as List<GioHang>;
            if (lstgiohang != null)
            {
                sum = lstgiohang.Sum(n => n.Thanhtien);
            }
            return sum;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstgiohang = LayGiohang();
            if (lstgiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstgiohang);
        }
        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGiohang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.Masach == iMaSach);
            if (sp != null) 
            {
                lstGioHang.RemoveAll(n => n.Masach == iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Giohang_partial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGiohang = LayGiohang();
            GioHang sp = lstGiohang.SingleOrDefault(n => n.Masach == iMaSach);
            if (sp != null)
            {
                sp.Soluong = int.Parse(f["Soluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGiohang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
           if (Session["TenDN"] == null|| Session["TenDN"].ToString()=="")
            {
                return Redirect("~~/Views/User/DangNhap?id=2");
            }
            //Kiểm tra có hàng trong giỏ chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<GioHang> lstGioHang = LayGiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TenDN"];
           
            List<GioHang> lstGioHang = LayGiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            
            var NgayGiao = String.Format("{0:dd/MM/yyyy}", f["NgayGiao"]);
            ddh.NgayGiaoHang = DateTime.Parse(NgayGiao);
            ddh.DaGiao = true;
            ddh.HTThanhToan = false;
            ddh.TriGia = (decimal)TongTien();
            ddh.TenNguoiNhan = kh.HoTenKH;
            ddh.DiaChiNhan = kh.DiaChiKH;
            ddh.HTGiaoHang = true;
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in lstGioHang)
            {

                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaSach = item.Masach;
                ctdh.SoLuong = item.Soluong;
                ctdh.DonGia = (decimal)item.Dongia;
                ctdh.ThanhTien = (decimal)item.Thanhtien;
                db.CTDATHANGs.Add(ctdh);
         
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
       
    }
}