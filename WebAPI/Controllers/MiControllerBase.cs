using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller")]
    public class MiControllerBase : ControllerBase
    {
        private IMediator _mediator;

        // Instancia de IMediator. Si es nulo, obtiene el servicio
        protected IMediator Mediator => _mediator ?? (_mediator =  HttpContext.RequestServices.GetService<IMediator>());

    }
}