namespace AccountService.Utils.Data;

public static class DataConstants
{
    public static class Account
    {
        public const string TableName = "accounts";
        public const string IdColumn = "id";
        public const string OwnerIdColumn = "owner_id";
        public const string TypeColumn = "type";
        public const string CurrencyColumn = "currency";
        public const string BalanceColumn = "balance";
        public const string PercentColumn = "percent";
        public const string CreatedAtColumn = "created_at";
        public const string ClosedAtColumn = "closed_at";
        public const string VersionColumn = "version";
    }

    public static class Transaction
    {
        public const string TableName = "transactions";
        public const string IdColumn = "id";
        public const string AccountIdColumn = "account_id";
        public const string CounterPartyIdColumn = "counterparty_id";
        public const string AmountColumn = "amount";
        public const string CurrencyColumn = "currency";
        public const string TypeColumn = "type";
        public const string DescriptionColumn = "description";
        public const string CreatedAtColumn = "created_at";
    }
}