using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xamarin.Forms.Core
{
    public partial class CoreBusiness
    {
        #region ReadOnly AppData Settings
        [JsonIgnore]
        public string AESEncryptionKey { get { return CoreSettings.Config.AESEncryptionKey; } }
        [JsonIgnore]
        public Dictionary<string, string> WebApis { get { return CoreSettings.Config?.WebApi; } }
        [JsonIgnore]
        public Dictionary<string, string> CustomSettings { get { return CoreSettings.Config?.CustomSettings; } }
        #endregion

        #region Injection Services


        /// <summary>
        /// Service that provides network calls over http.
        /// </summary>
        /// <value>The http service.</value>
        [JsonIgnore]
        protected IHttpService HttpService
        {
            get
            {
                return (IHttpService)CoreDependencyService.GetService<IHttpService, HttpService>(true);
            }
        }

        /// <summary>
        /// Embedded file store that allow objects to be json serialized.
        /// </summary>
        /// <value>The file store.</value>
        [JsonIgnore]
        protected IFileStore FileStore
        {
            get
            {
                return (IFileStore)CoreDependencyService.GetService<IFileStore, FileStore>(true);
            }
        }

        /// <summary>
        /// Service that uses the OS account store to retrieve dictionary data
        /// </summary>
        /// <value>The account service.</value>
        [JsonIgnore]
        protected ISecureDataService SecureDataService
        {
            get
            {
                return (ISecureDataService)CoreDependencyService.GetService<ISecureDataService, SecureDataService>(true);
            }
        }

        /// <summary>
        /// AES encryption and Hash service.
        /// </summary>
        /// <value>The encryption service.</value>
        [JsonIgnore]
        protected IEncryptionService EncryptionService
        {
            get
            {
                return (IEncryptionService)CoreDependencyService.GetService<IEncryptionService, EncryptionService>(true);
            }
        }

		#endregion
    }
}
