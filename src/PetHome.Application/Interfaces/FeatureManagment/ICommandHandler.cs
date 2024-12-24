using CSharpFunctionalExtensions;
using PetHome.Application.Validator;

namespace PetHome.Application.Interfaces.FeatureManagment;
public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, ErrorList>> Execute(TCommand command, CancellationToken ct);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<ErrorList>> Execute(TCommand command, CancellationToken ct);
}
