using Mrp.Models;
using Mrp.Services;
using System.Text.Json;

namespace Mrp.Controllers
{
    public class AuthController
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        public bool Register(AuthRequest request)
        {
            return authService.Register(request.Username, request.Password);
        }

        public string? Login(AuthRequest request)
        {
            string? token = authService.Login(request.Username, request.Password);
            if (token == null) return null;
            return JsonSerializer.Serialize(new { token });
        }

        public bool IsAuthorized(string? authHeader)
        {
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                return false;
            string token = authHeader.Substring("Bearer ".Length).Trim();
            return authService.ValidateToken(token);
        }
    }
}