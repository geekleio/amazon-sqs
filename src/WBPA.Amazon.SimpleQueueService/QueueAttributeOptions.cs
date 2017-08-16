using WBPA.Amazon.SimpleQueueService.Attributes;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to <see cref="Manager"/> operations.
    /// </summary>
    /// <seealso cref="AsyncOptions" />
    public class QueueAttributeOptions : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueAttributeOptions"/> class.
        /// </summary>
        public QueueAttributeOptions()
        {
            AttributeNames = new QueueAttributeNameCollection();
        }

        /// <summary>
        /// Gets a collection from which attributes can be added and subsequently retrieve information from AWS.
        /// </summary>
        /// <value>The attributes to retrieve from AWS.</value>
        public QueueAttributeNameCollection AttributeNames { get; internal set; }
    }
}