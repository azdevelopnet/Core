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
    public interface IMessageService
    {
        Message Create(Message usr);
        List<Message> Get();
        Message Get(string id);
        List<Message> GetBy(Expression<Func<Message, bool>> exp);
        PaginatedList<Message> GetByQuery(APIQuery query);
        Message GetFirstOrDefault(Expression<Func<Message, bool>> exp);
        void Remove(Message usr);
        void Remove(string id);
        void Update(Message usr);
    }

    public class MessageService: IMessageService
    {
        private readonly ILiteCollection<Message> messages;

        public MessageService(IConfiguration config)
        {
            var db = new LiteDatabase(config.GetConnectionString("AppDb"));
            messages = db.GetCollection<Message>("Messages");
        }

        public List<Message> Get()
        {
            return messages.FindAll().ToList();
        }

        public Message Get(string id)
        {
            return messages.FindOne(msg => msg.Id == id);
        }

        public Message Create(Message msg)
        {
            if (Get(msg.Id) == null)
            {
                messages.Insert(msg);
                return msg;
            }
            else
            {
                return null;
            }

        }

        public void Update(Message msg)
        {
            messages.Update(msg);
        }

        public void Remove(Message msg)
        {
            messages.Delete(msg.Id);
        }

        public void Remove(string id)
        {
            messages.Delete(id);
        }

        public List<Message> GetBy(Expression<Func<Message, bool>> exp)
        {
            return messages.Find(exp).ToList();
        }
        public Message GetFirstOrDefault(Expression<Func<Message, bool>> exp)
        {
            return messages.Find(exp).FirstOrDefault();
        }

        public PaginatedList<Message> GetByQuery(APIQuery query)
        {
            try
            {
                var lst = messages.Query()
                    .CreateSearchQuery(query.Search)
                    .CreateFilterQuery(query.Filter)
                    .CreateOrderBy(query.Sort)
                    .CreatePaginatedList<Message>(query);

                return lst;
            }
            catch (Exception ex)
            {
                return new PaginatedList<Message>();
            }
        }

    }
}
