using LanguageExt;
using LanguageExt.Common;
using LanguageExt.UnsafeValueAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Error = Application.Core.Error;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected ActionResult HandleResult<T>(Either<Error, T> result)
    {
        return (result.State) switch
        {
            EitherStatus.IsLeft => BadRequest(result.LeftToSeq().Head),
            EitherStatus.IsRight => Ok(result.ValueUnsafe()),
            _ => NotFound(),
        };
    }
}