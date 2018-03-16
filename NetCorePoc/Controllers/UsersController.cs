using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCorePoc.Application.DTOs;
using NetCorePoc.Application.Interfaces;

namespace NetCorePoc.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private readonly IUserAppService _userApp;

        public UsersController(IUserAppService userApp)
        {
            _userApp = userApp;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _userApp.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_userApp.GetUserById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserInput user)
        {
            return Created(string.Empty, new { Id = _userApp.InsertUser(user) });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserInput user)
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
