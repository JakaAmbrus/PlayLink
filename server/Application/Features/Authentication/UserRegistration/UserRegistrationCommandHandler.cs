using Application.Exceptions;
using Application.Features.Authentication.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public UserRegistrationCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService, ICacheInvalidationService cacheInvalidationService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var user = new AppUser
                {
                    UserName = request.Username,
                    Gender = request.Gender,
                    FullName = FormatPropertiesToTitleCase(request.FullName),
                    Country = FormatPropertiesToTitleCase(request.Country),
                    DateOfBirth = request.DateOfBirth,
                    Created = DateTime.UtcNow,
                };

                static string FormatPropertiesToTitleCase(string input)
                {
                    var inputInfo = CultureInfo.CurrentCulture.TextInfo;
                    return inputInfo.ToTitleCase(input.ToLower());
                }

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                    {
                        throw new ConflictException("Username already exists");
                    }

                    throw new ServerErrorException(string.Join(", \n", result.Errors.Select(e => e.Description)));
                }

                await _userManager.AddToRoleAsync(user, "Member");

                _cacheInvalidationService.InvalidateSearchUserCache();
                _cacheInvalidationService.InvalidateNearestBirthdayUsersCache();

                return new UserRegistrationResponse
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
            catch
            {

                throw new ServerErrorException("Problem creating new user");
            }
        }
    }
    
}
