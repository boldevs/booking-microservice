using System.Text.Json;
using BuldingBlock.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuldingBlock.EFCore
{
    public class EfTxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<EfTxBehavior<TRequest, TResponse>> _logger;
        private readonly IDbContext _dbContextBase;
        private readonly IBusPublisher _busPublisher;

        public EfTxBehavior(
            ILogger<EfTxBehavior<TRequest, TResponse>> logger,
            IDbContext dbContextBase,
            IBusPublisher busPublisher)
        {
            _logger = logger;
            _dbContextBase = dbContextBase;
            _busPublisher = busPublisher;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestType = typeof(TRequest).FullName;

            _logger.LogInformation(
                "{Prefix} Handling command {RequestType}",
                nameof(EfTxBehavior<TRequest, TResponse>),
                requestType);

            _logger.LogDebug(
                "{Prefix} Request content for {RequestType}: {RequestContent}",
                nameof(EfTxBehavior<TRequest, TResponse>),
                requestType,
                JsonSerializer.Serialize(request));

            _logger.LogInformation(
                "{Prefix} Beginning transaction for {RequestType}",
                nameof(EfTxBehavior<TRequest, TResponse>),
                requestType);

            await _dbContextBase.BeginTransactionAsync(cancellationToken);

            try
            {
                var response = await next();

                _logger.LogInformation(
                    "{Prefix} Executed the {RequestType} request",
                    nameof(EfTxBehavior<TRequest, TResponse>),
                    requestType);

                var domainEvents = _dbContextBase.GetDomainEvents();

                _logger.LogInformation(
                    "{Prefix} Publishing {EventCount} domain events for {RequestType}",
                    nameof(EfTxBehavior<TRequest, TResponse>),
                    domainEvents.Count,
                    requestType);

                await _busPublisher.SendAsync(domainEvents.ToArray(), cancellationToken);

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
