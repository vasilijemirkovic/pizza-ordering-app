using ContosoPizza.Data;
using ContosoPizza.Dtos;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace ContosoPizza.Services;


public class AuthService : IAuthService
{
    private readonly PizzaContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    public AuthService(PizzaContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
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
}