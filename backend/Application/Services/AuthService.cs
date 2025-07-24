using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Models;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IOptions<JwtSettings> jwtSettings)
    {
        _usuarioRepository = usuarioRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            // Buscar usuario por nombre de usuario
            var usuario = await _usuarioRepository.GetByUsernameAsync(request.Usuario);

            if (usuario == null || !usuario.Estado)
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Usuario no encontrado o inactivo"
                };
            }

            // Verificar contraseña (en producción debería estar hasheada)
            if (usuario.Contrasenia != request.Contrasenia)
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Credenciales inválidas"
                };
            }

            // Crear DTO del usuario autenticado
            var authUser = new AuthUserDto
            {
                Id = usuario.Id,
                Usuario = usuario.UsuarioName,
                RolId = usuario.RolId,
                EmpleadoId = usuario.EmpleadoId,
                Rol = new RolDto
                {
                    Id = usuario.Rol.Id,
                    Nombre = usuario.Rol.Nombre
                },
                Empleado = usuario.Empleado != null
                    ? new EmpleadoDto
                    {
                        Id = usuario.Empleado.Id,
                        Nombre = usuario.Empleado.Nombre,
                        Apellido = usuario.Empleado.Apellido,
                        Cargo = usuario.Empleado.Cargo
                    }
                    : null
            };

            // Generar token JWT
            var token = GenerateJwtToken(authUser);

            return new LoginResponse
            {
                Success = true,
                Token = token,
                User = authUser
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse
            {
                Success = false,
                Error = $"Error en autenticación: {ex.Message}"
            };
        }
    }

    public string GenerateJwtToken(AuthUserDto user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Usuario),
            new Claim(ClaimTypes.Role, user.Rol.Nombre),
            new Claim("RolId", user.RolId.ToString()),
            new Claim("EmpleadoId", user.EmpleadoId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
