using Mrp.Models;
using Mrp.Services;
using System;
using System.Collections.Generic;
using System.Text;
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

    }
}
