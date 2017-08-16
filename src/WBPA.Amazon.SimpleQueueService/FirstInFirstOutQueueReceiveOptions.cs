namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to first-in-first-out queue receive operations.
    /// </summary>
    /// <seealso cref="StandardQueueReceiveOptions" />
    public class FirstInFirstOutQueueReceiveOptions : StandardQueueReceiveOptions
    {
        private string _receiveRequestAttemptId;

        /// <summary>
        /// Represents the maximum receive request attempt ID length allowed by Amazon SQS.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 128 characters.</remarks>
        public const int MaximumReceiveRequestAttemptIdLength = FirstInFirstOutQueueSendOptions.MaximumMessageDeduplicationIdLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstInFirstOutQueueReceiveOptions"/> class.
        /// </summary>
        public FirstInFirstOutQueueReceiveOptions()
        {
        }

        /// <summary>
        /// Gets or sets the token used for deduplication of receive calls.
        /// </summary>
        /// <value>The token used for deduplication of receive calls.</value>
        public string ReceiveRequestAttemptId
        {
            get => _receiveRequestAttemptId;
            set
            {
                QueueValidator.ThrowIfInvalidMessageId(value, MaximumReceiveRequestAttemptIdLength, nameof(value));
                _receiveRequestAttemptId = value;
            }
        }
    }
}