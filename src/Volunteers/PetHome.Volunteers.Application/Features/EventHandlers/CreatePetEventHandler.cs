using MediatR;
using Microsoft.Extensions.Logging;
using PetHome.Volunteers.Domain.Events;

namespace PetHome.Volunteers.Application.Features.EventHandlers;
public class CreatePetEventHandler : INotificationHandler<CreatedPetEvent>

{
    private readonly ILogger<CreatePetEventHandler> _logger;

    public CreatePetEventHandler(ILogger<CreatePetEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(CreatedPetEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DOMAIN EVENT: В {0} был создал питомец с id = {1}", DateTime.UtcNow, notification.PetId); 
        await Task.CompletedTask;
    }
}
