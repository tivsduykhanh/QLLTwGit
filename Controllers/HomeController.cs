using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLlaptop.Models;
using PagedList;
using PagedList.Mvc;

namespace QLlaptop.Controllers
{
    public class HomeController : Controller
    {
        dbQLLTDataContext data = new dbQLLTDataContext();

        private List<SanPham> GetNewProc(int count)
        {
            return data.SanPhams.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        private List<LoaiSanPham> GetLoai()
        {
            return data.LoaiSanPhams.OrderBy(a => a.Tenloai).ToList();
        }

        public PartialViewResult getloai()
        {
            var loai = GetLoai();
            return PartialView(loai);
        }

        private List<ThuongHieu> GetBrand()
        {
            return data.ThuongHieus.OrderBy(a => a.Tenthuonghieu).ToList();
        }

        public PartialViewResult getbr()
        {
            var br = GetBrand();
            return PartialView(br);
        }

        public ActionResult Index(int ? page )
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            //Lay 5 laptop moi nhat
            var NewProc = GetNewProc(5);
            return View(NewProc.ToPagedList(pageNum,pageSize));
        }

        public ActionResult Detail(int id)
        {
            var sanpham = from sp in data.SanPhams
                          where sp.MaSP == id
                          select sp;
            return View(sanpham.Single());
        }

        //Lấy San pham theo loại
        public ActionResult SPtheoloai(int id)
        {
            var sanpham = from sp in data.SanPhams where sp.Maloai == id select sp;
            return View(sanpham);
        }
        //public ActionResult SPtheoloai(int ?page)
        //{
        //    int pageNumber = (page ?? 1);
        //    int pageSize = 7;
        //    return View(data.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        //}

        //Lấy Sản phẩm theo thương hiệu
        public ActionResult SPtheothuonghieu(int id)
        {
            var sanpham = from sp in data.SanPhams where sp.Mathuonghieu == id select sp;
            return View(sanpham);
        }
    }
}