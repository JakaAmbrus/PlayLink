﻿using MediatR;

namespace Application.Features.Users.GetUserByUsername
{
    public class GetUserByUsernameQuery : IRequest<GetUserByUsernameResponse>
    {
        public string Username { get; set; }
        public GetUserByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}