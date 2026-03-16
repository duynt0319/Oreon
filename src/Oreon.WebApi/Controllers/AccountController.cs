//using Oreon.Application.DTOs;
//using Oreon.Application.Features.Account.Commands.Login;
//using Oreon.Application.Features.Account.Commands.Register;
//using Microsoft.AspNetCore.Mvc;

//namespace Oreon.WebApi.Controllers
//{
//    public class AccountController : BaseApiController
//    {
//        [HttpPost("register")]
//        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
//            => Ok(await Sender.Send(new RegisterCommand(registerDto)));

//        [HttpPost("login")]
//        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
//            => Ok(await Sender.Send(new LoginCommand(loginDto)));
//    }
//}
