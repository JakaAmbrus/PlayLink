using Application.Exceptions;
using Application.Features.Authentication.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Globalization;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {

        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;
        private readonly ITokenService _tokenService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public UserRegistrationCommandHandler(IApplicationDbContext context , IUserManager userManager, ITokenService tokenService, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.BeginTransactionAsync(cancellationToken))
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

                    result = await _userManager.AddToRoleAsync(user, "Member");

                    if (!result.Succeeded)
                    {
                        throw new ServerErrorException("Error adding member role to user");
                    }

                    await transaction.CommitAsync(cancellationToken);

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
                catch (ConflictException)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                catch (ServerErrorException)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new ServerErrorException("Unexpected error while creating user");
                }
            }
        }
    }
    
}
