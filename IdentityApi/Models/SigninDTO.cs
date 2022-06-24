namespace IdentityApi.Models
{
    public class SigninDTO
    {
        [EmailAddress]
        public string Email { get; init; }

        [DataType(DataType.Password)]
        public string Password { get; init; }
    }
}
