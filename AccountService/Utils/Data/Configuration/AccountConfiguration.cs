using AccountService.Features.Accounts;
using AccountService.Utils.Data.Generator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Utils.Data.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(DataConstants.Account.TableName);
        builder.Property(account => account.Id)
            .IsRequired()
            .HasColumnName(DataConstants.Account.IdColumn);
        builder.HasKey(account => account.Id);
        builder.Property(account => account.Balance)
            .HasColumnName(DataConstants.Account.BalanceColumn)
            .IsRequired();
        builder.Property(account => account.Type)
            .HasColumnName(DataConstants.Account.TypeColumn)
            .IsRequired();
        builder.Property(account => account.CreatedAt)
            .HasColumnName(DataConstants.Account.CreatedAtColumn)
            .IsRequired()
            .HasValueGenerator<CurrentDateGenerator>();
        builder.Property(account => account.ClosedAt)
            .HasColumnName(DataConstants.Account.ClosedAtColumn)
            .IsRequired(false);
        builder.Property(account => account.Currency)
            .HasColumnName(DataConstants.Account.CurrencyColumn)
            .IsRequired()
            .HasMaxLength(3);
        builder.Property(account => account.OwnerId)
            .HasColumnName(DataConstants.Account.OwnerIdColumn)
            .IsRequired();
        builder.Property(account => account.Percent)
            .HasColumnName(DataConstants.Account.PercentColumn)
            .IsRequired(false);
        builder.Property(account => account.Version)
            .HasColumnName(DataConstants.Account.VersionColumn)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .IsRowVersion();
        builder.HasMany(account => account.AccountTransactions)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId);

        builder.HasMany(account => account.CounterPartyTransactions)
            .WithOne(x => x.CounterPartyAccount)
            .HasForeignKey(x => x.CounterPartyAccountId);
    }
}