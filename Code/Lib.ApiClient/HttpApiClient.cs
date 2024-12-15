using System.Net.Http.Headers;
using System.Text;
using Lib.ApiClient.Abstractions;
using Lib.Results;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Lib.ApiClient;

public sealed class HttpApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpApiClient> _logger;

    public HttpApiClient(HttpClient httpClient, ILogger<HttpApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<T>> Get<T>(Uri uri, string token)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrEmpty(token);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrEmpty(responseBody))
                {
                    return new Result<T>();
                }

                var data = JsonConvert.DeserializeObject<T>(responseBody);

                return new Result<T>(data);
            }

            var message =
                $"Call to HTTP get endpoint '{uri.AbsoluteUri}' method failed with status code: '{response.StatusCode}' and message: '{responseBody}'.";

            _logger.LogError(message);

            return new Result<T>(Result.Error(message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            return new Result<T>(Result.Error(e.Message));
        }
    }

    public async Task<Result> Post(Uri uri, string token, object payload = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrEmpty(token);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var content = payload is null ? string.Empty : JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, httpContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var message =
                $"Call to HTTP post endpoint '{uri.AbsoluteUri}' method failed with status code: '{response.StatusCode}' and message: '{responseBody}'.";

            _logger.LogError(message);

            return Result.Error(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            return Result.Error(e.Message);
        }
    }
}
