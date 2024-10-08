
namespace OT.Assessment.Api
{
    public class ApiWarmHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<ApiWarmHostedService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiWarmHostedService(
            ILogger<ApiWarmHostedService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("API Call Hosted Service is starting.");

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7120/api/warm");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Warm API Call succeeded with response: {0}", responseData);
                }
                else
                {
                    _logger.LogError("Warm API Call failed with status code: {0}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling the API endpoint.");
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("API Call Hosted Service is stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
