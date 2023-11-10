using Domain.Entities;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Features.Authentication.Common;

namespace Application.Features.Authentication.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginResponse>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<UserLoginCommand> _validator;

        public UserLoginCommandHandler(UserManager<AppUser> userManager,
            ITokenService tokenService,
            IValidator<UserLoginCommand> validator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }

        public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                return new UserLoginResponse
                {
                    User = null,
                    Errors = errors,
                    Success = false
                };
            }

            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == request.Username.ToLower().Trim());

            if (user == null)
            {
                return new UserLoginResponse
                {
                    Success = false,
                    Errors = new List<string> { "Invalid username" }
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return new UserLoginResponse
                {
                    Success = false,
                    Errors = new List<string> { "Invalid password" }
                };
            }

            string userName = user.UserName;

            return new UserLoginResponse
            {
                User = new UserDto
                {
                    Username = userName,
                    Token = await _tokenService.CreateToken(user)
                },
                Success = true
            };
        }
    }
}
