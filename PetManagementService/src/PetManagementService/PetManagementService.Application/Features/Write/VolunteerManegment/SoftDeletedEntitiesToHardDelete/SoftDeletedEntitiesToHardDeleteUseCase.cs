﻿using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetHome.Core.Infrastructure.Database;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.SoftDeletedEntitiesToHardDelete;
public class SoftDeletedEntitiesToHardDeleteUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SoftDeletedEntitiesToHardDeleteUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public SoftDeletedEntitiesToHardDeleteUseCase(
        IVolunteerRepository volunteerRepository,
        IPublishEndpoint publisher,
        ILogger<SoftDeletedEntitiesToHardDeleteUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _publisher = publisher;
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int, ErrorList>> Execute(CancellationToken ct)
    {
        List<Volunteer> volunteersToHardDelete = _volunteerRepository
            .GetDeleted(ct)
            .Where(x => x.DeletionDate != default(DateTime) && (DateTime.UtcNow - x.DeletionDate).Days > 30)
            .ToList();
        if (volunteersToHardDelete.Count == 0)
            return 0;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        _volunteerRepository.Remove(volunteersToHardDelete);
        await _unitOfWork.SaveChanges(ct);  
        transaction.Commit();

        int deletedCount = volunteersToHardDelete.Count;
        _logger.LogInformation("Было hard deleted {0} волонётра(-ов)", deletedCount);

        return deletedCount;
    }
}
