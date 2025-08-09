using System.ComponentModel.DataAnnotations;
using AccountService.Features.Accounts.Dto;
using MediatR;

namespace AccountService.Features.Accounts.FindByIdAccountExtract;

public class FindByIdAccountExtractQuery : IRequest<AccountResponseFullDto>
{
    public FindByIdAccountExtractQuery(Guid accountId, DateTimeOffset dateStart, DateTimeOffset dateEnd)
    {
        AccountId = accountId;
        DateStart = dateStart;
        DateEnd = dateEnd;
    }

    /// <summary>
    /// date start transactions
    /// </summary>
    public DateTimeOffset DateStart { get; }

    /// <summary>
    /// date end transactions
    /// </summary>
    public DateTimeOffset DateEnd { get; }

    /// <summary>
    /// account id
    /// </summary>
    [Required]
    public Guid AccountId { get; }
}