using System;
using System.Collections.Generic;
using System.Globalization;
using Amazon.SQS;
using Cuemon;
using Cuemon.Collections.Generic;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is common to a standard queue.
    /// </summary>
    /// <seealso cref="DelayOptions" />
    public class QueueOptions : DelayOptions
    {
        protected readonly Dictionary<string, string> Attributes = new Dictionary<string, string>();

        /// <summary>
        /// Represents the suffix needed for First-In-First-Out (FIFO) queues.
        /// </summary>
        public const string FifoSuffix = ".fifo";

        /// <summary>
        /// Represents the minimum message size allowed by Amazon SQS.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 1 KB.</remarks>
        public const int MinimumMessageSize = 1024;

        /// <summary>
        /// Represents the maximum message size allowed by Amazon SQS.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 256 KB.</remarks>
        public const int MaximumMessageSize = Manager.MaximumRequestSize;

        /// <summary>
        /// Represents the maximum length of time a message is retained by Amazon SQS. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 14 days.</remarks>
        public static readonly TimeSpan MaximumMessageRetentionPeriod = TimeSpan.FromDays(14);

        /// <summary>
        /// Represents the minimum length of time a message is retained by Amazon SQS. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 1 minute.</remarks>
        public static readonly TimeSpan MinimumMessageRetentionPeriod = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Represents the maximum visibility timeout for the queue allowed by Amazon SQS. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 12 hours.</remarks>
        public static readonly TimeSpan MaximumVisibilityTimeout = TimeSpan.FromHours(12);

        /// <summary>
        /// Represents the minimum visibility timeout for the queue allowed by Amazon SQS. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to zero.</remarks>
        public static readonly TimeSpan MinimumVisibilityTimeout = TimeSpan.Zero;

        /// <summary>
        /// Represents the maximum length of time for which a Amazon SQS receive action waits for a message to arrive. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 20 seconds.</remarks>
        public static readonly TimeSpan MaximumReceiveMessageWaitTime = TimeSpan.FromSeconds(20);

        /// <summary>
        /// Represents the minimum length of time for which a Amazon SQS receive action waits for a message to arrive. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to zero.</remarks>
        public static readonly TimeSpan MinimumReceiveMessageWaitTime = TimeSpan.Zero;

        /// <summary>
        /// Represents the maximum visibility timeout for the queue allowed by Amazon SQS. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 24 hours.</remarks>
        public static readonly TimeSpan MaximumEncryptionDataKeyReusePeriod = TimeSpan.FromHours(24);

        /// <summary>
        /// Represents the minimum visibility timeout for the queue allowed by Amazon SQS. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 1 second.</remarks>
        public static readonly TimeSpan MinimumEncryptionDataKeyReusePeriod = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueOptions"/> class.
        /// </summary>
        public QueueOptions()
        {
        }

        /// <summary>
        /// Gets or sets the AWS policy of the queue.
        /// </summary>
        /// <value>The AWS policy of the queue.</value>
        /// <remarks>http://docs.aws.amazon.com/IAM/latest/UserGuide/PoliciesOverview.html</remarks>
        public string Policy
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.Policy);
            set
            {
                if (value == null) { return; }
                Attributes.AddOrUpdate(QueueAttributeName.Policy, value);
            }
        }

        /// <summary>
        /// Gets or sets the redrive policy that includes the parameters for the dead-letter queue functionality of the source queue.
        /// </summary>
        /// <value>The redrive policy that includes the parameters for the dead-letter queue functionality of the source queue.</value>
        /// <remarks>http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-dead-letter-queues.html</remarks>
        public string RedrivePolicy
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.RedrivePolicy);
            set
            {
                if (value == null) { return; }
                Attributes.AddOrUpdate(QueueAttributeName.RedrivePolicy, value);
            }
        }

        /// <summary>
        /// Gets or sets the amount of time for which to delay a message. Default is <see cref="TimeSpan.Zero" />; hence no delay.
        /// </summary>
        /// <value>The amount of time for which to delay a message.</value>
        public override TimeSpan Delay
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.DelaySeconds).ParseWith(s => TimeSpan.FromSeconds(Convert.ToDouble(s)));
            set
            {
                if (value < TimeSpan.Zero) { value = TimeSpan.Zero; }
                if (value > MaximumMessageDelay) { value = MaximumMessageDelay; }
                Attributes.AddOrUpdate(QueueAttributeName.DelaySeconds, value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets the visibility timeout for the queue.
        /// </summary>
        /// <value>The visibility timeout for the queue.</value>
        /// <remarks>http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-visibility-timeout.html</remarks>
        public TimeSpan VisibilityTimeout
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.VisibilityTimeout).ParseWith(s => TimeSpan.FromSeconds(Convert.ToDouble(s)));
            set
            {
                if (value < MinimumVisibilityTimeout) { value = MinimumVisibilityTimeout; }
                if (value > MaximumVisibilityTimeout) { value = MaximumVisibilityTimeout; }
                Attributes.AddOrUpdate(QueueAttributeName.VisibilityTimeout, value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets the length of time for which Amazon SQS retains a message.
        /// </summary>
        /// <value>The length of time for which Amazon SQS retains a message.</value>
        public TimeSpan MessageRetentionPeriod
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.MessageRetentionPeriod).ParseWith(s => TimeSpan.FromSeconds(Convert.ToDouble(s)));
            set
            {
                if (value < MinimumMessageRetentionPeriod) { value = MinimumMessageRetentionPeriod; }
                if (value > MaximumMessageRetentionPeriod) { value = MaximumMessageRetentionPeriod; }
                Attributes.AddOrUpdate(QueueAttributeName.MessageRetentionPeriod, value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets the limit of how many bytes a message can contain before Amazon SQS rejects it.
        /// </summary>
        /// <value>The limit of how many bytes a message can contain before Amazon SQS rejects it.</value>
        public int MessageSize
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.MaximumMessageSize).ParseWith(Convert.ToInt32);
            set
            {
                if (value < MinimumMessageSize) { value = MinimumMessageSize; }
                if (value > MaximumMessageSize) { value = MaximumMessageSize; }
                Attributes.AddOrUpdate(QueueAttributeName.MaximumMessageSize, value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the length of time for which a receive action waits for a message to arrive.
        /// </summary>
        /// <value>The length of time for which a receive action waits for a message to arrive.</value>
        /// <remarks>http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ReceiveMessage.html</remarks>
        public TimeSpan ReceiveMessageWaitTime
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.ReceiveMessageWaitTimeSeconds).ParseWith(s => TimeSpan.FromSeconds(Convert.ToDouble(s)));
            set
            {
                if (value < MinimumReceiveMessageWaitTime) { value = MinimumReceiveMessageWaitTime; }
                if (value > MaximumReceiveMessageWaitTime) { value = MaximumReceiveMessageWaitTime; }
                Attributes.AddOrUpdate(QueueAttributeName.ReceiveMessageWaitTimeSeconds, value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets the ID of an AWS-managed customer master key (CMK) for Amazon SQS or a custom CMK.
        /// </summary>
        /// <value>The ID of an AWS-managed customer master key (CMK) for Amazon SQS or a custom CMK.</value>
        /// <remarks>http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-server-side-encryption.html#sqs-sse-key-terms</remarks>
        public string EncryptionMasterKeyId
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.KmsMasterKeyId);
            set
            {
                if (value == null) { return; }
                Attributes.AddOrUpdate(QueueAttributeName.KmsMasterKeyId, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of time for which Amazon SQS can reuse a data key to encrypt or decrypt messages before calling AWS KMS again.
        /// </summary>
        /// <value>The length of time for which Amazon SQS can reuse a data key to encrypt or decrypt messages before calling AWS KMS again.</value>
        /// <remarks>http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-server-side-encryption.html#sqs-how-does-the-data-key-reuse-period-work</remarks>
        public TimeSpan EncryptionDataKeyReusePeriod
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.KmsDataKeyReusePeriodSeconds).ParseWith(s => TimeSpan.FromSeconds(Convert.ToDouble(s)));
            set
            {
                if (value < MinimumEncryptionDataKeyReusePeriod) { value = MinimumEncryptionDataKeyReusePeriod; }
                if (value > MaximumEncryptionDataKeyReusePeriod) { value = MaximumEncryptionDataKeyReusePeriod; }
                Attributes.AddOrUpdate(QueueAttributeName.KmsDataKeyReusePeriodSeconds, value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets a a value that designates a First-In-First-Out (FIFO) queue.
        /// </summary>
        /// <value><c>true</c> if queue must be a FIFO; otherwise, <c>false</c>.</value>
        /// <remarks>http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/FIFO-queues.html#FIFO-queues-understanding-logic</remarks>
        public bool FifoQueue
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.FifoQueue).ParseWith(s => bool.Parse(s ?? bool.FalseString));
            set => Attributes.AddOrUpdate(QueueAttributeName.FifoQueue, Convert.ToString(value));
        }

        /// <summary>
        /// Gets or sets a value indicating whether content-based deduplication is enabled.
        /// </summary>
        /// <value><c>true</c> if content-based deduplication is enabled; otherwise, <c>false</c>.</value>
        /// <remarks>http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/FIFO-queues.html#FIFO-queues-exactly-once-processing</remarks>
        public bool ContentBasedDeduplication
        {
            get => Attributes.GetValueOrDefault(QueueAttributeName.ContentBasedDeduplication).ParseWith(s => bool.Parse(s ?? bool.FalseString));
            set => Attributes.AddOrUpdate(QueueAttributeName.ContentBasedDeduplication, Convert.ToString(value));
        }

        internal Dictionary<string, string> GetAttributes()
        {
            return Attributes;
        }
    }
}