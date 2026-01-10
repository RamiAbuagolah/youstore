using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Merchant.Commands;

public sealed record RegisterMerchantCommand(string Email, string Password) : IRequest<MerchantDto>;
