using Amazon.SQS;
using Amazon.SQS.Util;

namespace WBPA.Amazon.SimpleQueueService.Attributes
{
    /// <summary>
    /// Extension methods for the <see cref="ConstantClassCollection{T}"/>.
    /// </summary>
    public static class AttributeNameExtensions
    {
        /// <summary>
        /// A shortcut method for retrieving all attributes.
        /// </summary>
        /// <param name="col">The collection to extend.</param>
        public static void GetAll(this MessageSystemAttributeNameCollection col)
        {
            col.Add(SQSConstants.ATTRIBUTE_ALL);
        }

        /// <summary>
        /// A shortcut method for retrieving all attributes.
        /// </summary>
        /// <param name="col">The collection to extend.</param>
        public static void GetAll(this MessageAttributeNameCollection col)
        {
            col.Add(SQSConstants.ATTRIBUTE_ALL);
        }

        /// <summary>
        /// A shortcut method for retrieving all attributes.
        /// </summary>
        /// <param name="col">The collection to extend.</param>
        public static void GetAll(this QueueAttributeNameCollection col)
        {
            col.Add(QueueAttributeName.All);
        }
    }
}