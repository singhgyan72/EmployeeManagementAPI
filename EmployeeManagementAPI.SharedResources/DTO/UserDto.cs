using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.SharedResources.DTO
{
    public record UserForRegistrationDto
    {
        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }

        public string? Email { get; init; }

        public string? PhoneNumber { get; init; }

        public ICollection<string>? Roles { get; init; }
    }

    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }
    }

    public record TokenDto(string AccessToken, string RefreshToken);
}
