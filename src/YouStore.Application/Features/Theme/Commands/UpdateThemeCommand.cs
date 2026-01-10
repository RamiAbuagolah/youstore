using MediatR;
using YouStore.Application.Models;

namespace YouStore.Application.Features.Theme.Commands;

public sealed record UpdateThemeCommand(Guid StoreId, Guid TenantId, string PrimaryColor, string AccentColor, string FontFamily, string Background) : IRequest<ThemeDto>;
