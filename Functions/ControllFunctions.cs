using Microsoft.EntityFrameworkCore.Storage;
using ServerApi.Context;
using System.Transactions;

namespace ServerApi.Functions
{
    public class ControllFunctions
    {
        readonly ContextFile context;
        public ControllFunctions(ContextFile context)
        {
            this.context = context;
        }

        public async Task<IDbContextTransaction> StartTransaction()
        {
            return await context.Database.BeginTransactionAsync();
        }
    }
}
