using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoreReferenceExampleApi.Atrributes;
using CoreReferenceExampleApi.Data;
using CoreReferenceExampleApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CoreReferenceExampleApi.Controllers
{
    [ApiKeyAuthAttribute]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        //private readonly IMemoryCache _cache;
        private readonly IUserService _userService;

        public AuthenticationController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            //_cache = cache;
            _userService = userService;
        }


        #region Authorization
        [HttpPost]
        [Route("api/Authentication/Authorize")]
        public IActionResult Authorize([FromBody] OAuthParams oAuth)
        {
            if (oAuth.grant_type == "password")
            {
                var auth = GrantAuthorization(oAuth);
                if (auth != null)
                    return Ok(auth);
                else
                    return BadRequest("Could not create token");
            }
            //else if (oAuth.grant_type == "refresh_token")
            //{
            //    var auth = RefreshAuthorization(oAuth);
            //    if (auth != null)
            //        return Ok(auth);
            //    else
            //        return BadRequest("Could not create token");
            //}
            else
            {
                return BadRequest(new ApplicationException("Invalid authorization request"));
            }

        }


        private OAuthResponse GrantAuthorization(OAuthParams oAuth)
        {
            try
            {
                var person = _userService.GetFirstOrDefault(User => User.Email == oAuth.email && User.Password == oAuth.password && User.Active == true);

                if (person != null)
                {
                    var obj = new OAuthPerson(person);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, oAuth.email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                          _configuration["Tokens:Issuer"],
                          claims,
                          expires: DateTime.Now.AddHours(2),
                          signingCredentials: creds);

                    var refreshToken = Guid.NewGuid().ToString().Replace("-", string.Empty).Trim();
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(8));
                    //_cache.Set(refreshToken, oAuth.email, cacheEntryOptions);

                    return new OAuthResponse()
                    {
                        access_token = new JwtSecurityTokenHandler().WriteToken(token),
                        expires_in = (int)TimeSpan.FromHours(2).TotalSeconds,
                        refresh_token = refreshToken,
                        meta_data = JsonConvert.SerializeObject(obj)
                    };

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //private OAuthResponse RefreshAuthorization(OAuthParams oAuth)
        //{
        //    string userName = null;
        //    if (_cache.TryGetValue(oAuth.refresh_token, out userName))
        //    {
        //        if (!string.IsNullOrEmpty(userName))
        //        {
        //            var claims = new[]
        //            {
        //                new Claim(JwtRegisteredClaimNames.Sub, userName),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            };
        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
        //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
        //                  _configuration["Tokens:Issuer"],
        //                  claims,
        //                  expires: DateTime.Now.AddHours(2),
        //                  signingCredentials: creds);

        //            return new OAuthResponse()
        //            {
        //                access_token = new JwtSecurityTokenHandler().WriteToken(token),
        //                expires_in = (int)TimeSpan.FromHours(2).TotalSeconds,
        //                refresh_token = oAuth.refresh_token,
        //            };

        //        }
        //    }

        //    return null;
        //}


        #endregion
    }
}
