using System;
using Cuemon.Collections.Generic;

namespace WBPA.Amazon.SimpleQueueService.Attributes
{
    /// <summary>
    /// Represents a collection of string values that is validated according to the rules by AWS. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ConditionalCollection{T}" />
    public sealed class MessageAttributeNameCollection : ConditionalCollection<string>
    {
        private const int MaximumNameLength = 256;

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

        public override bool Contains(string name)
        {
            return Contains(name, StringComparer.Ordinal);
        }

        public override bool Remove(string name)
        {
            return Remove(name, s => s == name, StringComparer.Ordinal);
        }
    }
}