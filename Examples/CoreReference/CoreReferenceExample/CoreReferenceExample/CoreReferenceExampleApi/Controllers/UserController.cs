using System;
using CoreReferenceExampleApi.Atrributes;
using CoreReferenceExampleApi.Data;
using CoreReferenceExampleApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreReferenceExampleApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController: Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("api/User/GetUserAccounts")]
        public IActionResult GetUserAccounts()
        {
            try
            {

                var list = _userService.Get();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpGet]
        [Route("api/User/GetUserById")]
        public IActionResult GetUserById(string id)
        {
            try
            {
                var user = _userService.Get(id);
                if (user != null)
                    user.Password = string.Empty;
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost]
        [Route("api/User/GetUsersByQuery")]
        public PaginatedList<User> GetUsersByQuery([FromBody] APIQuery query)
        {
            return _userService.GetByQuery(query);
        }

        [HttpPost]
        [Route("api/User/AddUser")]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                var exists = _userService.GetFirstOrDefault(User => User.Email == user.Email);
                if (exists == null)
                {
                    user.UpdatedAt = DateTime.Now;
                    _userService.Create(user);
                    return Ok(user);
                }
                else
                {
                    return BadRequest(new ApplicationException("Account already exists"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("api/User/UpdateUser")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            try
            {
                user.UpdatedAt = DateTime.Now;
                _userService.Update(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost]
        [Route("api/User/DeleteUser")]
        public IActionResult DeleteUser([FromBody] User user)
        {
            try
            {
                var exists = _userService.GetFirstOrDefault(User => User.Id == user.Id);
                if (exists != null)
                    _userService.Remove(exists.Id);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
