using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Permissions;
using Todo.Models.Dtos;
using Todo.Models.Dtos.Requests;
using Todo.Services.Services;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Private Params
        private readonly UserService _userService;
        private readonly TolkenService _tolkenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public AuthController(UserService userService, TolkenService tolkenService, IHttpContextAccessor httpContextAccessor)
        {
            this._userService = userService;
            this._tolkenService = tolkenService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region POST
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] LoginDto model)
        {
            var user = await _userService.GetUser(model.username, model.password);
            if (user is null)
                return NotFound(new { message = "Usuerio ou senha invalidos" });

            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("user", JsonConvert.SerializeObject(user));

            var token = _tolkenService.GenerateTolken(user);
            user.Password = "****************";
            return new
            {
                user = user,
                tolken = token
            };
        }
        #endregion
    }
}
