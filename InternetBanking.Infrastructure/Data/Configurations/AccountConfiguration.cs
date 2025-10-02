using InternetBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetBanking.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade Account no Entity Framework
/// </summary>
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.ClientName)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(a => a.Document)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(a => a.Balance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(a => a.OpeningDate)
            .IsRequired();
            
        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<int>();
            
        // Índice único para documento
        builder.HasIndex(a => a.Document)
            .IsUnique();
            
        // Índice para busca por status
        builder.HasIndex(a => a.Status);
        
        // Configuração dos relacionamentos
        // Relacionamento: Account -> SentTransactions (como conta de origem)
        builder.HasMany(a => a.SentTransactions)
            .WithOne(t => t.FromAccount)
            .HasForeignKey(t => t.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Relacionamento: Account -> ReceivedTransactions (como conta de destino)
        builder.HasMany(a => a.ReceivedTransactions)
            .WithOne(t => t.ToAccount)
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}