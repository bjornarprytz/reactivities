using Application.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        return HandleResult(await Mediator.Send(new Details.Query(username)));
    }

    [HttpPut]
    public async Task<IActionResult> EditProfile(EditBio.Command edit)
    {
        return HandleResult(await Mediator.Send(edit));
    }

}