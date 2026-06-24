using MediatR;

namespace Application.Features.Users.CurrentUser;

public sealed record GetCurrentUserQuery : IRequest<GetCurrentUserResponse>;