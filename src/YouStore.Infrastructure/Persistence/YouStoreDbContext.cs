using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouStore.Application.Interfaces;
using YouStore.Domain.Entities;
using YouStore.Infrastructure.Outbox;
using YouStore.ReadModels;

namespace YouStore.Infrastructure.Persistence;

public sealed class YouStoreDbContext : DbContext, IUnitOfWork
{
    public DbSet<MerchantUser> MerchantUsers => Set<MerchantUser>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<ThemeConfig> ThemeConfigs => Set<ThemeConfig>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<StoreReadModel> StoreReadModels => Set<StoreReadModel>();
    public DbSet<ProductReadModel> ProductReadModels => Set<ProductReadModel>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public YouStoreDbContext(DbContextOptions<YouStoreDbContext> options)
        : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YouStoreDbContext).Assembly);
    }
}
