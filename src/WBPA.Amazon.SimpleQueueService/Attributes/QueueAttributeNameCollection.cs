using Amazon.SQS;

namespace WBPA.Amazon.SimpleQueueService.Attributes
{
    /// <summary>
    /// Represents a collection of <see cref="QueueAttributeName"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ConstantClassCollection{QueueAttributeName}" />
    public sealed class QueueAttributeNameCollection : ConstantClassCollection<QueueAttributeName>
    {
        internal QueueAttributeNameCollection()
        {
        }
    }
}