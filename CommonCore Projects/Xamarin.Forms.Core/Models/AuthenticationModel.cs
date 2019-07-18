using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Core
{
	public class AuthenticationModel
	{
        private int _expiresIn;

		public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn 
        {
            get { return _expiresIn; }
            set
            {
                _expiresIn = value;
                this.UTCExpiration = DateTime.UtcNow.AddSeconds(ExpiresIn).Ticks;
            }
        }
		public Dictionary<string, string> MetaData { get; set; }
        public long UTCExpiration { get; set; }

        public bool TokenIsValid
        {
            get
            {
                return UTCExpiration > DateTimeOffset.UtcNow.Ticks;
            }
        }

	}
}
