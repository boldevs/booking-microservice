using System.Data;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuldingBlock.EFCore
{
    public class EfIdentityTxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<EfIdentityTxBehavior<TRequest, TResponse>> _logger;
        private readonly IDbContext _dbContextBase;

        public EfIdentityTxBehavior(
            ILogger<EfIdentityTxBehavior<TRequest, TResponse>> logger,
            IDbContext dbContextBase)
        {
            _logger = logger;
            _dbContextBase = dbContextBase;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestType = typeof(TRequest).FullName;

            _logger.LogInformation(
                "{Prefix} Handling command {RequestType}",
                nameof(EfIdentityTxBehavior<TRequest, TResponse>),
                requestType);

            _logger.LogDebug(
                "{Prefix} Request content for {RequestType}: {RequestContent}",
                nameof(EfIdentityTxBehavior<TRequest, TResponse>),
                requestType,
                JsonSerializer.Serialize(request));

            _logger.LogInformation(
                "{Prefix} Beginning transaction for {RequestType}",
                nameof(EfIdentityTxBehavior<TRequest, TResponse>),
                requestType);

            await _dbContextBase.BeginTransactionAsync(cancellationToken);

            try
            {
                var response = await next();

                _logger.LogInformation(
                    "{Prefix} Successfully handled {RequestType}",
                    nameof(EfIdentityTxBehavior<TRequest, TResponse>),
                    requestType);

                await _dbContextBase.CommitTransactionAsync(cancellationToken);

                return response;
            }
            catch
            {
                await _dbContextBase.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
