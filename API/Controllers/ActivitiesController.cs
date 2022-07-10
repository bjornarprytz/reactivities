﻿using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[AllowAnonymous]
public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Reactivity>>> GetActivities(CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new List.Query(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Reactivity>> GetActivity(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Details.Query(id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Reactivity reactivity, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Create.Command(reactivity), cancellationToken));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateActivity(Guid id, Reactivity reactivity, CancellationToken cancellationToken)
    {
        reactivity.Id = id;
        
        return HandleResult(await Mediator.Send(new Edit.Command(reactivity), cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Delete.Command(id), cancellationToken));
    }
}
