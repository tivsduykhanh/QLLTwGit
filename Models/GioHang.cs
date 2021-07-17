using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLlaptop.Models
{
    public class GioHang
    {
        dbQLLTDataContext data = new dbQLLTDataContext();
        public int iMasanpham { set; get; }
        public string sTensanpham { set; get; }
        public  string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get
            {
                return iSoluong * dDongia;
            }
        }
        public GioHang(int MaSP)
        {
            iMasanpham = MaSP;
            SanPham sp = data.SanPhams.Single(n => n.MaSP == iMasanpham);
            sTensanpham = sp.TenSP;
            sAnhbia = sp.ImageSP;
            dDongia = double.Parse(sp.Giatien.ToString());
            iSoluong = 1;
    }
    }
   
}