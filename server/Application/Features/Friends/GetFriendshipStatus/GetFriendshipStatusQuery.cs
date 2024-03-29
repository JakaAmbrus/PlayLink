﻿using MediatR;

namespace Application.Features.Friends.GetRelationshipStatus
{
    public class GetFriendshipStatusQuery : IRequest<GetFriendshipStatusResponse>
    {
        public string ProfileUsername { get; set; }
        public int AuthUserId { get; set; }
    }
}
