using System.Net.Http.Json;
using System.Text.Json;
using Core.Dto.Auth;
using Core.Dto.Common;

namespace Core.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/auth/register", request, _jsonOptions, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>(_jsonOptions, cancellationToken);
            return result ?? throw new InvalidOperationException("Received empty response from server.");
        }

        // Handle validation or application errors natively (RFC 7807 integration)
        if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity ||
            response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>(_jsonOptions, cancellationToken);
            if (problem?.Errors != null && problem.Errors.Count > 0)
            {
                // Format validation errors into a single readable string message
                var errorMessages = problem.Errors.Select(e => $"{string.Join(", ", e.Value)}");
                throw new ApplicationException(string.Join("\n", errorMessages));
            }

            throw new ApplicationException(problem?.Detail ?? "Validation failed on the server side.");
        }

        throw new HttpRequestException("An unexpected error occurred during network operation.", null, response.StatusCode);
    }
}
