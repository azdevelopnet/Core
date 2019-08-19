using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace Xamarin.Forms.Core
{
    public class CacheService: ICacheService
	{
        private string _cacheFileName = "CacheHistory";
        private List<CacheHistory> _cacheHistory;
        private IFileStore _fileStore;

        public CacheService()
        {
            _fileStore = CoreDependencyService.GetService<IFileStore, FileStore>(true);
        }
        public async Task ClearExpired()
        {
            var now = DateTime.Now;
            await GetCacheHistory();
            for(var x = _cacheHistory.Count - 1; x > -1; x--)
            {
                var cache = _cacheHistory[x];
                if (cache.Expiration > now)
                {
                    await _fileStore.DeleteAsync(cache.Key);
                }
            }
            await _fileStore.SaveAsync<List<CacheHistory>>(_cacheFileName, _cacheHistory);

        }

        public async Task<T> GetData<T>(string key) where T : class, new()
        {
            var now = DateTime.Now;
            T response = null;
            await GetCacheHistory();
            var result = await _fileStore.GetAsync<T>(key);
            if(result.Success && result.Response != null)
            {
                var cache = _cacheHistory.FirstOrDefault(x => x.Key == key);
                if (cache!=null && cache.Expiration < now)
                {
                    await _fileStore.DeleteAsync(cache.Key);
                    _cacheHistory.Remove(cache);
                    await _fileStore.SaveAsync<List<CacheHistory>>(_cacheFileName, _cacheHistory);
                }
                else
                {
                    response = result.Response;
                }
            }

            return response;
        }

        public async Task<string> GetStringData(string key)
        {
            var now = DateTime.Now;
            string response = null;
            await GetCacheHistory();
            var result = await _fileStore.GetStringAsync(key);
            if (result.Success && result.Response != null)
            {
                var cache = _cacheHistory.FirstOrDefault(x => x.Key == key);
                if (cache != null && cache.Expiration < now)
                {
                    await _fileStore.DeleteAsync(cache.Key);
                    _cacheHistory.Remove(cache);
                    await _fileStore.SaveAsync<List<CacheHistory>>(_cacheFileName, _cacheHistory);
                }
                else
                {
                    response = result.Response;
                }
            }

            return response;
        }

        public async Task<bool> SaveData<T>(T obj, string key, DateTime expire) where T : class, new()
        {
            await GetCacheHistory();
            var result = await _fileStore.SaveAsync<T>(key, obj);
            if (result.Success)
            {

                var cache = _cacheHistory.FirstOrDefault(x => x.Key == key);
                if (cache != null)
                {
                    _cacheHistory.Remove(cache);
                }

                _cacheHistory.Add(new CacheHistory()
                {
                    Key = key,
                    Expiration = expire
                });

                var cacheSaveResult = await _fileStore.SaveAsync<List<CacheHistory>>(_cacheFileName, _cacheHistory);
                return cacheSaveResult.Success;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SaveStringData(string obj, string key, DateTime expire)
        {
            await GetCacheHistory();
            var result = await _fileStore.SaveStringAsync(key, obj);
            if (result.Success)
            {

                var cache = _cacheHistory.FirstOrDefault(x => x.Key == key);
                if (cache != null)
                {
                    _cacheHistory.Remove(cache);
                }

                _cacheHistory.Add(new CacheHistory()
                {
                    Key = key,
                    Expiration = expire
                });

                var cacheSaveResult = await _fileStore.SaveAsync<List<CacheHistory>>(_cacheFileName, _cacheHistory);
                return cacheSaveResult.Success;
            }
            else
            {
                return false;
            }
        }

        private async Task GetCacheHistory()
        {
            var historyResult = await _fileStore.GetAsync<List<CacheHistory>>(_cacheFileName);
            if (historyResult.Success)
                _cacheHistory = historyResult.Response;

            if (_cacheHistory == null)
                _cacheHistory = new List<CacheHistory>();

        }
    }
}
