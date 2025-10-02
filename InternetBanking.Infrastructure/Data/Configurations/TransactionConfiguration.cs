using InternetBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetBanking.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade Transaction no Entity Framework
/// </summary>
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.FromAccountId)
            .IsRequired();
            
        builder.Property(t => t.ToAccountId)
            .IsRequired();
            
        builder.Property(t => t.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(t => t.CreatedAt)
            .IsRequired();
            
        builder.Property(t => t.Description)
            .HasMaxLength(500);
        
        // Índices para performance
        builder.HasIndex(t => t.FromAccountId);
        builder.HasIndex(t => t.ToAccountId);
        builder.HasIndex(t => t.CreatedAt);
        builder.HasIndex(t => t.Type);        
    }
}