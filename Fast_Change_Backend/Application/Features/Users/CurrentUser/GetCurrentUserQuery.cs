using MediatR;

namespace Application.Features.Users.CurrentUser;

public class GetCurrentUserQuery : IRequest<GetCurrentUserResponse>;