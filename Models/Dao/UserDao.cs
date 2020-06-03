using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Common;


namespace Models.Dao
{
    public class UserDao
    {
        OnlineShopDBContext db = null;
        public UserDao()
        {
            db = new OnlineShopDBContext();
        }
        public long Insert(User userEntity)
        {
            db.Users.Add(userEntity);
            db.SaveChanges();
            return userEntity.ID;
        }

        public long InsertForFacebook(User userEntity)
        {
            var user = db.Users.SingleOrDefault(x =>x.Username == userEntity.Username );
            if (user == null)
            {
                db.Users.Add(userEntity);
                db.SaveChanges();
                return userEntity.ID;
            }
            else return user.ID;
        }

        public bool Update(User userEntity)
        {
            try
            {
                var user = db.Users.Find(userEntity.ID);
                user.Name = userEntity.Name;
                if (!string.IsNullOrEmpty(userEntity.Password))
                {
                    user.Password = userEntity.Password;
                }
                user.Address = userEntity.Address;
                user.Email = userEntity.Email;
                user.ModifiedBy = userEntity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public User GetUserByUsername(string userName)
        {
            return db.Users.SingleOrDefault(x=>x.Username == userName);
        }

        public User GetUserById(int id)
        {
            return db.Users.Find(id);
        }

        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.Username == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
        }
        public int Login(string userName, string password, bool isAdminLogin )
        {
            var result = db.Users.SingleOrDefault(x => x.Username == userName || x.Email == userName);
            if(result== null)
            {
                return 0;
            }
            else
            {
                if (isAdminLogin == true )
                {
                    if(result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == password)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == password)
                            return 1;
                        else
                            return -2;
                    }
                }
                
            }
                      
        }
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckUserNameExist(string userName)
        {
            return db.Users.Count(x => x.Username == userName) > 0;
        }
        public bool CheckEmailExist(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
    }
}
