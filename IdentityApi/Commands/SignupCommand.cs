using Microsoft.Extensions.Logging;

namespace IdentityApi.Commands
{
    public class SignupCommand : IRequest<IdentityResult>
    {
        public SignupDTO SignupDTO { get; set; }

        public class SignupCommandHandler : IRequestHandler<SignupCommand, IdentityResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ILogger<SignupCommandHandler> _logger;

            public SignupCommandHandler(UserManager<ApplicationUser> userManager, ILogger<SignupCommandHandler> logger)
            { 
                _userManager = userManager;
                _logger = logger;
            }

            public async Task<IdentityResult> Handle(SignupCommand request, CancellationToken cancellationToken)
            {
                var buyer = new ApplicationUser
                {
                    UserName = request.SignupDTO.Email,
                    Email = request.SignupDTO.Email,
                    Name = request.SignupDTO.User.Name,
                    LastName = request.SignupDTO.User.LastName,
                };

                return await _userManager.CreateAsync(buyer, request.SignupDTO.Password);
            }
        }
    }
}
