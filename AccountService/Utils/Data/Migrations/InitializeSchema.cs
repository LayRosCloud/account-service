using System.Data;
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
            .WithColumn(DataConstants.Account.VersionColumn).AsInt64().NotNullable().WithDefaultValue(1)
            .WithColumn(DataConstants.Account.PercentColumn).AsDecimal().Nullable();

        // ReSharper disable once UseRawString
        Execute.Sql(@"
            CREATE OR REPLACE FUNCTION update_version()
            RETURNS TRIGGER AS $$
            BEGIN
                NEW.""" + DataConstants.Account.VersionColumn + @""" = OLD.""" + DataConstants.Account.VersionColumn + @""" + 1;
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;
        ");
        // ReSharper disable once UseRawString
        Execute.Sql(@"
            CREATE TRIGGER accounts_version_trigger
            BEFORE UPDATE ON """ + DataConstants.Account.TableName + @"""
            FOR EACH ROW
            EXECUTE FUNCTION update_version();
        ");
        Create.Table(DataConstants.Transaction.TableName)
            .WithColumn(DataConstants.Transaction.IdColumn)
            .AsGuid()
            .PrimaryKey()
            .WithColumn(DataConstants.Transaction.AccountIdColumn)
            .AsGuid()
            .NotNullable()
            .WithColumn(DataConstants.Transaction.CounterPartyIdColumn)
            .AsGuid()
            .Nullable()
            .WithColumn(DataConstants.Transaction.AmountColumn)
            .AsDecimal()
            .NotNullable()
            .WithColumn(DataConstants.Transaction.DescriptionColumn)
            .AsString(255)
            .NotNullable()
            .WithColumn(DataConstants.Transaction.TypeColumn)
            .AsInt16()
            .NotNullable()
            .WithColumn(DataConstants.Transaction.CreatedAtColumn)
            .AsDateTimeOffset()
            .NotNullable()
            .WithColumn(DataConstants.Transaction.CurrencyColumn)
            .AsString(3)
            .NotNullable();

        Create.ForeignKey($"fk_{DataConstants.Transaction.TableName}_{DataConstants.Account.TableName}_accountId")
            .FromTable(DataConstants.Transaction.TableName).ForeignColumn(DataConstants.Transaction.AccountIdColumn)
            .ToTable(DataConstants.Account.TableName).PrimaryColumn(DataConstants.Account.IdColumn)
            .OnDelete(Rule.None);
        Create.ForeignKey($"fk_{DataConstants.Transaction.TableName}_{DataConstants.Account.TableName}_counterPartyId")
            .FromTable(DataConstants.Transaction.TableName).ForeignColumn(DataConstants.Transaction.CounterPartyIdColumn)
            .ToTable(DataConstants.Account.TableName).PrimaryColumn(DataConstants.Account.IdColumn)
            .OnDelete(Rule.None);

        Execute.Sql("""
                    CREATE INDEX ix_accounts_owner_id_hash
                                   ON accounts USING hash (owner_id)
                    """);
        Create.Index("ix_transactions_account_id_date")
            .OnTable(DataConstants.Transaction.TableName)
            .OnColumn(DataConstants.Transaction.AccountIdColumn).Ascending()
            .OnColumn(DataConstants.Transaction.CreatedAtColumn).Ascending()
            .WithOptions().NonClustered();

        Create.Index("ix_transactions_created_at_gist")
            .OnTable(DataConstants.Transaction.TableName)
            .OnColumn(DataConstants.Transaction.CreatedAtColumn)
            .Ascending();
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TRIGGER IF EXISTS accounts_version_trigger ON """ + DataConstants.Account.TableName + @""";");
        Execute.Sql("DROP FUNCTION IF EXISTS update_version();");
        Execute.Sql("DROP INDEX IF EXISTS ix_accounts_owner_id_hash;");
        Execute.Sql("DROP INDEX IF EXISTS ix_transactions_account_id_date;");
        Execute.Sql("DROP INDEX IF EXISTS ix_transactions_created_at_gist;");
        Delete.FromTable(DataConstants.Account.TableName);
        Delete.FromTable(DataConstants.Transaction.TableName);
    }
}