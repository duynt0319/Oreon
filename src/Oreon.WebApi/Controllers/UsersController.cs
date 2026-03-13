//using Oreon.WebApi.Extensions;
//using Oreon.WebApi.Helpers;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using Oreon.Application.Features.Users.Commands.AddPhoto;
//using Oreon.Application.Features.Users.Commands.DeletePhoto;
//using Oreon.Application.Features.Users.Commands.SetMainPhoto;
//using Oreon.Application.Features.Users.Commands.UpdateMember;
//using Oreon.Application.Features.Users.Queries.GetMemberByUsername;
//using Oreon.Application.Features.Users.Queries.GetMembers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Oreon.WebApi.Controllers
//{
//    [Authorize]
//    public class UsersController : BaseApiController
//    {
//        [HttpGet]
//        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
//        {
//            var gender = await Sender.Send(new GetMemberGenderQuery(User.GetUsername()));
//            userParams.CurrentUsername = User.GetUsername();
//            if (string.IsNullOrEmpty(userParams.Gender))
//                userParams.Gender = gender == "male" ? "female" : "male";

//            var members = await Sender.Send(new GetMembersQuery(userParams));
//            Response.AddPaginationHeader(new PaginationHeader(members.CurrentPage, members.PageSize, members.TotalCount, members.TotalPages));
//            return Ok(members);
//        }

//        [HttpGet("{username}")]
//        public async Task<ActionResult<MemberDto>> GetUser(string username)
//            => Ok(await Sender.Send(new GetMemberByUsernameQuery(username)));

//        [HttpPut]
//        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
//        {
//            await Sender.Send(new UpdateMemberCommand(User.GetUsername(), memberUpdateDto));
//            return NoContent();
//        }

//        [HttpPost("add-photo")]
//        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
//        {
//            var photo = await Sender.Send(new AddPhotoCommand(User.GetUsername(), file));
//            return CreatedAtAction(nameof(GetUser), new { username = User.GetUsername() }, photo);
//        }

//        [HttpPut("set-main-photo/{photoId}")]
//        public async Task<ActionResult> SetMainPhoto(int photoId)
//        {
//            await Sender.Send(new SetMainPhotoCommand(User.GetUsername(), photoId));
//            return NoContent();
//        }

//        [HttpDelete("delete-photo/{photoId}")]
//        public async Task<ActionResult> DeletePhoto(int photoId)
//        {
//            await Sender.Send(new DeletePhotoCommand(User.GetUsername(), photoId));
//            return Ok();
//        }
//    }
//}
