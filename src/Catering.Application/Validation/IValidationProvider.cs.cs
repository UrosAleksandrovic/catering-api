using Catering.Application.Results;

namespace Catering.Application.Validation;

public interface IValidationProvider
{
    Result ValidateModel<T>(T model);
    Result ValidateModel(Type type, object model);
    Task<Result> ValidateModelAsync<T>(T model);

}
