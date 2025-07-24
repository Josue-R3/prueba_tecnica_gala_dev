namespace Application.DTOs.Auth;

public record LoginResponse
{
    public bool Success { get; init; }
    public string? Token { get; init; }
    public AuthUserDto? User { get; init; }
    public string? Error { get; init; }
}
