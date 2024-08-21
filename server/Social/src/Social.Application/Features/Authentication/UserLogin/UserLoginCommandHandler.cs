using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Features.Authentication.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Authentication.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginResponse>
    {

        private readonly IUserManager _userManager;
        private readonly ITokenService _tokenService;

        public UserLoginCommandHandler(IUserManager userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
 
            var user = await _userManager.FindByUsernameAsync(request.Username.ToLower().Trim()) 
                ?? throw new UnauthorizedException("Invalid Username or Password");

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                throw new UnauthorizedException("Invalid Username or Password");
            }

            return new UserLoginResponse
            {
                User = new UserDto
                {
                    Username = user.UserName,
                    Token = await _tokenService.CreateToken(user),
                    FullName = user.FullName,
                    Gender = user.Gender,
                    ProfilePictureUrl = user.ProfilePictureUrl
                }
            };
        }
    }
}
