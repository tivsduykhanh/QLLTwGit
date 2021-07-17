using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLlaptop.Models;

namespace QLlaptop.Controllers
{
    public class GioHangController : Controller
    {
        dbQLLTDataContext data = new dbQLLTDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if(lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int iMasanpham , string strURL)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.Find(n => n.iMasanpham == iMasanpham);
            if(sanpham == null)
            {
                sanpham = new GioHang(iMasanpham);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int Tongsoluong()
        {
            int iTongsoluong = 0;
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if(lstGiohang != null)
            {
                iTongsoluong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongsoluong;
        }
        private double Tongtien()
        {
            double iTongtien = 0;
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if(lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongtien;
        }
        public ActionResult Giohang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return View(lstGiohang);
        }
        public ActionResult Xoagiohang(int iMaSP)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMasanpham == iMaSP);
            if(sanpham !=null)
            {
                lstGiohang.RemoveAll(n => n.iMasanpham == iMaSP);
                return RedirectToAction("Giohang");
            }
            if(lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult Capnhatgiohang(int iMaSP, FormCollection f)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMasanpham == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult Xoaall()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["User"]==null ||  Session["User"].ToString()=="")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if(Session["Giohang"]== null )
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstgiohang = Laygiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return View(lstgiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            Khachhang kh = (Khachhang)Session["User"];
            List<GioHang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Tinhtrangthanhtoan = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach(var item in gh)
            {
                CTDonHang ctdh = new CTDonHang();
                ctdh.MaDH = ddh.MaDH;
                ctdh.MaSP = item.iMasanpham;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CTDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandondathang", "Giohang");

        }
        public ActionResult Xacnhandondathang()
        {
            return View();
        }
    }
}