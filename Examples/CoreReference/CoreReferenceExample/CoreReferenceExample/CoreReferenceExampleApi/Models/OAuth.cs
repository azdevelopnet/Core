using System;
using System.Linq;

namespace CoreReferenceExampleApi.Models
{
    public class OAuthParams
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
    public class OAuthResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string meta_data { get; set; }
    }

    public class OAuthPerson : User
    {
        public string PhotoUrl { get; set; }
        public OAuthPerson(User p)
        {
            var properties = this.GetType().GetProperties();
            foreach (var prop in p.GetType().GetProperties())
            {
                if (properties.Any(x => x.Name == prop.Name))
                {
                    var v = prop.GetValue(p);
                    prop.SetValue(this, v);
                }
            }
        }
    }
}
