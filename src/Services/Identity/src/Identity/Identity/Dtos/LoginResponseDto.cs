namespace Identity.Identity.Dtos;

public record LoginResponseDto(string AccessToken, int ExpiresIn, string TokenType, string Scope);
