﻿namespace Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(string? userId, string? userEmail, string? role);
    }
}
