//using Oreon.WebApi.Extensions;
//using Oreon.WebApi.Helpers;
//using Oreon.Application.Common.Pagination;
//using Oreon.Application.DTOs;
//using Oreon.Application.Features.Likes.Commands.AddLike;
//using Oreon.Application.Features.Likes.Queries.GetUserLikes;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Oreon.WebApi.Controllers
//{
//    [Authorize]
//    public class LikesController : BaseApiController
//    {
//        [HttpPost("{username}")]
//        public async Task<ActionResult> AddLike(string username)
//        {
//            await Sender.Send(new AddLikeCommand(User.GetUserId(), username));
//            return Ok();
//        }

//        [HttpGet]
//        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
//        {
//            likesParams.UserId = User.GetUserId();
//            var users = await Sender.Send(new GetUserLikesQuery(likesParams));
//            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
//            return Ok(users);
//        }
//    }
//}
