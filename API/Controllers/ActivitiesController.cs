using API.DTO;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetActivities(CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new List.Query(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ActivityDto>> GetActivity(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Details.Query(id), cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<ReactivityCreatedDto>> CreateActivity(Reactivity reactivity, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Create.Command(reactivity), cancellationToken));
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateActivity(Guid id, Reactivity reactivity, CancellationToken cancellationToken)
    {
        reactivity.Id = id;

        return HandleResult(await Mediator.Send(new Edit.Command(reactivity), cancellationToken));
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Delete.Command(id), cancellationToken));
    }

    [HttpPost("{id:guid}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command(id)));
    }
}
