using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Common.Interfaces;

using MediatR;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public const int DEFAULT_BENCHMARK_IN_MILLISECONDS = 2000;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();

            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string requestName = typeof(TRequest).Name;

            var requestProperties = request.GetType().GetProperties().Select(x => x.GetValue(request, null));

            string requestJsonProperties = JsonConvert.SerializeObject(requestProperties);

            _logger.LogInformation($"Running {requestName} : {requestJsonProperties}");

            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > DEFAULT_BENCHMARK_IN_MILLISECONDS)
            {
                _logger.LogWarning($"Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) {request}");
            }

            return response;
        }
    }
}