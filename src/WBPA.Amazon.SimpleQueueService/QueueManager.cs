using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS.Model;
using Cuemon;
using Cuemon.Threading;
using WBPA.Amazon.Runtime;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Provides ways for managing queues on AWS SQS.
    /// </summary>
    /// <seealso cref="AmazonSqsManager" />
    public class QueueManager : AmazonSqsManager
    {
        private const int MaximumNameLength = 80;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueManager"/> class.
        /// </summary>
        /// <param name="endpoint">The <see cref="RegionEndpoint"/> from which to manage queues.</param>
        /// <param name="credentials">The credentials used to authenticate with AWS.</param>
        /// <param name="setup">The <see cref="ManagerOptions"/> which need to be configured.</param>
        public QueueManager(RegionEndpoint endpoint, AWSCredentials credentials, Action<ManagerOptions> setup = null) : base(credentials, () => endpoint, setup)
        {
            Validator.ThrowIfNull(endpoint, nameof(endpoint));
        }

        /// <summary>
        /// Creates a new standard or FIFO queue as an asynchronous operation.
        /// </summary>
        /// <param name="name">The name of the new queue.</param>
        /// <param name="setup">The <see cref="QueueOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<CreateQueueResponse> CreateAsync(string name, Action<QueueOptions> setup = null)
        {
            QueueValidator.ThrowIfNameIsNullOrWhitespace(name);
            QueueValidator.ThrowIfNameLengthIsGreaterThan(name, MaximumNameLength);
            QueueValidator.ThrowIfQueueNameHasInvalidCharacters(name);
            var options = setup.ConfigureOptions();
            if (options.FifoQueue) { if (!name.EndsWith(QueueOptions.FifoSuffix, StringComparison.Ordinal)) { name = name + QueueOptions.FifoSuffix; } }
            var cqr = new CreateQueueRequest(name)
            {
                Attributes = options.GetAttributes()
            };
            return await Client.CreateQueueAsync(cqr, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the queue with the specified <paramref name="name"/> as an asynchronous operation.
        /// </summary>
        /// <param name="name">The name of the queue to delete.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<DeleteQueueResponse> DeleteAsync(string name, Action<AsyncOptions> setup = null)
        {
            QueueValidator.ThrowIfNameIsNullOrWhitespace(name);
            QueueValidator.ThrowIfNameLengthIsGreaterThan(name, MaximumNameLength);
            QueueValidator.ThrowIfQueueNameHasInvalidCharacters(name);
            var response = await GetUrlAsync(name, setup).ConfigureAwait(false);
            return await DeleteAsync(response.QueueUrl.ToUri(), setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the queue with the specified <paramref name="endpoint"/> as an asynchronous operation.
        /// </summary>
        /// <param name="endpoint">The URI endpoint of the queue to delete.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<DeleteQueueResponse> DeleteAsync(Uri endpoint, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(endpoint, nameof(endpoint));
            var options = setup.ConfigureOptions();
            var re = endpoint.ToRegionEndpoint();
            Validator.ThrowIfNotEqual(re.SystemName, Client.Config.RegionEndpoint.SystemName, nameof(endpoint), "The specified {0} resolves to AWS {1} but was expected to be AWS {2}.".FormatWith(nameof(endpoint), re.SystemName, Client.Config.RegionEndpoint.SystemName));
            var dqr = new DeleteQueueRequest(endpoint.OriginalString);
            return await Client.DeleteQueueAsync(dqr, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// List queues as an asynchronous operation.
        /// </summary>
        /// <param name="setup">The <see cref="ListQueuesOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<ListQueuesResponse> ListAsync(Action<ListQueuesOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            var lqr = new ListQueuesRequest()
            {
                QueueNamePrefix = options.QueueNamePrefix
            };
            return await Client.ListQueuesAsync(lqr, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the URL of the queue with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the queue to retrieve an URL from.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<GetQueueUrlResponse> GetUrlAsync(string name, Action<AsyncOptions> setup = null)
        {
            QueueValidator.ThrowIfNameIsNullOrWhitespace(name);
            QueueValidator.ThrowIfNameLengthIsGreaterThan(name, MaximumNameLength);
            QueueValidator.ThrowIfQueueNameHasInvalidCharacters(name);
            var options = setup.ConfigureOptions();
            var gqur = new GetQueueUrlRequest(name);
            return await Client.GetQueueUrlAsync(gqur, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the attributes of the queue <paramref name="name"/> as an asynchronous operation.
        /// </summary>
        /// <param name="name">The name of the queue to modify attributes.</param>
        /// <param name="setup">The <see cref="QueueOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<SetQueueAttributesResponse> SetAttributesAsync(string name, Action<AsyncOptions> setup = null)
        {
            QueueValidator.ThrowIfNameIsNullOrWhitespace(name);
            QueueValidator.ThrowIfNameLengthIsGreaterThan(name, MaximumNameLength);
            QueueValidator.ThrowIfQueueNameHasInvalidCharacters(name);
            var response = await GetUrlAsync(name, setup).ConfigureAwait(false);
            return await SetAttributesAsync(response.QueueUrl.ToUri(), setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the attributes of the queue <paramref name="endpoint"/> as an asynchronous operation.
        /// </summary>
        /// <param name="endpoint">The <see cref="Uri"/> of the queue to modify attributes.</param>
        /// <param name="setup">The <see cref="QueueOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<SetQueueAttributesResponse> SetAttributesAsync(Uri endpoint, Action<QueueOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            var sqar = new SetQueueAttributesRequest()
            {
                QueueUrl = endpoint.OriginalString,
                Attributes = options.GetAttributes()
            };
            return await Client.SetQueueAttributesAsync(sqar, options.CancellationToken).ConfigureAwait(false);
        }
    }
}