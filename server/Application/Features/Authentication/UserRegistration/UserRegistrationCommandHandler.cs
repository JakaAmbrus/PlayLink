using MediatR;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using System.Globalization;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Application.Features.Authentication.Common;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<UserRegistrationCommand> _validator;

        public UserRegistrationCommandHandler(UserManager<AppUser> userManager,
            ITokenService tokenService,
            IValidator<UserRegistrationCommand> validator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                return new UserRegistrationResponse
                {
                    User = null,
                    Errors = errors,
                    Success = false
                };
            }

            if (await UserExists(request.Username))
            {
                return new UserRegistrationResponse
                {
                    User = null,
                    Errors = new List<string> { "Username already in use" },
                    Success = false
                };
            }
            var user = new AppUser
            {
                UserName = request.Username.ToLower().Trim(),
                FullName = FromatPropertiesToTitleCase(request.FullName),
                City = FromatPropertiesToTitleCase(request.City),
                Country = FromatPropertiesToTitleCase(request.Country),
                DateOfBirth = request.DateOfBirth,
                Created = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new UserRegistrationResponse
                {
                    User = null,
                    Errors = errors,
                    Success = false
                };
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description).ToList();
                return new UserRegistrationResponse
                {
                    User = null,
                    Errors = errors,
                    Success = false
                };
            }

            return new UserRegistrationResponse
            {
                User = new UserDto
                {
                    Username = user.UserName,
                    Token = await _tokenService.CreateToken(user)
                },
                Success = true
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
