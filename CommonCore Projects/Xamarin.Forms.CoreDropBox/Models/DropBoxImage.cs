using System;
using System.IO;

namespace Xamarin.Forms.Core
{
    public class DropBoxImage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Base64Image { get; set; }
        public ImageSource ImageSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(Base64Image)));
            }
        }

        public DropBoxImage()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
