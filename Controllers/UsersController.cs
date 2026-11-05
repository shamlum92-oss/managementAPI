using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // In-memory data store for testing
        private static readonly List<User> Users = new()
        {
            new User { Id = 1, Name = "Alice Nsubuga", Email = "alice@techhive.com", Role = "HR Specialist", Department = "HR" },
            new User { Id = 2, Name = "Brian Okello", Email = "brian@techhive.com", Role = "Systems Admin", Department = "IT" }
        };

        // 1. GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(Users);
        }

        // 2. GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} was not found." });
            }
            return Ok(user);
        }

        // 3. POST: api/users
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User newUser)
        {
            if (newUser == null || string.IsNullOrWhiteSpace(newUser.Name))
            {
                return BadRequest(new { message = "Invalid user data provided." });
            }

            newUser.Id = Users.Max(u => u.Id) + 1;
            Users.Add(newUser);

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // 4. PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} was not found." });
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.Role = updatedUser.Role;
            existingUser.Department = updatedUser.Department;

            return NoContent();
        }

        // 5. DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} was not found." });
            }

            Users.Remove(user);
            return NoContent();
        }
    }
}