using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Oreon.WebApi.Controllers
{
    // [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private ISender _sender;
        protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
