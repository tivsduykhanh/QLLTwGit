using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLlaptop.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace QLlaptop.Controllers
{
    public class AdminController : Controller
    {
        dbQLLTDataContext data = new dbQLLTDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["pass"];
            Admin ad = data.Admins.SingleOrDefault(n => n.User == tendn && n.Password == matkhau);
            if(ad != null )
            {
                ViewBag.Thongbao = "Chúc mừng ban đăng nhập thành công";
                Session["User"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            }
         return View();
        }
        public ActionResult Sanpham(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(data.SanPhams.ToList());
            return View(data.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themsanpham()
        {
            ViewBag.Mathuonghieu = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.Tenthuonghieu), "Mathuonghieu", "Tenthuonghieu");
            ViewBag.Maloai = new SelectList(data.LoaiSanPhams.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(SanPham sp , HttpPostedFileBase fileupload)
        {
            ViewBag.Mathuonghieu = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.Tenthuonghieu), "Mathuonghieu", "Tenthuonghieu");
            ViewBag.Maloai = new SelectList(data.LoaiSanPhams.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");   
            if(fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
               if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/All/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã trùng";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.ImageSP = fileName;
                    data.SanPhams.InsertOnSubmit(sp);
                    data.SubmitChanges();
                }
            }

            return RedirectToAction("Sanpham");
        }
        public ActionResult Chitietsanpham(int id)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost,ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SanPhams.DeleteOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("Sanpham");
        }
        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Mathuonghieu = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.Tenthuonghieu), "Mathuonghieu", "Tenthuonghieu",sp.Mathuonghieu);
            ViewBag.Maloai = new SelectList(data.LoaiSanPhams.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai",sp.Maloai);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(SanPham sp , HttpPostedFileBase fileupload)
        {
            ViewBag.Mathuonghieu = new SelectList(data.ThuongHieus.ToList().OrderBy(n => n.Tenthuonghieu), "Mathuonghieu", "Tenthuonghieu");
            ViewBag.Maloai = new SelectList(data.LoaiSanPhams.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/All/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã trùng";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.ImageSP = fileName;
                    UpdateModel(sp);
                    data.SubmitChanges();   
                }
            }

            return RedirectToAction("Sanpham");
        }
    }
}