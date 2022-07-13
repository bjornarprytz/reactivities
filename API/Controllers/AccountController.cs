using System.Security.Claims;
using API.DTO;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly TokenMintingService _tokenMintingService;

    public AccountController(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        TokenMintingService tokenMintingService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenMintingService = tokenMintingService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user is null) return Unauthorized();

        var tryLogin = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, 
            lockoutOnFailure: false);

        return tryLogin.Succeeded
            ? CreateUserObject(user)
            : Unauthorized();
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }
        
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
        {
            ModelState.AddModelError("username", "Username taken");
            return ValidationProblem();
        }

        var user = new AppUser{
            DisplayName = registerDto.DisplayName, 
            Email = registerDto.Email, 
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        return result.Succeeded
            ? CreateUserObject(user)
            : BadRequest("Problem registering user");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        if (User.FindFirstValue(ClaimTypes.Email) 
            is not { } email
            ||
            await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Email == email)
            is not {} user
            )
        {
            return NotFound();
        }

        return CreateUserObject(user);
    }


    private UserDto CreateUserObject(AppUser user)
    {
        return new UserDto(
            user.DisplayName, 
            _tokenMintingService.MintToken(user), 
            user.UserName, 
            user.Photos?.FirstOrDefault(x => x.IsMain)?.Url);
    }
}