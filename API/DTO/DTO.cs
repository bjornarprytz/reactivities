using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public record LoginDto(string Email, string Password);
public record RegisterDto(
    [Required]
    string DisplayName, 
    [Required][EmailAddress]
    string Email,
    [Required][RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage="Password must be complex")] // Number, lower case, upper case, 4-8 characters
    string Password, 
    [Required]
    string Username
    );
public record UserDto(string DisplayName, string Token, string Username, string? Image);
