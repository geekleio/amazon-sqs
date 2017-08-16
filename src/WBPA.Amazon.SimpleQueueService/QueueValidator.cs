using System;
using Cuemon;

namespace WBPA.Amazon.SimpleQueueService
{
    internal static class QueueValidator
    {
        private const char Period = '.';

        private static readonly string StringPeriod = Period.ToString();

        internal static readonly string[] InvalidMessageAttributeNamePrefixes = { "AWS", "Amazon" };

        internal static readonly string ValidQueueNameCharacters = string.Concat(StringUtility.AlphanumericCharactersCaseSensitive, "_-");

        internal static readonly string ValidAttributeNameCharacters = string.Concat(ValidQueueNameCharacters, ".");

        internal const string ValidFifoMessageIdPunctuationMarks = "(!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~).";

        internal static readonly string ValidFifoMessageIdCharacters = string.Concat(StringUtility.AlphanumericCharactersCaseSensitive, ValidFifoMessageIdPunctuationMarks);

        internal static void ThrowIfInvalidMessageId(string messageTypeId, int length, string paramName)
        {
            Validator.ThrowIfNullOrWhitespace(messageTypeId, paramName);
            Validator.ThrowIfGreaterThan(messageTypeId.Length, length, paramName);
            if (IsNotValid(ValidAttributeNameCharacters, messageTypeId)) { throw new ArgumentException("One or more characters in the {0} are invalid; only alphanumeric characters and these punctuation marks {1} are allowed.".FormatWith(paramName, ValidFifoMessageIdPunctuationMarks), nameof(messageTypeId)); }
        }

        internal static void ThrowIfNameIsNullOrWhitespace(string name)
        {
            Validator.ThrowIfNullOrWhitespace(name, nameof(name));
        }

        internal static void ThrowIfNameLengthIsGreaterThan(string name, int length)
        {
            Validator.ThrowIfGreaterThan(name.Length, length, nameof(name), "Name cannot exceed a length of {0} characters. Actual size was {1}.".FormatWith(length, name.Length));
        }

        internal static void ThrowIfNameHasAmazonPrefix(string name)
        {
            if (name.StartsWith(InvalidMessageAttributeNamePrefixes)) { throw new ArgumentException("Name cannot start with {0} (or any casing variants).".FormatWith(InvalidMessageAttributeNamePrefixes.ToDelimitedString(" or ")), nameof(name)); }
        }

        internal static void ThrowIfQueueNameHasInvalidCharacters(string name)
        {
            if (IsNotValid(ValidQueueNameCharacters, name)) { throw new ArgumentException("One or more characters in the name are invalid; only alphanumeric characters, underscore (_) and hyphen (-) are allowed.", nameof(name)); }
        }

        internal static void ThrowIfAttributeNameHasInvalidCharacters(string name)
        {
            if (IsNotValid(ValidAttributeNameCharacters, name)) { throw new ArgumentException("One or more characters in the name are invalid; only alphanumeric characters, underscore (_), hyphen (-), and period (.) are allowed.", nameof(name)); }
        }

        internal static void ThrowIfNameHasInvalidPeriodLocation(string name)
        {
            if (name.StartsWith(StringPeriod) || name.EndsWith(StringPeriod)) { throw new ArgumentException("Name must not start or end with a period (.).", nameof(name)); }
        }

        internal static void ThrowIfNameHasConsecutivePeriods(string name)
        {
            if (StringUtility.HasConsecutiveCharacters(name, Period)) { throw new ArgumentException("Name cannot have periods in succession (..).", nameof(name)); }
        }

        public static bool IsNotValid(string validChars, string chars)
        {
            return StringUtility.ParseDistinctDifference(validChars, chars, out _);
        }
    }
}