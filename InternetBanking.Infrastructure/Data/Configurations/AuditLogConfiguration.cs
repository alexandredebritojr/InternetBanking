using InternetBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetBanking.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade AuditLog no Entity Framework
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(a => a.EntityType)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(a => a.EntityId)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(a => a.UserResponsible)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(a => a.Timestamp)
            .IsRequired();
            
        builder.Property(a => a.Details)
            .HasMaxLength(1000);
        
        // Índices para consultas frequentes
        builder.HasIndex(a => a.EntityType);
        builder.HasIndex(a => a.EntityId);
        builder.HasIndex(a => a.Timestamp);
        builder.HasIndex(a => a.UserResponsible);
    }
}