using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS.Model;
using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Threading;
using WBPA.Amazon.Runtime;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Provides ways for managing messages on AWS SQS.
    /// </summary>
    /// <seealso cref="AmazonSqsManager" />
    public abstract class MessageQueueManager : AmazonSqsManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueManager"/> class.
        /// </summary>
        /// <param name="queueEndpoint">The <see cref="Uri"/> pointing to a queue from which to manage message related operations.</param>
        /// <param name="credentials">The credentials used to authenticate with AWS.</param>
        /// <param name="setup">The <see cref="ManagerOptions"/> which need to be configured.</param>
        protected MessageQueueManager(Uri queueEndpoint, AWSCredentials credentials, Action<ManagerOptions> setup = null) : base(credentials, queueEndpoint.ToRegionEndpoint, setup)
        {
            QueueEndpoint = queueEndpoint;
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> pointing to a queue from which to manage message related operations.
        /// </summary>
        /// <value>The <see cref="Uri"/> pointing to a queue from which to manage message related operations.</value>
        public Uri QueueEndpoint { get; }

        /// <summary>
        /// Delivers a <paramref name="message"/> as an asynchronous operation.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public abstract Task<SendMessageResponse> SendAsync(string message);

        /// <summary>
        /// Delivers up to ten <paramref name="messages"/> as an asynchronous operation.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public abstract Task<SendMessageBatchResponse> SendBatchAsync(IEnumerable<string> messages);

        /// <summary>
        /// Delivers <paramref name="messages"/> in partitions of ten as an asynchronous operation.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>Messages are delivered in partitions of ten through <see cref="SendBatchAsync"/>.</remarks>
        public abstract Task<IEnumerable<SendMessageBatchResponse>> SendManyBatchAsync(IEnumerable<string> messages);

        /// <summary>
        /// Deletes a message from the specified <paramref name="receiptHandle"/> as an asynchronous operation.
        /// </summary>
        /// <param name="receiptHandle">The receipt handle associated with the message to delete.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<DeleteMessageResponse> DeleteAsync(string receiptHandle, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(receiptHandle, nameof(receiptHandle));
            var options = setup.ConfigureOptions();
            var dmr = new DeleteMessageRequest(QueueEndpoint.OriginalString, receiptHandle); 
            return await Client.DeleteMessageAsync(dmr, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes up to ten messages from the specified <paramref name="receiptHandles"/> as an asynchronous operation.
        /// </summary>
        /// <param name="receiptHandles">The receipt handle associated with the message to delete.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<DeleteMessageBatchResponse> DeleteBatchAsync(IEnumerable<string> receiptHandles, Action<BatchOptions> setup = null)
        {
            Validator.ThrowIfNull(receiptHandles, nameof(receiptHandles));
            var options = setup.ConfigureOptions();
            var entries = BatchUtility.CreateBatchRequestEntries(receiptHandles, (receiptHandle, counter) => new DeleteMessageBatchRequestEntry(options.BatchRequestIdGenerator?.Invoke(counter) ?? "Entry{0}".FormatWith(counter), receiptHandle));
            var batch = new DeleteMessageBatchRequest(QueueEndpoint.OriginalString, entries.ToList());
            return await Client.DeleteMessageBatchAsync(batch, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes messages from the specified <paramref name="receiptHandles"/> in partitions of ten as an asynchronous operation.
        /// </summary>
        /// <param name="receiptHandles">The receipt handle associated with the message to delete.</param>
        /// <param name="setup">The <see cref="StandardQueueSendOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>Messages are deleted in partitions of ten through <see cref="DeleteBatchAsync"/>.</remarks>
        public async Task<IEnumerable<DeleteMessageBatchResponse>> DeleteManyBatchAsync(IEnumerable<string> receiptHandles, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(receiptHandles, nameof(receiptHandles));
            var partitions = receiptHandles.ToPartitionCollection(MaximumBatchRequestEntries);
            var batches = new List<DeleteMessageBatchResponse>();
            while (partitions.HasPartitions)
            {
                var partition = partitions.ToList();
                batches.Add(await DeleteBatchAsync(partition, setup).ConfigureAwait(false));
            }
            return batches;
        }

        /// <summary>
        /// Retrieves one or more messages (up to ten) as an asynchronous operation.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public abstract Task<ReceiveMessageResponse> ReceiveAsync();

        /// <summary>
        /// Retrieves one or more messages (up to ten) aggregated to a maximum of <paramref name="approximateCount"/> as an asynchronous operation. Default is 200.
        /// </summary>
        /// <param name="approximateCount">The approximate number of messages to retrieve.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>Messages are retrieved and aggregated by calling <see cref="ReceiveAsync"/>.</remarks>
        public abstract Task<IEnumerable<ReceiveMessageResponse>> ReceiveManyAsync(int approximateCount = 200);

        /// <summary>
        /// Deletes all messages as an asynchronous operation.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<PurgeQueueResponse> PurgeAsync(Action<AsyncOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            var pqr = new PurgeQueueRequest
            {
                QueueUrl = QueueEndpoint.OriginalString
            };
            return await Client.PurgeQueueAsync(pqr, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the attributes associated with the current queue as an asynchronous operation.
        /// </summary>
        /// <param name="setup">The <see cref="QueueAttributeOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<GetQueueAttributesResponse> GetAttributesAsync(Action<QueueAttributeOptions> setup = null)
        {
            return await GetAttributesAsync(QueueEndpoint, setup).ConfigureAwait(false);
        }
    }
}