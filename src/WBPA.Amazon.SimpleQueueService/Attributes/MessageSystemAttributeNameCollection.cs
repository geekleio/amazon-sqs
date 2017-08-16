using Amazon.SQS;

namespace WBPA.Amazon.SimpleQueueService.Attributes
{
    /// <summary>
    /// Represents a collection of <see cref="MessageSystemAttributeName"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ConstantClassCollection{MessageSystemAttributeName}" />
    public sealed class MessageSystemAttributeNameCollection : ConstantClassCollection<MessageSystemAttributeName>
    {
        internal MessageSystemAttributeNameCollection()
        {
        }
    }
}