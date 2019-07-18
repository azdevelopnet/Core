using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Xamarin.Forms.Core
{
    public class SecureDataService : ISecureDataService
    {
        public IEncryptionService Encryption
        {
            get { return CoreDependencyService.GetService<IEncryptionService, EncryptionService>(true); }
        }

        public async Task<(T Response, bool Success, Exception Error)> GetSecureData<T>(string name) where T : class, new()
        {
            var result = await RetrieveSecureData<T>(name);
            var success = result.Error == null ? true : false;
            return (result.Response, success, result.Error);
        }

        public async Task<(T Response, bool Success, Exception Error)> GetSecureData<T>(string name, string password) where T : class, new()
        {
            var result = await RetrieveSecureData<T>(name, password);
            var success = result.Error == null ? true : false;
            return (result.Response, success, result.Error);
        }

        public async Task<(bool Success, Exception Error)> SaveSecureData<T>(string name, T obj) where T : class, new()
        {
            var result = await PersistSecureData<T>(name, null, obj);
            var success = result == null ? true : false;
            return (success, result);
        }

        public async Task<(bool Success, Exception Error)> SaveSecureData<T>(string name, string password, T obj) where T : class, new()
        {
            var result = await PersistSecureData<T>(name, password, obj);
            var success = result == null ? true : false;
            return (success, result);
        }

        private async Task<Exception> PersistSecureData<T>(string name, string password, T obj) where T : class, new()
        {
            try
            {
                var data = JsonConvert.SerializeObject(obj);
                if (!string.IsNullOrEmpty(password))
                    data = Encryption.AesEncrypt(data, password);
                await SecureStorage.SetAsync(name, data);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }
        private async Task<(T Response, Exception Error)> RetrieveSecureData<T>(string name, string password = null) where T : class, new()
        {
            try
            {
                var data = await SecureStorage.GetAsync(name);
                if (!string.IsNullOrEmpty(password))
                    data = Encryption.AesDecrypt(data, password);
                var obj = JsonConvert.DeserializeObject<T>(data);
                return (obj, null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }

        }

    }
}

