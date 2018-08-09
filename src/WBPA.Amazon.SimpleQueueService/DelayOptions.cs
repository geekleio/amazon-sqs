using System;
using Cuemon.Threading;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is common to queue delay operations.
    /// </summary>
    public class DelayOptions : AsyncOptions
    {
        private TimeSpan _delay = TimeSpan.Zero;

        /// <summary>
        /// Represents the maximum length of time for which the delivery of all messages in the queue is delayed. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to 15 seconds.</remarks>
        public static readonly TimeSpan MaximumMessageDelay = TimeSpan.FromSeconds(15);

        /// <summary>
        /// Represents the minimum length of time for which the delivery of all messages in the queue is delayed. This field is read-only.
        /// </summary>
        /// <remarks>The value of this field is equivalent to zero.</remarks>
        public static readonly TimeSpan MinimumMessageDelay = TimeSpan.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="DelayOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Delay"/></term>
        ///         <description><see cref="TimeSpan.Zero"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public DelayOptions()
        {
        }

        /// <summary>
        /// Gets or sets the amount of time for which to delay a message. Default is <see cref="TimeSpan.Zero"/>; hence no delay.
        /// </summary>
        /// <value>The amount of time for which to delay a message.</value>
        public virtual TimeSpan Delay
        {
            get => _delay;
            set
            {
                if (value < TimeSpan.Zero) { value = TimeSpan.Zero; }
                if (value > MaximumMessageDelay) { value = MaximumMessageDelay; }
                _delay = value;
            }
        }
    }
}