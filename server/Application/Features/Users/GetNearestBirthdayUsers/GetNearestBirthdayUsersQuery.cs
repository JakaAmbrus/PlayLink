﻿using MediatR;

namespace Application.Features.Users.GetNearestBirthdayUsers
{
    public class GetNearestBirthdayUsersQuery : IRequest<GetNearestBirthdayUsersResult>
    {
        public int AuthUserId { get; set; }
    }
}
