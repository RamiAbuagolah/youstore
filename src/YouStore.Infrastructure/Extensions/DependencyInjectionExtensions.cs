using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YouStore.Application.Interfaces;
using YouStore.Application.Interfaces.Repositories;
using YouStore.Domain.Events;
using YouStore.Infrastructure.Consumers;
using YouStore.Infrastructure.EventBus;
using YouStore.Infrastructure.Outbox;
using YouStore.Infrastructure.Repositories;
using YouStore.Infrastructure.Repositories.ReadModel;
using YouStore.Infrastructure.Services;
using YouStore.Infrastructure.Persistence;

namespace YouStore.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            "Host=localhost;Port=5432;Database=devdb;Username=devuser;Password=dev123;Include Error Detail=true";

        services.AddDbContext<YouStoreDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(YouStoreDbContext).Assembly.GetName().Name)));

        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<IThemeRepository, ThemeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<YouStoreDbContext>());
        services.AddSingleton<IBlobStorage, AzureBlobStorage>();
        services.AddScoped<IStoreReadModelRepository, StoreReadModelRepository>();
        services.AddScoped<IProductReadModelRepository, ProductReadModelRepository>();
        services.AddScoped<IOutboxService, OutboxService>();
        services.AddScoped<IIntegrationEventHandler<StoreCreatedEvent>, StoreReadModelEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ProductCreatedEvent>, ProductReadModelEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ProductUpdatedEvent>, ProductReadModelEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ProductPublishedEvent>, ProductReadModelEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ProductDiscountChangedEvent>, ProductReadModelEventHandler>();

        services.AddSingleton<IEventBus>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            var useAzure = bool.TryParse(config["UseAzureServiceBus"], out var use) && use;
            if (use)
            {
                return ActivatorUtilities.CreateInstance<AzureServiceBusEventBus>(provider);
            }

            return ActivatorUtilities.CreateInstance<InMemoryEventBus>(provider);
        });

        services.AddHostedService<OutboxDispatcher>();

        return services;
    }
}
