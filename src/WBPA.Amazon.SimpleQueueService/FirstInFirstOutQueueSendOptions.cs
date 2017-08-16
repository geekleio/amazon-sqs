using System;
using System.Collections.Generic;
using Amazon.SQS.Model;
using Cuemon;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to first-in-first-out queue send operations.
    /// </summary>
    public class FirstInFirstOutQueueSendOptions : AsyncOptions
    {
        private string _messageGroupId;
        private string _messageDeduplicationId;

        /// <summary>
        /// Represents the maximum message group ID length allowed by Amazon SQS.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 128 characters.</remarks>
        public const int MaximumMessageGroupIdLength = 128;

        /// <summary>
        /// Represents the maximum message deduplication ID length allowed by Amazon SQS.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 128 characters.</remarks>
        public const int MaximumMessageDeduplicationIdLength = MaximumMessageGroupIdLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstInFirstOutQueueSendOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FirstInFirstOutQueueSendOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="BatchRequestIdGenerator"/></term>
        ///         <description><c>c => "Entry{0}".FormatWith(c);</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="BatchMessageDeduplicationIdGenerator"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MessageDeduplicationId"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MessageGroupId"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FirstInFirstOutQueueSendOptions()
        {
            MessageAttributes = new Dictionary<string, MessageAttributeValue>();
            BatchRequestIdGenerator = c => "Entry{0}".FormatWith(c);
        }

        /// <summary>
        /// Gets the message attributes.
        /// </summary>
        /// <value>The message attributes to store with the message.</value>
        public Dictionary<string, MessageAttributeValue> MessageAttributes { get; }

        /// <summary>
        /// Gets or sets the function delegate that provides a unique identifier for a message within a batch request.
        /// </summary>
        /// <value>The function delegate that provides a unique identifier for a message within a batch request.</value>
        public Func<int, string> BatchRequestIdGenerator { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that provides a unique token used for deduplication of sent messages within a batch request.
        /// </summary>
        /// <value>The function delegate that provides a unique token used for deduplication of sent messages within a batch request.</value>
        public Func<int, string> BatchMessageDeduplicationIdGenerator { get; set; }

        /// <summary>
        /// Gets or sets the token used for deduplication of sent messages.
        /// </summary>
        /// <value>The token used for deduplication of sent messages.</value>
        public string MessageDeduplicationId
        {
            get => _messageDeduplicationId;
            set
            {
                QueueValidator.ThrowIfInvalidMessageId(value, MaximumMessageDeduplicationIdLength, nameof(value));
                _messageDeduplicationId = value;
            }
        }

        /// <summary>
        /// Gets or sets the tag that specifies that a message belongs to a specific message group. 
        /// </summary>
        /// <value>The tag that specifies that a message belongs to a specific message group.</value>
        public string MessageGroupId
        {
            get
            {
                if (_messageGroupId.IsNullOrWhiteSpace()) { throw new ArgumentException("A first-in-first-out message must be assigned a MessageGroupId."); }
                return _messageGroupId;
            }
            set
            {
                QueueValidator.ThrowIfInvalidMessageId(value, MaximumMessageGroupIdLength, nameof(value));
                _messageGroupId = value;
            }
        }
    }
}