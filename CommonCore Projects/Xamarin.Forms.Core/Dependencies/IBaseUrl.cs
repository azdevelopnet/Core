using System;
namespace Xamarin.Forms.Core
{
    public interface IBaseUrl
    {
        string Get();
        string ReadContent(string fileName);
    }
}
