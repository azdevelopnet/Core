using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
	public interface ISecureDataService
	{
        Task<(bool Success, Exception Error)>  SaveSecureData<T>(string name, T obj) where T : class, new();
        Task<(bool Success, Exception Error)> SaveSecureData<T>(string name, string password, T obj) where T : class, new();
        Task<(T Response, bool Success, Exception Error)> GetSecureData<T>(string name) where T : class, new();
        Task<(T Response, bool Success, Exception Error)> GetSecureData<T>(string name, string password) where T : class, new();
        Task DeleteSecureData(string name);
	}
}
