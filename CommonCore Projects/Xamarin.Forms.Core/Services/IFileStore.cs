using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
	public interface IFileStore
	{
        Task<(T Response, bool Success, Exception Error)> GetAsync<T>(string contentName) where T : class, new();
		Task<(bool Success, Exception Error)> DeleteAsync(string contentName);
		Task<(bool Success, Exception Error)> SaveAsync<T>(string contentName, object obj);
        Task<(string Response, bool Success, Exception Error)> GetStringAsync(string contentName);
		Task<(bool Success, Exception Error)> SaveStringAsync(string contentName, string obj);
	}
}
