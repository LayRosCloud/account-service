
using FluentMigrator;

namespace AccountService.Utils.Data.Migrations;

[Migration(2, "add event schema and column frozen")]
public class EventSchema : Migration
{
    public override void Up()
    {
        Create.Table("events")
            .WithColumn("event_id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("processed_at").AsDateTime().NotNullable()
            .WithColumn("handler").AsString(100).NotNullable();
        Create.Column("is_frozen").OnTable(DataConstants.Account.TableName).AsBoolean().Nullable();
    }

    public override void Down()
    {
        Delete.Table("events");
        Delete.Column("is_frozen");
    }
}