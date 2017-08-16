using System;
using System.Collections.Generic;
using Amazon.SQS.Model;
using Cuemon;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to standard queue send operations.
    /// </summary>
    public class StandardQueueSendOptions : DelayOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardQueueSendOptions"/> class.
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
        /// </list>
        /// </remarks>
        public StandardQueueSendOptions()
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
    }
}