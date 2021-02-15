using System;
using Xamarin.Forms.Core;

namespace Xamarin.Forms.Core
{
    public partial class CoreSettings
    {
        public class BuildEnv
        {
            public const string Dev = "dev";
            public const string QA = "qa";
            //public const string UAT = "uat";
            public const string PROD = "prod";
        }

        public static void DecryptLocalSettings(string encryptionKey)
        {
            CoreSettings.JsonEncryptionKey = encryptionKey;
            CoreSettings.GlobalInit();
        }

        public static void Start()
        {
            CoreSettings.GlobalInit();
            CoreSettings.LocalInit();
        }

        public static void LocalInit()
        {

        }

    }
}
