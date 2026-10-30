using ContosoPizza.Data;
using ContosoPizza.Dtos;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ContosoPizza.Services;


public class AuthService : IAuthService
{
    private readonly PizzaContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(PizzaContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<bool> RegisterUserAsync(RegisterRequestDto dto)
    {
        bool foundUser = await _context.Users.AnyAsync(eachUser => eachUser.Username == dto.Username);

        if(foundUser) return false;

        User registerUser = new User{Username=dto.Username, Role = "User"};

        registerUser.PasswordHash = _passwordHasher.HashPassword(registerUser, dto.Password);
        

        _context.Users.Add(registerUser);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<string?> LoginUserAsync(LoginRequestDto dto)
    {
        var user = await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(eachUser => eachUser.Username == dto.Username);

        if (user is null) return null;

        var exactPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if(exactPassword == PasswordVerificationResult.Failed) return null;

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        return "test";
    }

}