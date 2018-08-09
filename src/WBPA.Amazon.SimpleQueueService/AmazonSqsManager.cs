using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Cuemon;
using WBPA.Amazon.Runtime;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Provides a base-class for AWS SQS implementations.
    /// </summary>
    /// <seealso cref="Manager{AmazonSQSClient,AmazonSQSConfig}" />
    public abstract class AmazonSqsManager : Manager<AmazonSQSClient, AmazonSQSConfig>
    {
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
        /// Initializes a new instance of the <see cref="AmazonSqsManager"/> class.
        /// </summary>
        /// <param name="credentials">The credentials used to authenticate with AWS.</param>
        /// <param name="regionEndpointParser">The function delegate that will resolve a <see cref="T:Amazon.RegionEndpoint" />.</param>
        /// <param name="setup">The <see cref="T:WBPA.Amazon.Runtime.ManagerOptions" /> which need to be configured.</param>
        protected AmazonSqsManager(AWSCredentials credentials, Func<RegionEndpoint> regionEndpointParser, Action<ManagerOptions> setup = null) : base(credentials, regionEndpointParser, setup)
        {
        }

        /// <summary>
        /// Gets the attributes of the queue <paramref name="endpoint"/> as an asynchronous operation.
        /// </summary>
        /// <param name="endpoint">The <see cref="Uri"/> of the queue to retrieve attributes.</param>
        /// <param name="setup">The <see cref="QueueAttributeOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<GetQueueAttributesResponse> GetAttributesAsync(Uri endpoint , Action<QueueAttributeOptions> setup = null)
        {
            Validator.ThrowIfNull(endpoint, nameof(endpoint));
            var options = setup.ConfigureOptions();
            var gqar = new GetQueueAttributesRequest
            {
                QueueUrl = endpoint.OriginalString,
                AttributeNames = options.AttributeNames.Select(cc => cc.Value).ToList()
            };
            return Client.GetQueueAttributesAsync(gqar, options.CancellationToken);
        }
    }
}