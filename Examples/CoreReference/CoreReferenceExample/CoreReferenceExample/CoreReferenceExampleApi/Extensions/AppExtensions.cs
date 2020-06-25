using System;
using System.Collections.Generic;
using System.Linq;
using CoreReferenceExampleApi.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using LiteDB;

namespace CoreReferenceExampleApi.Extensions
{
    public static class AppExtensions
    {
        public static PaginatedList<T> CreatePaginatedList<T>(this ILiteQueryable<T> query, APIQuery apiQuery) where T : class
        {
            int pageIndex = apiQuery.PageIndex.HasValue ? apiQuery.PageIndex.Value : 0;
            int pageSize = apiQuery.PageSize.HasValue ? apiQuery.PageSize.Value : 0;
            int count = 0;
            List<T> items = null;


            if (string.IsNullOrEmpty(apiQuery.Sort) && typeof(T).GetProperty("Id") != null)
            {
                query = query.OrderBy("Id");
            }

            if (typeof(T).GetProperty("IsActive") != null)
            {
                if (!apiQuery.IncludeAll)
                    query = query.Where($"IsActive = true");
            }

            if (apiQuery.PageSize.HasValue)
            {
   
                count = query.Count();
                items = query.Skip(pageIndex * pageSize).Limit(pageSize).ToList();

            }
            else
            {
                items = query.ToList();
                count = items.Count;
            }
            return new PaginatedList<T>(items, count, pageIndex, pageSize);

        }
        public static ILiteQueryable<T> CreateOrderBy<T>(this ILiteQueryable<T> query, string sort) where T : class
        {
            if (!string.IsNullOrEmpty(sort))
            {
                var col = sort.Split(" ");
                if (sort.IndexOf("Asc") != -1 || sort.IndexOf("asc") != -1)
                {
                    query = query.OrderBy(col[0]);
                }
                else
                {
                    query = query = query.OrderBy(col[0] + " descending");
                }
                return query;
            }
            else
            {
                return query;
            }

        }

        public static ILiteQueryable<T> CreateFilterQuery<T>(this ILiteQueryable<T> query, string filter) where T : class
        {
            if (!string.IsNullOrEmpty(filter))
            {
                return query.Where(filter);
            }
            else
            {
                return query;
            }

        }
        public static ILiteQueryable<T> CreateSearchQuery<T>(this ILiteQueryable<T> query, string search) where T : class
        {
            if (!string.IsNullOrEmpty(search))
            {
                var properties = typeof(T)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(o => o.CanRead && o.CanWrite)
                    .Where(o => o.PropertyType == typeof(string));
                var builder = new StringBuilder();
                foreach (var prop in properties)
                {
                    builder.Append($"{prop.Name}.Contains(\"{search}\") || ");
                }
                var temp = builder.ToString();
                var index = temp.LastIndexOf("||");
                return query.Where(temp.Substring(0, index));
            }
            else
            {
                return query;
            }

        }
    }
}
