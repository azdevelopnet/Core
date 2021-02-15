using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;

namespace Xamarin.Forms.Core
{
    public class LiteDbModel
    {
        public string Id { get; set; }
    }

    public interface ILiteNoSql
    {
        Task<(List<T> Response, Exception Error)> GetAll<T>() where T : LiteDbModel, new();
        Task<(T Response, Exception Error)> GetById<T>(string _objectId) where T : LiteDbModel, new();
        Task<(bool Success, Exception Error)> Insert<T>(T obj) where T : LiteDbModel, new();
        Task<(bool Success, Exception Error)> Delete<T>(T obj) where T : LiteDbModel, new();
        Task<(bool Success, Exception Error)> Delete<T>(string _objectId) where T : LiteDbModel, new();
        Task<(bool Success, Exception Error)> Update<T>(T obj) where T : LiteDbModel, new();
        Task<(List<T> Response, Exception Error)> Get<T>(Expression<Func<T, bool>> exp) where T : LiteDbModel, new();
        Task BulkSync<T>(List<T> list) where T : LiteDbModel, new();
        Task<(bool Success, Exception Error)> DeleteAll<T>() where T : LiteDbModel, new();
    }
}
