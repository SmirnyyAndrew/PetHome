using CSharpFunctionalExtensions;
using PetHome.SharedKernel.Responses.ErrorManagement;

namespace PetHome.Core.Application.Interfaces.FeatureManagement;
public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> Execute(TQuery query, CancellationToken ct);
}

public interface IQueryHandler<TResponse>
{
    public Task<TResponse> Execute(CancellationToken ct);
}
 
