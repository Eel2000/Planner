using Microsoft.EntityFrameworkCore;
using Planner.core.Data;
using Planner.core.DTOs;
using Planner.core.Models;

namespace Planner.core.Services;

public class AuthenticationService
{
    private readonly ApplicationDbContext _context;

    public AuthenticationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<string>> RegisterAsync(string username, string password)
    {
        var userWithSameUsername = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (userWithSameUsername is not null)
        {
            return new Response<string>(Status.Failure, "This username is already taken");
        }

        User user = new User()
        {
            Password = password,
            UserName = username
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new Response<string>(Status.Success, "Your account has been created. Connect now to se your work space.");
    }

    public async Task<Response<User>> LogInAsync(string username, string password)
    {
        var userToConnect = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        if (userToConnect is null)
        {
            return new Response<User>(Status.Failure, "Invalid login attempt");
        }
        //TODO: after beta , generate JWT token when authenticated.

        return new Response<User>(Status.Success, $"welcome {userToConnect.Id}", userToConnect);
    }
}
