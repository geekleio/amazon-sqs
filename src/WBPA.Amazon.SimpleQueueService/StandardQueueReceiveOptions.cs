using System;
using WBPA.Amazon.SimpleQueueService.Attributes;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to standard queue receive operations.
    /// </summary>
    /// <seealso cref="AsyncOptions" />
    public class StandardQueueReceiveOptions : AsyncOptions
    {
        private int _maxNumberOfMessages;
        private TimeSpan _visibilityTimeout;
        private TimeSpan _waitTime;
        public const int MinimumAllowedNumberOfMessages = 1;
        public const int MaximumAllowedNumberOfMessages = 10;


        /// <summary>
        /// Initializes a new instance of the <see cref="StandardQueueReceiveOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="StandardQueueReceiveOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="MaxNumberOfMessages"/></term>
        ///         <description><c>1</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="VisibilityTimeout"/></term>
        ///         <description><c>30 seconds</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="WaitTime"/></term>
        ///         <description><c><see cref="TimeSpan.Zero"/></c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public StandardQueueReceiveOptions()
        {
            MaxNumberOfMessages = MinimumAllowedNumberOfMessages;
            VisibilityTimeout = TimeSpan.FromSeconds(30);
            WaitTime = QueueOptions.MinimumReceiveMessageWaitTime;
            MessageAttributeNames = new MessageAttributeNameCollection();
            AttributeNames = new MessageSystemAttributeNameCollection();
        }

        /// <summary>
        /// Gets or sets the maximum number of messages to return.
        /// </summary>
        /// <value>The maximum number of messages to return.</value>
        public int MaxNumberOfMessages
        {
            get => _maxNumberOfMessages;
            set
            {
                if (value > MaximumAllowedNumberOfMessages) { value = MaximumAllowedNumberOfMessages; }
                if (value < MinimumAllowedNumberOfMessages) { value = MinimumAllowedNumberOfMessages; }
                _maxNumberOfMessages = value;
            }
        }

        /// <summary>
        /// Gets or sets the duration that the received messages are hidden from subsequent retrieve requests after being retrieved by a receive request.
        /// </summary>
        /// <value>The duration that the received messages are hidden from subsequent retrieve requests after being retrieved by a receive request.</value>
        public TimeSpan VisibilityTimeout
        {
            get => _visibilityTimeout;
            set
            {
                if (value < TimeSpan.Zero) { value = TimeSpan.Zero; }
                _visibilityTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the duration for which the call waits for a message to arrive in the queue before returning.
        /// </summary>
        /// <value>The duration for which the call waits for a message to arrive in the queue before returning.</value>
        public TimeSpan WaitTime
        {
            get => _waitTime;
            set
            {
                if (value > QueueOptions.MaximumReceiveMessageWaitTime) { value = QueueOptions.MaximumReceiveMessageWaitTime; }
                if (value < QueueOptions.MinimumReceiveMessageWaitTime) { value = QueueOptions.MinimumReceiveMessageWaitTime; }
                _waitTime = value;
            }
        }

        /// <summary>
        /// Gets a collection from where you can add system related attribute names to retrieve with the message.
        /// </summary>
        /// <value>The system related attributes to retrieve with the message.</value>
        public MessageSystemAttributeNameCollection AttributeNames { get; internal set; }

        /// <summary>
        /// Gets a collection from where you can add custom attribute names to retrieve with the message.
        /// </summary>
        /// <value>The custom attributes to retrieve with the message.</value>
        public MessageAttributeNameCollection MessageAttributeNames { get; internal set; }
    }
}