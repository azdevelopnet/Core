using System;
namespace CoreReferenceExample.Models
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
}
