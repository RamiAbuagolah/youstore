using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Merchant.Commands;

public sealed record LoginMerchantCommand(string Email, string Password) : IRequest<MerchantLoginResult>;
