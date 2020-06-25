using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoreReferenceExampleApi.Models;
using LiteDB;
using Microsoft.Extensions.Configuration;
using CoreReferenceExampleApi.Extensions;

namespace CoreReferenceExampleApi.Data
{
    public interface IUserService
    {
        User Create(User usr);
        List<User> Get();
        User Get(string id);
        List<User> GetBy(Expression<Func<User, bool>> exp);
        PaginatedList<User> GetByQuery(APIQuery query);
        User GetFirstOrDefault(Expression<Func<User, bool>> exp);
        void Remove(User usr);
        void Remove(string id);
        void Update(User usr);
    }

    public class UserService : IUserService
    {
        private readonly ILiteCollection<User> users;

        public UserService(IConfiguration config)
        {
            var db = new LiteDatabase(config.GetConnectionString("AppDb"));
            users = db.GetCollection<User>("Users");
        }

        public List<User> Get()
        {
            return users.FindAll().ToList();
        }

        public User Get(string id)
        {
            return users.FindOne(usr => usr.Id == id);
        }

        public User Create(User usr)
        {
            if (Get(usr.Id) == null)
            {
                usr.Id = Guid.NewGuid().ToString();
                users.Insert(usr);
                return usr;
            }
            else
            {
                return null;
            }
            
        }

        public void Update(User usr)
        {
            users.Update(usr);
        }

        public void Remove(User usr)
        {
            users.Delete(usr.Id);
        }

        public void Remove(string id)
        {
            users.Delete(id);
        }

        public List<User> GetBy(Expression<Func<User, bool>> exp)
        {
            return users.Find(exp).ToList();
        }
        public User GetFirstOrDefault(Expression<Func<User, bool>> exp)
        {
            return users.Find(exp).FirstOrDefault();
        }

        public PaginatedList<User> GetByQuery(APIQuery query)
        {
            try
            {
                var lst = users.Query()
                    .CreateSearchQuery(query.Search)
                    .CreateFilterQuery(query.Filter)
                    .CreateOrderBy(query.Sort)
                    .CreatePaginatedList<User>(query);

                return lst;
            }
            catch (Exception ex)
            {
                return new PaginatedList<User>();
            }
        }
    }
}
