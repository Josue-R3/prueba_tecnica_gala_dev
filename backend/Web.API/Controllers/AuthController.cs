using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Autenticar usuario y obtener token JWT
    /// </summary>
    /// <param name="request">Credenciales de usuario</param>
    /// <returns>Respuesta de autenticación con token JWT</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Intento de login para usuario: {Usuario}", request.Usuario);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.LoginAsync(request);

            if (!response.Success)
            {
                _logger.LogWarning("Login fallido para usuario: {Usuario}. Error: {Error}",
                    request.Usuario, response.Error);
                return Unauthorized(response);
            }

            _logger.LogInformation("Login exitoso para usuario: {Usuario}", request.Usuario);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el login para usuario: {Usuario}", request.Usuario);
            return StatusCode(500, new LoginResponse
            {
                Success = false,
                Error = "Error interno del servidor"
            });
        }
    }

    /// <summary>
    /// Verificar si el token actual es válido
    /// </summary>
    [HttpPost("verify")]
    public ActionResult<object> VerifyToken()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Ok(new
            {
                success = true,
                message = "Token válido",
                user = new
                {
                    id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                    usuario = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                    rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                }
            });
        }

        return Unauthorized(new { success = false, message = "Token inválido" });
    }
}
