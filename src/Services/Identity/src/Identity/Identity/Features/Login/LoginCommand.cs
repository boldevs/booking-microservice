using Identity.Identity.Dtos;
using MediatR;

namespace Identity.Identity.Features.Login;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;
