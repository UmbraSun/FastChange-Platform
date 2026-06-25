using Application.Features.Transactions.GetTransactionHistory;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mapping;

public sealed class TransactionMappings
    : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<
            Transaction,
            GetTransactionHistoryResponse>();
    }
}
