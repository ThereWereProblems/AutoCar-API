using AutoCar.Models.DTO;
using AutoCar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoCar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("create")]
        public ActionResult RegisterUrer([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete([FromRoute] int id)
        {
            var IsDeleted = _accountService.Delete(id);
            if (IsDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);

            return Ok(token);
        }

        [HttpGet("getuser/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetUsers()
        {
            var listOfUsers = _accountService.GetAll();

            return Ok(listOfUsers);
        }

        [HttpGet("getusers")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetUser([FromRoute] int id)
        {
            var user = _accountService.GetUser(id);


            return Ok(user);
        }

        [HttpPatch("changepassword")]
        [Authorize]
        public ActionResult ChangePassword([FromBody] NewPasswordDto dto)
        {
            _accountService.ChangePassword(dto);
            return Ok();
        }

        [HttpPatch("edituser/{id}")]
        [Authorize]
        public ActionResult EditUser([FromBody] UserEditDto dto)
        {
            _accountService.EditUser(dto);
            return Ok();
        }

        [HttpPatch("changerole/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRole([FromRoute] int id, [FromBody] ChangeRoleDto dto)
        {
            _accountService.ChangeRole(id, dto);
            return Ok();
        }
    }
}
