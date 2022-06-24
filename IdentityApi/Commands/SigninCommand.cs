namespace IdentityApi.Commands
{
    public class SigninCommand : IRequest<IdentityResult>
    {
        public SigninDTO SigninDTO { get; set; }

        public class SigninCommandHandler : IRequestHandler<SigninCommand, IdentityResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signManager;
            private readonly IConfiguration _configuration;
            private readonly IIdentityServerInteractionService _interaction;

            public SigninCommandHandler(
                UserManager<ApplicationUser> userManager, 
                SignInManager<ApplicationUser> signInManager, 
                IConfiguration configuration,
                IIdentityServerInteractionService interaction)
            { 
                _userManager = userManager; 
                _signManager = signInManager;
                _configuration = configuration;
                _interaction = interaction;
            }

            public async Task<IdentityResult> Handle(SigninCommand request, CancellationToken cancellationToken)
            {
                var buyer = await _userManager.FindByEmailAsync(request.SigninDTO.Email);
                if (buyer == null) return IdentityResult.Failed(errors: new IdentityError { Description = "Invalid username or password" });

                if (await _userManager.CheckPasswordAsync(buyer, request.SigninDTO.Password))
                {
                    var tokenLifeTime = _configuration.GetValue("TokenLifetimeMinutes", 120);

                    var props = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifeTime),
                        AllowRefresh = true,
                    };

                    await _signManager.SignInAsync(buyer, props);

                    return IdentityResult.Success;
                }

                return IdentityResult.Failed(errors: new IdentityError { Description = "Invalid username or password" });
            }
        }
    }
}