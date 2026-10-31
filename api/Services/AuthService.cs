using ContosoPizza.Data;
using ContosoPizza.Dtos;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

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
        Claim id = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
        Claim name = new Claim(ClaimTypes.Name, user.Username);
        Claim role = new Claim(ClaimTypes.Role, user.Role);

        List<Claim> claims = new List<Claim> { id, name, role};

        var secret = _configuration["Jwt:Key"];
        
        if (secret is null) throw new InvalidOperationException("JWT key is not configured.");


        SymmetricSecurityKey key = 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token =
        new JwtSecurityToken(
            issuer : _configuration["Jwt:Issuer"],
            audience : _configuration["Jwt:Audience"],
            claims : claims,
            notBefore : DateTime.UtcNow,
            expires : DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiresInMinutes")),
            signingCredentials : credentials);
        
        string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenToReturn;
    }

}