using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCorePoc.Application.DTOs;
using NetCorePoc.Application.Interfaces;

namespace NetCorePoc.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private readonly IUserAppService _userApp;

        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserAppService userApp, ILogger<UsersController> logger)
        {
            _userApp = userApp;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Getting all users");
            var users = _userApp.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_userApp.GetUserById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserRequest user)
        {
            return Created(string.Empty, new { Id = _userApp.InsertUser(user) });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserRequest user)
        {
            if (_userApp.UpdatetUser(id, user))
                return Ok();

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_userApp.DeleteUser(id))
                return Ok();

            return NotFound();
        }
    }
}
