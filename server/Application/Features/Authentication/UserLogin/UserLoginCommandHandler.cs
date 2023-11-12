using Application.Exceptions;
using Application.Features.Authentication.Common;
using Domain.Entities;
using Infrastructure.Interfaces;
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
 
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == request.Username.ToLower().Trim());

            if (user == null)
            {
                throw new UnauthorizedException("Invalid username or password");         
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                throw new UnauthorizedException("Invalid username or password");
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
