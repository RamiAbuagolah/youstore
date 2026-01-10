using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouStore.Application.Interfaces;
using YouStore.Domain.Entities;
using YouStore.Domain.ValueObjects;
using YouStore.Infrastructure.Outbox;
using YouStore.Infrastructure.Seed;
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

        ConfigureMerchant(modelBuilder.Entity<MerchantUser>());
        ConfigureStore(modelBuilder.Entity<Store>());
        ConfigureTemplate(modelBuilder.Entity<Template>());
        ConfigureThemeConfig(modelBuilder.Entity<ThemeConfig>());
        ConfigureCategory(modelBuilder.Entity<Category>());
        ConfigureProduct(modelBuilder.Entity<Product>());
        ConfigureProductImage(modelBuilder.Entity<ProductImage>());
        ConfigureStoreReadModel(modelBuilder.Entity<StoreReadModel>());
        ConfigureProductReadModel(modelBuilder.Entity<ProductReadModel>());
        ConfigureOutbox(modelBuilder.Entity<OutboxMessage>());
    }

    private static void ConfigureMerchant(EntityTypeBuilder<MerchantUser> builder)
    {
        builder.ToTable("merchant_users");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(id => id.Value, value => MerchantId.From(value))
            .ValueGeneratedNever();
        builder.Property(m => m.TenantId)
            .HasConversion(t => t.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(m => m.Email)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(m => m.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(m => m.CreatedAt)
            .IsRequired();
        builder.HasIndex(m => m.Email).IsUnique();
    }

    private static void ConfigureStore(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("stores");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .ValueGeneratedNever();
        builder.Property(s => s.TenantId)
            .HasConversion(t => t.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(s => s.MerchantId)
            .HasConversion(m => m.Value, value => MerchantId.From(value))
            .IsRequired();
        builder.Property(s => s.TemplateId)
            .HasConversion(t => t.Value, value => TemplateId.From(value))
            .IsRequired();
        builder.Property(s => s.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(s => s.Slug)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(s => s.CreatedAt)
            .IsRequired();
        builder.Property(s => s.IsPublished)
            .HasDefaultValue(false);
        builder.HasIndex(s => new { s.TenantId, s.Slug }).IsUnique();
    }

    private static void ConfigureTemplate(EntityTypeBuilder<Template> builder)
    {
        builder.ToTable("templates");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(id => id.Value, value => TemplateId.From(value))
            .ValueGeneratedNever();
        builder.Property(t => t.Name)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(t => t.Description)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(t => t.PreviewImageUrl)
            .HasMaxLength(1024)
            .IsRequired();

        builder.HasData(TemplateSeed.AllTemplates.ToArray());
    }

    private static void ConfigureThemeConfig(EntityTypeBuilder<ThemeConfig> builder)
    {
        builder.ToTable("theme_configs");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(t => t.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(t => t.PrimaryColor).HasMaxLength(64).IsRequired();
        builder.Property(t => t.AccentColor).HasMaxLength(64).IsRequired();
        builder.Property(t => t.FontFamily).HasMaxLength(128).IsRequired();
        builder.Property(t => t.Background).HasMaxLength(256).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();
        builder.HasIndex(t => t.StoreId).IsUnique();
    }

    private static void ConfigureCategory(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => CategoryId.From(value))
            .ValueGeneratedNever();
        builder.Property(c => c.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(c => c.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(c => c.Name)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(c => c.Slug)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(c => c.Description)
            .HasMaxLength(512);
        builder.Property(c => c.CreatedAt).IsRequired();
    }

    private static void ConfigureProduct(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => ProductId.From(value))
            .ValueGeneratedNever();
        builder.Property(p => p.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(p => p.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(p => p.CategoryId)
            .HasConversion(id => id.Value, value => CategoryId.From(value))
            .IsRequired();
        builder.Property(p => p.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(p => p.Description)
            .HasMaxLength(1024)
            .IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);
        builder.Property(p => p.Currency)
            .HasMaxLength(8)
            .IsRequired();
        builder.Property(p => p.DiscountPrice).HasPrecision(18, 2);
        builder.Property(p => p.DiscountDescription)
            .HasMaxLength(512);
        builder.Property(p => p.IsPublished).HasDefaultValue(false);
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();
    }

    private static void ConfigureProductImage(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images");
        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.Id)
            .HasConversion(id => id.Value, value => ProductImageId.From(value))
            .ValueGeneratedNever();
        builder.Property(pi => pi.TenantId)
            .HasConversion(id => id.Value, value => TenantId.From(value))
            .IsRequired();
        builder.Property(pi => pi.StoreId)
            .HasConversion(id => id.Value, value => StoreId.From(value))
            .IsRequired();
        builder.Property(pi => pi.ProductId)
            .HasConversion(id => id.Value, value => ProductId.From(value))
            .IsRequired();
        builder.Property(pi => pi.Url)
            .HasMaxLength(2048)
            .IsRequired();
        builder.Property(pi => pi.IsPrimary).IsRequired();
        builder.Property(pi => pi.UploadedAt).IsRequired();
    }

    private static void ConfigureStoreReadModel(EntityTypeBuilder<StoreReadModel> builder)
    {
        builder.ToTable("store_read_models");
        builder.HasKey(s => s.StoreId);
        builder.Property(s => s.Name).HasMaxLength(256).IsRequired();
        builder.Property(s => s.Slug).HasMaxLength(128).IsRequired();
        builder.Property(s => s.TemplateId).IsRequired();
        builder.Property(s => s.MerchantId).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.IsPublished).IsRequired();
    }

    private static void ConfigureProductReadModel(EntityTypeBuilder<ProductReadModel> builder)
    {
        builder.ToTable("product_read_models");
        builder.HasKey(p => p.ProductId);
        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1024).IsRequired();
        builder.Property(p => p.Currency).HasMaxLength(8).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();
        builder.Property(p => p.IsPublished).IsRequired();
    }

    private static void ConfigureOutbox(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.EventType)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(o => o.Payload).IsRequired();
        builder.Property(o => o.OccurredOn).IsRequired();
        builder.Property(o => o.Processed).HasDefaultValue(false);
        builder.Property(o => o.ProcessedAt);
        builder.HasIndex(o => o.Processed);
    }
}
