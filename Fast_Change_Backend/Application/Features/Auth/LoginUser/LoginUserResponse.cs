namespace Application.Features.Auth.LoginUser;

public sealed record LoginUserResponse(string AccessToken, string RefreshToken);
