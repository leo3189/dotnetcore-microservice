namespace IdentityApi.Models
{
    public class SignupDTO
    {
        [EmailAddress]
        public string Email { get; init; }

        [DataType(DataType.Password)]
        public string Password { get; init; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; init; }

        public ApplicationUser User { get; init; }
    }
}
