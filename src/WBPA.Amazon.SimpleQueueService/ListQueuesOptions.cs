using Cuemon.Threading;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to list queues operations.
    /// </summary>
    /// <seealso cref="AsyncOptions" />
    public class ListQueuesOptions : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListQueuesOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ListQueuesOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="QueueNamePrefix"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ListQueuesOptions()
        {
        }

        /// <summary>
        /// Gets or sets the name to use for filtering the list results. Only those queues whose name begins with the specified string are returned.
        /// </summary>
        /// <value>The name to use for filtering the list results.</value>
        /// <remarks>Queue names are case-sensitive.</remarks>
        public string QueueNamePrefix { get; set; }
    }
}