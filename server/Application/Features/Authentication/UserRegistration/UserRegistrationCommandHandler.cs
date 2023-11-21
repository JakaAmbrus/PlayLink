using Application.Exceptions;
using Application.Features.Authentication.Common;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {

        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public UserRegistrationCommandHandler(DataContext context ,UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
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

                    await transaction.CommitAsync(cancellationToken);

                    return new UserRegistrationResponse
                    {
                        User = new UserDto
                        {
                            Username = user.UserName,
                            Token = await _tokenService.CreateToken(user)
                        }
                    };
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
        private string FormatPropertiesToTitleCase(string input)
        {
            var inputInfo = CultureInfo.CurrentCulture.TextInfo;
            return inputInfo.ToTitleCase(input.ToLower());
        }
    }
}
