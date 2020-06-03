using Models.Dao;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        //public ActionResult Category(long cateId, int pageSize=2, int pageIndex = 1 )
        //{
        //    var productCategory = new ProductCategoryDao().ViewDetail(cateId);
        //    ViewBag.ProductCategory = productCategory;

        //    int totalRecord = 0;
        //    var model = new ProductDao().ListProductsByCategoryId(cateId,ref totalRecord, pageIndex, pageSize);

        //    int maxPage = 5;
        //    int totalPage = 0;
        //    totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));


        //    ViewBag.Total = totalRecord;
        //    ViewBag.Page = pageIndex;
        //    ViewBag.TotalPage = totalPage; ;
        //    ViewBag.MaxPage = maxPage;
        //    ViewBag.First = 1;
        //    ViewBag.Last = totalPage;
        //    ViewBag.Next = pageIndex + 1;
        //    ViewBag.Prev = pageIndex - 1;

        //    return View(model);
        //}

        public ActionResult Category(long cateId, int? page)
        {
            var productCategory = new ProductCategoryDao().ViewDetail(cateId);
            ViewBag.ProductCategory = productCategory;
            var pageNumber = page ?? 1;
            var pageSize = 2;
            var model = new ProductDao().ListProductsByCategoryId2(cateId);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [OutputCache(Duration = 3600 * 24, VaryByParam = "productId")]
        public ActionResult Detail(long productId)
        {
            var productModel = new ProductDao().ViewDetail(productId);
            ViewBag.Category = new ProductCategoryDao().ViewDetail(productModel.CategoryID.Value);
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(productId);
            return View(productModel);
        }


    }
}