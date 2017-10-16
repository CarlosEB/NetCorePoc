using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCorePoc.Application.DTOs;
using NetCorePoc.Application.Interfaces;

namespace NetCorePoc.Controllers
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

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var users = _userApp.GetUsers();
            return Ok(users);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_userApp.GetUserById(id));
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]UserInput user)
        {
            return Ok( new { Id = _userApp.InsertUser(user) });            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]UserInput user)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
