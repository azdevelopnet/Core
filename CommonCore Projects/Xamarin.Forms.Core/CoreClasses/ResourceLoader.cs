using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Xamarin.Forms.Core
{
	/// <summary>
	/// Utility class that can be used to find and load embedded resources into memory.
	/// </summary>
	public static class ResourceLoader
	{
		/// <summary>
		/// Attempts to find and return the given resource from within the specified assembly.
		/// </summary>
		/// <returns>The embedded resource stream.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="resourceFileName">Resource file name.</param>
		public static (Stream Response, Exception Error) GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
		{
            (Stream Response, Exception Error) response = (null, null);
			var resourceNames = assembly.GetManifestResourceNames();

			var resourcePaths = resourceNames
				.Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
				.ToArray();

			if (!resourcePaths.Any())
			{
				response.Error = new Exception(string.Format("Resource ending with {0} not found.", resourceFileName));
			}

			if (resourcePaths.Count() > 1)
			{
				response.Error = new Exception(string.Format("Multiple resources ending with {0} found: {1}{2}", resourceFileName, Environment.NewLine, string.Join(Environment.NewLine, resourcePaths)));
			}

            response.Response = assembly.GetManifestResourceStream(resourcePaths.Single());
            return response;

		}

		/// <summary>
		/// Attempts to find and return the given resource from within the specified assembly.
		/// </summary>
		/// <returns>The embedded resource as a byte array.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="resourceFileName">Resource file name.</param>
		public static byte[] GetEmbeddedResourceBytes(Assembly assembly, string resourceFileName)
		{
			var result = GetEmbeddedResourceStream(assembly, resourceFileName);
            if (result.Error==null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    result.Response.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            else
            {
                return new byte[0];
            }

		}

		/// <summary>
		/// Attempts to find and return the given resource from within the specified assembly.
		/// </summary>
		/// <returns>The embedded resource as a string.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="resourceFileName">Resource file name.</param>
		public static (string Response, Exception Error) GetEmbeddedResourceString(Assembly assembly, string resourceFileName)
		{
			var result = GetEmbeddedResourceStream(assembly, resourceFileName);
            if (result.Error==null)
            {
                using (var streamReader = new StreamReader(result.Response))
                {
                    var stream=  streamReader.ReadToEnd();
                    return (stream, null);
                }
            }
            else{
                return (null, result.Error);
            }

		}
	}
}


