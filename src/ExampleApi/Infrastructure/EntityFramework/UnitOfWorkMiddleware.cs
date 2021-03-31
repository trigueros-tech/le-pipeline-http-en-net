using System;
using System.Threading.Tasks;
using System.Transactions;
using ExampleApi.Data;
using Microsoft.AspNetCore.Http;

namespace ExampleApi.Infrastructure.EntityFramework
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;

        public UnitOfWorkMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationContext dbContext)
        {
            var cancellationToken = context.RequestAborted;
            var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            
            try
            {
                await _next(context);
                if (dbContext.ChangeTracker.HasChanges())
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}