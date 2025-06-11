using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuldingBlock.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            const string prefix = nameof(LoggingBehavior<TRequest, TResponse>);

            var requestType = typeof(TRequest).Name;
            var responseType = typeof(TResponse).Name;

            _logger.LogInformation("[{Prefix}] Handling request {RequestType} and expecting response {ResponseType}",
                prefix, requestType, responseType);

            var timer = Stopwatch.StartNew();

            var response = await next();

            timer.Stop();
            var elapsedSeconds = timer.Elapsed.TotalSeconds;

            if (elapsedSeconds > 3)
            {
                _logger.LogWarning("[{Prefix}] The request {RequestType} took {ElapsedSeconds:N2} seconds, which exceeds the threshold.",
                    prefix, requestType, elapsedSeconds);
            }

            _logger.LogInformation("[{Prefix}] Handled request {RequestType} in {ElapsedSeconds:N2} seconds.",
                prefix, requestType, elapsedSeconds);

            return response;
        }
    }
}
