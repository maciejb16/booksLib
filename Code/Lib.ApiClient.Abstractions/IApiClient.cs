using Lib.Results;

namespace Lib.ApiClient.Abstractions;

public interface IApiClient
{
    Task<Result<T>> Get<T>(Uri uri, string token);

    Task<Result> Post(Uri uri, string token, object payload);
}
