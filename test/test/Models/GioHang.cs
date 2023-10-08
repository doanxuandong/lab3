using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class GioHang
    {
        QLBanSachEntities3 db = new QLBanSachEntities3();
        public int Masach { get; set; }
        public string Tensach { get; set; }
        public string Anhbia { get; set; }
        public double Dongia { get; set; }
        public int Soluong { get; set; }
        public double Thanhtien {
            get { return Soluong * Dongia; }
        }
        public GioHang(int ms)
        {
            Masach = ms;
            SACH s = db.SACHes.Single(n => n.MaSach == Masach);
            Tensach = s.TenSach;
            Anhbia = s.HinhMinhHoa;
            Dongia = double.Parse(s.DonGia.ToString());
            Soluong = 1;
        }

    }
}