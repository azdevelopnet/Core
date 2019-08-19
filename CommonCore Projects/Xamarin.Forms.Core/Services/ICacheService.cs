using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
    public class CacheHistory
	{
        public string Key { get; set; }
        public DateTime Expiration { get; set; }
        
	}
    public interface ICacheService
    {
        Task<T> GetData<T>(string key) where T : class, new();
        Task<bool> SaveData<T>(T obj, string key, DateTime expire) where T : class, new();
        Task<bool> SaveStringData(string obj, string key, DateTime expire);
        Task<string> GetStringData(string key);
        Task ClearExpired();
    }
}
