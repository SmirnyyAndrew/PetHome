using CSharpFunctionalExtensions;
using PetHome.Core.Response.Validation.Validator;

namespace PetHome.Core.Interfaces.FeatureManagment;
public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> Execute(TQuery query, CancellationToken ct);
}

public interface IQueryHandler<TResponse>
{
    public Task<TResponse> Execute(CancellationToken ct);
}
 
