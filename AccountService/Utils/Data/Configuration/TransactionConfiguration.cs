using AccountService.Features.Transactions;
using AccountService.Utils.Data.Generator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Utils.Data.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(DataConstants.Transaction.TableName);
        builder.Property(transaction => transaction.Id)
            .IsRequired()
            .HasColumnName(DataConstants.Transaction.IdColumn);
        builder.HasKey(transaction => transaction.Id);
        builder.Property(transaction => transaction.AccountId)
            .IsRequired()
            .HasColumnName(DataConstants.Transaction.AccountIdColumn);
        builder.Property(transaction => transaction.CounterPartyAccountId)
            .IsRequired(false)
            .HasColumnName(DataConstants.Transaction.CounterPartyIdColumn);
        builder.Property(transaction => transaction.CreatedAt)
            .IsRequired()
            .HasColumnName(DataConstants.Transaction.CreatedAtColumn)
            .HasValueGenerator<CurrentDateGenerator>();
        builder.Property(transaction => transaction.Currency)
            .HasMaxLength(3)
            .HasColumnName(DataConstants.Transaction.CurrencyColumn)
            .IsRequired();
        builder.Property(transaction => transaction.Description)
            .HasMaxLength(255)
            .HasColumnName(DataConstants.Transaction.DescriptionColumn)
            .IsRequired();
        builder.Property(transaction => transaction.Sum)
            .HasColumnName(DataConstants.Transaction.AmountColumn)
            .IsRequired();
        builder.Property(transaction => transaction.Type)
            .HasColumnName(DataConstants.Transaction.TypeColumn)
            .IsRequired();
    }
}