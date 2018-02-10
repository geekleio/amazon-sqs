using System;
using Cuemon.Collections.Generic;

namespace WBPA.Amazon.SimpleQueueService.Attributes
{
    /// <summary>
    /// Represents a collection of string values that is validated according to the rules by AWS.
    /// </summary>
    /// <seealso cref="ConditionalCollection{T}" />
    public class MessageAttributeNameCollection : ConditionalCollection<string>
    {
        private const int MaximumNameLength = 256;

        /// <summary>
        /// Adds the specified name to this collection.
        /// </summary>
        /// <param name="name">The name of the message attribute.</param>
        public override void Add(string name)
        {
            Add(name, () => 
            {
                QueueValidator.ThrowIfNameIsNullOrWhitespace(name);
                QueueValidator.ThrowIfNameLengthIsGreaterThan(name, MaximumNameLength);
                QueueValidator.ThrowIfAttributeNameHasInvalidCharacters(name);
                QueueValidator.ThrowIfNameHasAmazonPrefix(name);
                QueueValidator.ThrowIfNameHasInvalidPeriodLocation(name);
                QueueValidator.ThrowIfNameHasConsecutivePeriods(name);
                if (Contains(name)) { throw new ArgumentException("Name has already been added.", nameof(name)); }
            });
        }

        /// <summary>
        /// Determines whether this collection contains the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the message attribute.</param>
        /// <returns><c>true</c> if the specified <paramref name="name"/> is contained within this collection; otherwise, <c>false</c>.</returns>
        public override bool Contains(string name)
        {
            return Contains(name, StringComparer.Ordinal);
        }

        /// <summary>
        /// Removes the specified <paramref name="name"/> from this collection.
        /// </summary>
        /// <param name="name">The name of the message attribute.</param>
        /// <returns><c>true</c> if the <paramref name="name"/> was removed, <c>false</c> otherwise.</returns>
        public override bool Remove(string name)
        {
            return Remove(name, s => s == name, StringComparer.Ordinal);
        }
    }
}