using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCorePoc.Application.DTOs;
using NetCorePoc.Application.Interfaces;

namespace NetCorePoc.Api.Controllers
{
    /// <summary>
    /// Controller used to handle users.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private readonly IUserAppService _userApp;

        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Constructor class
        /// </summary>
        /// <param name="userApp"></param>
        /// <param name="logger"></param>
        public UsersController(IUserAppService userApp, ILogger<UsersController> logger)
        {
            _userApp = userApp;
            _logger = logger;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Getting all users");
            var users = _userApp.GetUsers();
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID.
        /// </summary>
        /// <param name="id"></param>  
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_userApp.GetUserById(id));
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user"></param> 
        [HttpPost]
        public IActionResult Post([FromBody]UserRequest user)
        {
            return Created(string.Empty, new { Id = _userApp.InsertUser(user) });
        }

        /// <summary>
        /// Update user by Id
        /// </summary>
        /// <param name="id"></param>  
        /// <param name="user"></param>  
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserRequest user)
        {
            if (_userApp.UpdatetUser(id, user))
                return Ok();

            return NotFound();
        }

        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <param name="id"></param>  
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_userApp.DeleteUser(id))
                return Ok();

            return NotFound();
        }
    }
}
