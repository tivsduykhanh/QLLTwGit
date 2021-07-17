using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLlaptop.Models;

namespace QLlaptop.Controllers
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        dbQLLTDataContext data = new dbQLLTDataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, Khachhang kh)
        {
            var hoten = collection["HotenKH"];
            var user = collection["TenDN"];
            var password = collection["Matkhau"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            var diachi = collection["Diachi"];
            var gioitinh = collection["Gioitinh"];
            var sdt = collection["Dienthoai"];
            var cmnd = collection["CMND"];
            var email = collection["Email"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(user))
            {
                ViewData["Loi2"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi4"] = "Địa chỉ không được để trống";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi5"] = " SDT không được để trống";
            }
            else if (String.IsNullOrEmpty(cmnd))
            {
                ViewData["Loi6"] = "CMND không được để trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi7"] = "Email không được để trống";
            }
            else
            {
                kh.Hoten = hoten;
                kh.User = user;
                kh.Password = password;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                kh.Diachi = diachi;
                if (gioitinh== null )
                {
                    kh.Gioitinh = true;
                }
                else
                    kh.Gioitinh = false;
                kh.SDT = sdt;
                kh.CMND = cmnd;
                kh.Email = email;
                data.Khachhangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Khachhang kh = data.Khachhangs.SingleOrDefault(n => n.User == tendn && n.Password == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng bạn đã đăng nhập thành công";
                    Session["User"] = kh;
                    //return RedirectToAction("Index","Home");
                }
                else
                    ViewBag.Thongbao = "Có gì đó không đúng mời bạn đăng nhập lại ";
            }
            return View();
        }
    }
}