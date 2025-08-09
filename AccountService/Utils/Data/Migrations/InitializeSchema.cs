using FluentMigrator;

namespace AccountService.Utils.Data.Migrations;

[Migration(1, "initialize")]
public class InitializeSchema : Migration
{
    public override void Up()
    {
        Create.Table(DataConstants.Account.TableName)
            .WithColumn(DataConstants.Account.IdColumn).AsGuid().PrimaryKey()
            .WithColumn(DataConstants.Account.OwnerIdColumn).AsGuid().NotNullable()
            .WithColumn(DataConstants.Account.BalanceColumn).AsDecimal().NotNullable()
            .WithColumn(DataConstants.Account.CurrencyColumn).AsString(3).NotNullable()
            .WithColumn(DataConstants.Account.CreatedAtColumn).AsDateTimeOffset().NotNullable()
            .WithColumn(DataConstants.Account.ClosedAtColumn).AsDateTimeOffset().Nullable()
            .WithColumn(DataConstants.Account.TypeColumn).AsInt16().NotNullable()
            .WithColumn(DataConstants.Account.VersionColumn).AsByte().NotNullable()
            .WithColumn(DataConstants.Account.PercentColumn).AsDouble().Nullable();

        Create.Table(DataConstants.Transaction.TableName)
            .WithColumn(DataConstants.Transaction.IdColumn).AsGuid().PrimaryKey()
            .WithColumn(DataConstants.Transaction.AccountIdColumn).AsGuid().NotNullable()
            .WithColumn(DataConstants.Transaction.CounterPartyIdColumn).AsGuid().Nullable()
            .WithColumn(DataConstants.Transaction.AmountColumn).AsDecimal().NotNullable()
            .WithColumn(DataConstants.Transaction.DescriptionColumn).AsString(255).NotNullable()
            .WithColumn(DataConstants.Transaction.TypeColumn).AsInt16().NotNullable()
            .WithColumn(DataConstants.Transaction.CreatedAtColumn).AsDateTimeOffset().NotNullable()
            .WithColumn(DataConstants.Transaction.CurrencyColumn).AsString(3).NotNullable();
    }

    public override void Down()
    {
        Delete.FromTable(DataConstants.Account.TableName);
        Delete.FromTable(DataConstants.Transaction.TableName);
    }
}