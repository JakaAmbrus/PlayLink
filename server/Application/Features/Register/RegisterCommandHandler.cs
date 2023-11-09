using MediatR;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Application.Features.Common;
using System.Globalization;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Application.Features.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterCommand> _validator;

        public RegisterCommandHandler(UserManager<AppUser> userManager,
            ITokenService tokenService,
            IValidator<RegisterCommand> validator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return new RegisterResponse
                {
                    User = null,
                    Errors = errors,
                    Success = false
                };
            }

            if (await UserExists(request.Username))
            {
                return new RegisterResponse
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
                var errors = new List<string>();
                result.Errors.ToList().ForEach(x => errors.Add(x.ToString()));
                return new RegisterResponse
                {
                    User = null,
                    Errors = errors,
                    Success = false
                };
            }

            return new RegisterResponse
            {
                User = new UserDto
                {
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
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
