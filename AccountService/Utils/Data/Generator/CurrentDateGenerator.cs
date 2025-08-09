using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AccountService.Utils.Data.Generator;

public class CurrentDateGenerator : ValueGenerator<DateTimeOffset>
{
    public override DateTimeOffset Next(EntityEntry entry)
    {
        return DateTimeOffset.Now;
    }

    public override bool GeneratesTemporaryValues => false;
}