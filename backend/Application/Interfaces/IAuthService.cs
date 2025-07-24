using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    string GenerateJwtToken(AuthUserDto user);
}
