using LanguageExt;
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

    protected ActionResult HandlePagedResult<T>(Either<Error, PagedList<T>> result)
    {
        return (result.State) switch
        {
            EitherStatus.IsLeft => BadRequest(result.LeftToSeq().Head),
            EitherStatus.IsRight => Ok(HandleSuccess()),
            _ => NotFound(),
        };

        PagedList<T> HandleSuccess()
        {
            var pagedResult = result.ValueUnsafe();

            Response.AddPaginationHeader(pagedResult);

            return pagedResult;
        }
    }
}