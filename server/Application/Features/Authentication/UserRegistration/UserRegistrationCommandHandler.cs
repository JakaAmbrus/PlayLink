using Application.Features.Authentication.Common;
using Domain.Entities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public UserRegistrationCommandHandler(UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {

            if (await UserExists(request.Username))
            {
                throw new InvalidOperationException("Username already existis");
            }
            var user = new AppUser
            {
                UserName = request.Username.ToLower().Trim(),
                FullName = FromatPropertiesToTitleCase(request.FullName).Trim(),
                City = FromatPropertiesToTitleCase(request.City).Trim(),
                Country = FromatPropertiesToTitleCase(request.Country).Trim(),
                DateOfBirth = request.DateOfBirth,
                Created = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", \n", result.Errors.Select(e => e.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", \n", roleResult.Errors.Select(e => e.Description)));
            }

            return new UserRegistrationResponse
            {
                User = new UserDto
                {
                    Username = user.UserName,
                    Token = await _tokenService.CreateToken(user)
                }
            };

        }
        private string FromatPropertiesToTitleCase(string input)
        {
            var inputInfo = CultureInfo.CurrentCulture.TextInfo;
            return inputInfo.ToTitleCase(input.ToLower());
        }

        private async Task<bool> UserExists(string username)
        {

            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
