using ECommerceAPI.Modules.Users.CustomModels;
using ECommerceAPI.Modules.Users.DTOs;
using ECommerceAPI.Modules.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Modules.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("SingUp")]
        public async Task<ActionResult<AuthModel>> SingUp(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.SingUp(model);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("SingIn")]
        public async Task<ActionResult<AuthModel>> SingIn(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.SingIn(model);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("StaySingingIn")]
        public async Task<ActionResult<AuthModel>> StaySingingIn(string refreshToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.StaySingingIn(refreshToken);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PagedResultUsers>> GetPagedUsersAsync([FromQuery] PagedRequestUsers request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Users = await _userService.GetPagedUsersAsync(request);

            return Ok(Users);

        }
    }




}
