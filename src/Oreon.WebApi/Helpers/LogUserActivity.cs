//using Oreon.WebApi.Extensions;
//using Oreon.Application.Abstractions.Persistence;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace Oreon.WebApi.Helpers
//{
//    public class LogUserActivity : IAsyncActionFilter
//    {
//        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        {
//            var resultContext = await next();

//            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

//            var userId = resultContext.HttpContext.User.GetUserId();
//            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

//            var user = await uow.Users.GetByIdAsync(userId);
//            user?.UpdateLastActive();
//            uow.Users.Update(user);
//            await uow.CompleteAsync();
//        }
//    }
//}
