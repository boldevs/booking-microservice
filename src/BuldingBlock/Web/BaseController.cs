using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BuldingBlock.Web
{
   // [Route(BaseApiPath)]
    [ApiController]
    [ApiVersion("1.0")]
    public abstract class BaseController : ControllerBase
    {
        private IMapper _mapper;

        private IMediator _mediator;

        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
    }

}
