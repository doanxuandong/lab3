using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Models;

namespace test.Controllers
{
    public class UserController : Controller
    {
        QLBanSachEntities3 db = new QLBanSachEntities3();
        // GET: Sach
        /*public ActionResult Index()
        {
            return View();
        }*/
        
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection clq)
        {
            KHACHHANG kh = (KHACHHANG)Session["TenDN"];
            var hoten = clq["TenDN"];
            var matkhat = clq["MatKhau"];
            int state = int.Parse(Request.QueryString["id"]);
            if (kh != null)
            {
                Session["TenDN"] = kh;
                if (state == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("DatHang", "Giohang");
                }
            }
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["er1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if(String.IsNullOrEmpty(matkhat))
            {
                ViewData["er2"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                KHACHHANG user = db.KHACHHANGs.SingleOrDefault(n=>n.TenDN == hoten && n.MatKhau == matkhat );
                if(user is null)
                {
                    ViewBag.thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
                else
                {
                    ViewBag.thongbao = "Bạn đã đăng nhập thành công";
                    Session["TenDN"] = user;
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
           
        }
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection clq, KHACHHANG user)
        {
            var hoten = clq["HoTenKH"];
            var diachi = clq["DiaChiKH"];
            var dienthoai = clq["DienThoaiKH"];
            var ten = clq["TenDN"];
            var matkhau = clq["MatKhau"];
            var ngaysinh = String.Format("{0:MM/dd/yyy}", clq["NgaySinh"]);
            var gioitinh = clq["GioiTinh"];
            var email = clq["Email"];
            var daduyet = clq["DaDuyet"];
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["er1"] = "Họ tên không được rỗng";
                
            }  
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["er2"] = "Đại chỉ không được rỗng";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["er3"] = "Điện thoại không được rỗng";
            }
            else if (String.IsNullOrEmpty(ten))
            {
                ViewData["er4"] = "Tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["er5"] = "Bạn chưa đặt mật khẩu";
            }
            else if (String.IsNullOrEmpty(ngaysinh))
            {
                ViewData["er6"] = "Bạn chưa chọn ngày sinh";
            }
            else if (String.IsNullOrEmpty(gioitinh))
            {
                ViewData["er7"] = "Bạn chưa chọn giới tính";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["er8"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(daduyet))
            {
                ViewData["er9"] = "Đại chỉ không được rỗng";
            }
            else if(db.KHACHHANGs.SingleOrDefault(n=>n.TenDN==ten)!=null)
            {
                ViewBag.thongbao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == email) != null)
            {
                ViewBag.thongbao = "Email đã tồn tại";
            }
            else
            {
                user.HoTenKH = hoten;
                user.DiaChiKH = diachi;
                user.DienThoaiKH = dienthoai;
                user.TenDN = ten;
                user.MatKhau = matkhau;
                user.NgaySinh = DateTime.Parse(ngaysinh);
                user.GioiTinh = bool.Parse(gioitinh);
                user.Email = email;
                user.DaDuyet = bool.Parse(daduyet);
                db.KHACHHANGs.Add(user);
                db.SaveChanges();
                return RedirectToAction("DangNhap", "User");
            }
            return View();
        }
     
    }
}