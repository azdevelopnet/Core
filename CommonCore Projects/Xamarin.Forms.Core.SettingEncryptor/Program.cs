using System;
using System.IO;
using Newtonsoft.Json;

namespace Xamarin.Forms.Core.SettingEncryptor
{
    class Program
    {
        static void Main(string[] args)
        {
            var encryptionKey = "$1ngl3$0urc3";
            var temp = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            var exeFolder = temp.Replace("file:", string.Empty);
            var path = exeFolder.Replace("bin/Debug/net5.0", "Config");
            foreach(var file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".json"))
                {
                    var fileName = Path.GetFileName(file);
                    var str = File.ReadAllText(file);
                    var obj = JsonConvert.DeserializeObject<CoreConfiguration>(str);
                    var encrypted = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings()
                    {
                        ContractResolver = new EncryptedStringPropertyResolver(encryptionKey)
                    });


                    var uencrypted = JsonConvert.DeserializeObject<CoreConfiguration>(encrypted, new JsonSerializerSettings()
                    {
                        ContractResolver = new EncryptedStringPropertyResolver(encryptionKey)
                    });
                    var writeFolder = $"{exeFolder}/Config";
                    if (!Directory.Exists(writeFolder))
                    {
                        Directory.CreateDirectory(writeFolder);
                    }
                    File.WriteAllText($"{writeFolder}/{fileName}", encrypted);
                }
            }

            Console.WriteLine("Encrypted Config File Created");
        }
    }
}
