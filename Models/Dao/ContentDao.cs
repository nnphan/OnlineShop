using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Models.Dao
{
    public class ContentDao
    {
        OnlineShopDBContext db = null;
        public static string USER_SESSION = "USER_SESSION";
        public ContentDao()
        {
            db = new OnlineShopDBContext();
        }

        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }

        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }



    }
}
