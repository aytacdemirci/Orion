using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Orion.API.Controllers.SeedWork
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrionBaseController : ControllerBase
    {
        private IMediator _mediatr;
        protected IMediator Mediator => _mediatr ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
