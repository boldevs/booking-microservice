using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Identity.Identity.Dtos;
using Identity.Identity.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Identity.Identity.Features.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var httpClient = _httpClientFactory.CreateClient();

        var authority = _configuration["Jwt:Authority"];
        var tokenEndpoint = $"{authority.TrimEnd('/')}/connect/token";

        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = "client",
                ["client_secret"] = "secret",
                ["username"] = request.Username,
                ["password"] = request.Password,
                ["scope"] = "openid profile flight-api passenger-api booking-api"
            })
        };

        var response = await httpClient.SendAsync(tokenRequest, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new LoginUserException($"Failed to login. Status code: {response.StatusCode}. Content: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

        if (tokenResponse.TryGetProperty("error", out var error))
        {
             throw new LoginUserException($"Failed to login: {error.GetString()}");
        }

        return new LoginResponseDto(
            tokenResponse.GetProperty("access_token").GetString(),
            tokenResponse.GetProperty("expires_in").GetInt32(),
            tokenResponse.GetProperty("token_type").GetString(),
            tokenResponse.GetProperty("scope").GetString()
        );
    }
}
