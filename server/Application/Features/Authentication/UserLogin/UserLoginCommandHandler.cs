using Application.Exceptions;
using Application.Features.Authentication.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authentication.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginResponse>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public UserLoginCommandHandler(UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
 
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == request.Username.ToLower().Trim(), cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid Username or Password");         
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                throw new UnauthorizedException("Invalid Username or Password");
            }

            string userName = user.UserName;

            return new UserLoginResponse
            {
                User = new UserDto
                {
                    Username = userName,
                    Token = await _tokenService.CreateToken(user)
                }
            };
        }
    }
}
