using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class ContactDao
    {
        OnlineShopDBContext db = null;
        public ContactDao()
        {
            db = new OnlineShopDBContext();
        }

        public Contact GetActiveContact()
        {
            return db.Contacts.Single(x => x.Status == true);
        }

       
    }
}
