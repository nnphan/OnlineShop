using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class CategoryDao
    {
        OnlineShopDBContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDBContext();
        }

        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }

        public long Insert(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category.ID;
        }

        public Category ViewDetail(long id)
        {
            return db.Categories.Find(id);
        }
    }
}
