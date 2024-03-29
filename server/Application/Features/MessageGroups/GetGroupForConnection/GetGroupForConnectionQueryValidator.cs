﻿using FluentValidation;

namespace Application.Features.MessageGroups.GetGroupForConnection
{
    public class GetGroupForConnectionQueryValidator : AbstractValidator<GetGroupForConnectionQuery>
    {
        public GetGroupForConnectionQueryValidator()
        {
            RuleFor(x => x.ConnectionId)
                .NotEmpty().WithMessage("Connection Id required.");
        }
    }
}
