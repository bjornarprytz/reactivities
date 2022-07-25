using Application.Comments;
using LanguageExt.UnsafeValueAccess;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IMediator mediator, ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task SendComment(Create.Command command)
    {
        var comment = await _mediator.Send(command);
        
        // TODO: Check Error
        
        await Clients.Group(command.ActivityId.ToString())
            .SendAsync("ReceiveComment", comment.ValueUnsafe());
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.GetHttpContext()?.Request.Query["activityId"]
            is not { } activityId)
        {
            _logger.LogError("Cannot find activityId in request");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, activityId!);
        var result = await _mediator.Send(new List.Query(Guid.Parse(activityId!)));
        // TODO: Check Error
        await Clients.Caller.SendAsync("LoadComments", result.ValueUnsafe());
        
        await base.OnConnectedAsync();
    }
}