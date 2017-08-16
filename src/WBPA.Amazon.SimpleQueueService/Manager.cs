using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Cuemon;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Provides a base-class for AWS SQS implementations.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public abstract class Manager : IDisposable
    {
        private volatile bool _isDisposed;
        private readonly Lazy<AmazonSQSClient> _client;

        /// <summary>
        /// The maximum number of entries for a batch request allowed by Amazon SQS.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 10.</remarks>
        public const int MaximumBatchRequestEntries = 10;

        /// <summary>
        /// The maximum request size allowed by Amazon SQS.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 256 KB.</remarks>
        public const int MaximumRequestSize = 256 * 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="Manager"/> class.
        /// </summary>
        /// <param name="credentials">The credentials used to authenticate with AWS.</param>
        /// <param name="regionEndpointParser">The function delegate that will resolve a <see cref="RegionEndpoint"/>.</param>
        /// <param name="setup">The <see cref="ManagerOptions"/> which need to be configured.</param>
        protected Manager(AWSCredentials credentials, Func<RegionEndpoint> regionEndpointParser, Action<ManagerOptions> setup = null)
        {
            Validator.ThrowIfNull(credentials, nameof(credentials));
            Validator.ThrowIfNull(regionEndpointParser, nameof(regionEndpointParser));
            var options = setup.ConfigureOptions();
            _client = new Lazy<AmazonSQSClient>(() =>
            {
                var config = Initializer.Create(new AmazonSQSConfig())
                    .IgnoreMissingMethod(c => c.RegionEndpoint = regionEndpointParser())
                    .IgnoreMissingMethod(c => c.AllowAutoRedirect = options.AllowAutoRedirect)
                    .IgnoreMissingMethod(c => c.ResignRetries = options.ResignRetries)
                    .IgnoreMissingMethod(c => c.AuthenticationRegion = options.AuthenticationRegion)
                    .IgnoreMissingMethod(c => c.AuthenticationServiceName = options.AuthenticationServiceName)
                    .IgnoreMissingMethod(c => c.BufferSize = options.BufferSize)
                    .IgnoreMissingMethod(c => c.DisableLogging = options.DisableLogging)
                    .IgnoreMissingMethod(c => c.LogMetrics = options.LogMetrics)
                    .IgnoreMissingMethod(c => c.MaxConnectionsPerServer = options.MaxConnectionsPerServer)
                    .IgnoreMissingMethod(c => c.LogResponse = options.LogResponse)
                    .IgnoreMissingMethod(c => c.MaxErrorRetry = options.MaxErrorRetry)
                    .IgnoreMissingMethod(c => c.ProgressUpdateInterval = options.ProgressUpdateInterval)
                    .IgnoreMissingMethod(c => c.ProxyCredentials = options.ProxyCredentials)
                    .IgnoreMissingMethod(c => c.ProxyHost = options.ProxyHost)
                    .IgnoreMissingMethod(c => c.ProxyPort = options.ProxyPort)
                    .IgnoreMissingMethod(c => c.SignatureMethod = options.SignatureMethod)
                    .IgnoreMissingMethod(c => c.ThrottleRetries = options.ThrottleRetries)
                    .IgnoreMissingMethod(c => c.Timeout = options.Timeout)
                    .IgnoreMissingMethod(c => c.UseDualstackEndpoint = options.UseDualStackEndpoint)
                    .IgnoreMissingMethod(c => c.UseHttp = options.UseHttp);
                return new AmazonSQSClient(credentials, config.Instance);
            });
        }

        /// <summary>
        /// Gets a reference to the configured <see cref="AmazonSQSClient"/>.
        /// </summary>
        /// <value>The configured <see cref="AmazonSQSClient"/>.</value>
        protected AmazonSQSClient Client => _client.Value;

        /// <summary>
        /// Gets the attributes of the queue <paramref name="endpoint"/> as an asynchronous operation.
        /// </summary>
        /// <param name="endpoint">The <see cref="Uri"/> of the queue to retrieve attributes.</param>
        /// <param name="setup">The <see cref="QueueAttributeOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<GetQueueAttributesResponse> GetAttributesAsync(Uri endpoint , Action<QueueAttributeOptions> setup = null)
        {
            Validator.ThrowIfNull(endpoint, nameof(endpoint));
            var options = setup.ConfigureOptions();
            var gqar = new GetQueueAttributesRequest
            {
                QueueUrl = endpoint.OriginalString,
                AttributeNames = options.AttributeNames.Select(cc => cc.Value).ToList()
            };
            return await Client.GetQueueAttributesAsync(gqar, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (_isDisposed || !disposing) { return; }
            _isDisposed = true;
            Client?.Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}