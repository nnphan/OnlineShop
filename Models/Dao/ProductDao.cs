using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;


namespace Models.Dao
{
    public class ProductDao
    {
        OnlineShopDBContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDBContext();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Product> ListProductsByCategoryId(long categoryId, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryId).Count();
            return  db.Products.Where(x => x.CategoryID == categoryId).OrderByDescending(x=> x.CreatedDate).Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
        }

        public List<Product> ListProductsByCategoryId2(long categoryId)
        {

            return db.Products.Where(x => x.CategoryID == categoryId).ToList();
        }
        //public void UpdateImages(long productId, string images)
        //{
        //    var product = db.Products.Find(productId);
        //    product.MoreImages = images;
        //    db.SaveChanges();
        //}
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
    }
}
