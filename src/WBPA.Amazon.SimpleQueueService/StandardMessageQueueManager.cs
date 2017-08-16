using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.SQS.Model.Internal.MarshallTransformations;
using Cuemon;
using Cuemon.Collections.Generic;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Provides ways for managing messages on standard AWS SQS. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="MessageQueueManager" />
    public sealed class StandardMessageQueueManager : MessageQueueManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardMessageQueueManager"/> class.
        /// </summary>
        /// <param name="queueEndpoint">The <see cref="Uri" /> pointing to a queue from which to manage message related operations.</param>
        /// <param name="credentials">The credentials used to authenticate with AWS.</param>
        /// <param name="setup">The <see cref="ManagerOptions" /> which need to be configured.</param>
        public StandardMessageQueueManager(Uri queueEndpoint, AWSCredentials credentials, Action<ManagerOptions> setup = null) : base(queueEndpoint, credentials, setup)
        {
        }

        /// <summary>
        /// Delivers a <paramref name="message"/> as an asynchronous operation.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override async Task<SendMessageResponse> SendAsync(string message)
        {
            return await SendAsync(message, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Delivers a <paramref name="message"/> as an asynchronous operation.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="setup">The <see cref="StandardQueueSendOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<SendMessageResponse> SendAsync(string message, Action<StandardQueueSendOptions> setup)
        {
            Validator.ThrowIfNullOrWhitespace(message, nameof(message));
            var options = setup.ConfigureOptions();
            var smr = new SendMessageRequest(QueueEndpoint.OriginalString, message)
            {
                DelaySeconds = (int)options.Delay.TotalSeconds,
                MessageAttributes = options.MessageAttributes
            }.ValidateRequestSize(new SendMessageRequestMarshaller());
            return await Client.SendMessageAsync(smr, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Delivers up to ten <paramref name="messages"/> as an asynchronous operation.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override async Task<SendMessageBatchResponse> SendBatchAsync(IEnumerable<string> messages)
        {
            return await SendBatchAsync(messages, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Delivers up to ten <paramref name="messages"/> as an asynchronous operation.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <param name="setup">The <see cref="StandardQueueSendOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<SendMessageBatchResponse> SendBatchAsync(IEnumerable<string> messages, Action<StandardQueueSendOptions> setup)
        {
            Validator.ThrowIfNull(messages, nameof(messages));
            var options = setup.ConfigureOptions();
            return await SendBatchCoreAsync(messages, options).ConfigureAwait(false);
        }

        private async Task<SendMessageBatchResponse> SendBatchCoreAsync(IEnumerable<string> messages, StandardQueueSendOptions options, int initialCounter = 0)
        {
            var entries = BatchUtility.CreateBatchRequestEntries(messages, (message, counter) => new SendMessageBatchRequestEntry(options.BatchRequestIdGenerator?.Invoke(counter) ?? "Entry{0}".FormatWith(counter), message)
            {
                DelaySeconds = (int)options.Delay.TotalSeconds,
                MessageAttributes = options.MessageAttributes
            }).ToList();
            if (entries.Count > MaximumBatchRequestEntries) { throw new ArgumentOutOfRangeException(nameof(messages), entries.Count, "Maximum number of messages per batch request is {0}.".FormatWith(MaximumBatchRequestEntries)); }
            var batch = new SendMessageBatchRequest(QueueEndpoint.OriginalString, entries).ValidateRequestSize(new SendMessageBatchRequestMarshaller());
            return await Client.SendMessageBatchAsync(batch, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Delivers <paramref name="messages"/> in partitions of ten as an asynchronous operation.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>Messages are delivered in partitions of ten through <see cref="SendBatchAsync(IEnumerable{string})"/>.</remarks>
        public override async Task<IEnumerable<SendMessageBatchResponse>> SendManyBatchAsync(IEnumerable<string> messages)
        {
            return await SendManyBatchAsync(messages, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Delivers <paramref name="messages"/> in partitions of ten as an asynchronous operation.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <param name="setup">The <see cref="StandardQueueSendOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>Messages are delivered in partitions of ten through <see cref="SendBatchAsync(IEnumerable{string})"/>.</remarks>
        public async Task<IEnumerable<SendMessageBatchResponse>> SendManyBatchAsync(IEnumerable<string> messages, Action<StandardQueueSendOptions> setup)
        {
            Validator.ThrowIfNull(messages, nameof(messages));
            var options = setup.ConfigureOptions();
            var partitions = messages.ToPartitionCollection(MaximumBatchRequestEntries);
            var batches = new List<SendMessageBatchResponse>();
            var processed = 0;
            while (partitions.HasPartitions)
            {
                var partition = partitions.ToList();
                batches.Add(await SendBatchCoreAsync(partition, options, processed).ConfigureAwait(false));
                processed += partition.Count;
            }
            return batches;
        }

        /// <summary>
        /// Retrieves one or more messages (up to ten) as an asynchronous operation.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public override async Task<ReceiveMessageResponse> ReceiveAsync()
        {
            return await ReceiveAsync(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves one or more messages (up to ten) as an asynchronous operation.
        /// </summary>
        /// <param name="setup">The <see cref="StandardQueueReceiveOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<ReceiveMessageResponse> ReceiveAsync(Action<StandardQueueReceiveOptions> setup)
        {
            var options = setup.ConfigureOptions();
            var rmr = new ReceiveMessageRequest(QueueEndpoint.OriginalString)
            {
                MaxNumberOfMessages = options.MaxNumberOfMessages,
                VisibilityTimeout = (int)options.VisibilityTimeout.TotalSeconds,
                WaitTimeSeconds = (int)options.WaitTime.TotalSeconds,
                AttributeNames = options.AttributeNames.Select(cc => cc.Value).ToList(),
                MessageAttributeNames = options.MessageAttributeNames.ToList()
            };
            return await Client.ReceiveMessageAsync(rmr, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves one or more messages (up to ten) aggregated to a maximum of <paramref name="approximateCount"/> as an asynchronous operation. Default is 200.
        /// </summary>
        /// <param name="approximateCount">The approximate number of messages to retrieve.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>Messages are retrieved and aggregated by calling <see cref="ReceiveAsync()"/>.</remarks>
        public override async Task<IEnumerable<ReceiveMessageResponse>> ReceiveManyAsync(int approximateCount = 200)
        {
            return await ReceiveManyAsync(approximateCount, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves one or more messages (up to ten) aggregated to a maximum of <paramref name="approximateCount"/> as an asynchronous operation.
        /// </summary>
        /// <param name="approximateCount">The approximate number of messages to retrieve.</param>
        /// <param name="setup">The <see cref="StandardQueueReceiveOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>Messages are retrieved and aggregated by calling <see cref="ReceiveAsync()"/>.</remarks>
        public async Task<IEnumerable<ReceiveMessageResponse>> ReceiveManyAsync(int approximateCount, Action<StandardQueueReceiveOptions> setup)
        {
            Validator.ThrowIfLowerThanOrEqual(approximateCount, 0, nameof(approximateCount), "ApproximateCount must have a value that is greater than 0.");
            var options = setup.ConfigureOptions();
            var attributes = await GetAttributesAsync(o => o.AttributeNames.Add(QueueAttributeName.ApproximateNumberOfMessages)).ConfigureAwait(false);
            if (attributes.ApproximateNumberOfMessages < approximateCount) { approximateCount = attributes.ApproximateNumberOfMessages; }
            var responses = new List<Task<ReceiveMessageResponse>>();
            while (approximateCount > 0)
            {
                responses.Add(ReceiveAsync(setup));
                approximateCount -= options.MaxNumberOfMessages;
            }
            return await Task.WhenAll(responses).ConfigureAwait(false);
        }
    }
}