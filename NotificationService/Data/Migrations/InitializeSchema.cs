using FluentMigrator;

namespace NotificationService.Data.Migrations;

[Migration(1)]
public class InitializeSchema : Migration
{
    public override void Up()
    {
        Create.Table("events")
            .WithColumn("event_id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("processed_at").AsDateTime().NotNullable()
            .WithColumn("handler").AsString(100).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("events");
    }
}