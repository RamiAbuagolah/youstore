using System;
using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Store.Commands;

public sealed record CreateStoreCommand(Guid MerchantId, Guid TenantId, string Name, string Slug, Guid TemplateId) : IRequest<StoreDto>;
