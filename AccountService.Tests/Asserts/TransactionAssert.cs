using AccountService.Features.Transactions;
using AccountService.Features.Transactions.CreateTransaction;
using AccountService.Features.Transactions.Dto;
using AccountService.Features.Transactions.TransferBetweenAccounts;

namespace AccountService.Tests.Asserts;

public class TransactionAssert
{
    public static void AssertTransactions(Transaction transaction, TransactionFullDto dto)
    {
        Assert.NotNull(dto);
        Assert.Equal(transaction.Type, dto.Type);
        Assert.Equal(transaction.Sum, dto.Sum);
        Assert.Equal(transaction.Currency, dto.Currency);
        Assert.Equal(transaction.CounterPartyAccountId, dto.CounterPartyAccountId);
        Assert.Equal(transaction.AccountId, dto.AccountId);
        Assert.Equal(transaction.CreatedAt, dto.CreatedAt);
        Assert.Equal(transaction.Id, dto.Id);
    }

    public static void AssertTransactions(Transaction transaction, CreateTransactionCommand dto)
    {
        Assert.NotNull(dto);
        Assert.Equal(transaction.Type, dto.Type);
        Assert.Equal(transaction.Sum, dto.Sum);
        Assert.Equal(transaction.AccountId, dto.AccountId);
        Assert.Equal(transaction.Description, dto.Description);
    }

    public static void AssertTransactions(Transaction transaction, TransferBetweenAccountsCommand dto)
    {
        Assert.NotNull(dto);
        Assert.Equal(transaction.CounterPartyAccountId, dto.CounterPartyAccountId);
        Assert.Equal(transaction.Description, dto.Description);
        Assert.Equal(transaction.Sum, dto.Sum);
        Assert.Equal(transaction.AccountId, dto.AccountId);
    }
}