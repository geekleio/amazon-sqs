using System;
using Cuemon;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to queue batch operations.
    /// </summary>
    public class BatchOptions : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="BatchOptions"/>.
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
        public BatchOptions()
        {
            BatchRequestIdGenerator = c => "Entry{0}".FormatWith(c);
        }

        /// <summary>
        /// Gets or sets the function delegate that provides a unique identifier for a message within a batch request.
        /// </summary>
        /// <value>The function delegate that provides a unique identifier for a message within a batch request.</value>
        public Func<int, string> BatchRequestIdGenerator { get; set; }
    }
}