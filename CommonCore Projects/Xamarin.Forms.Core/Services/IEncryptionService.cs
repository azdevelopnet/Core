using System;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
	public interface IEncryptionService
	{
        Task<string> AesEncrypt(string clearValue, string encryptionKey);
        Task<string> AesDecrypt(string encryptedValue, string encryptionKey);
		byte[] GetHash(string input, Encoding encoding);
		byte[] GetHash(string input);
		string GetHashString(byte[] input);
		string GetHashString(string input, Encoding encoding);
		string GetHashString(string input);
		byte[] GetHash(byte[] input);
	}
}
