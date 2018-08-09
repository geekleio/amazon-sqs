using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Amazon.SQS.Model;
using Cuemon;
using Cuemon.Collections.Generic;
using WBPA.Amazon.Attributes;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Extension methods for the <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        public static string GetString(this IDictionary<string, MessageAttributeValue> dic, string key)
        {
            Validator.ThrowIfNull(dic, nameof(dic));
            return dic.GetValueOrDefault(key)?.StringValue;
        }

        public static T GetNumber<T>(this IDictionary<string, MessageAttributeValue> dic, string key) where T : IConvertible
        {
            Validator.ThrowIfNull(dic, nameof(dic));
            var valueOrDefault = dic.GetValueOrDefault(key);
            return valueOrDefault == null ? default(T) : valueOrDefault.StringValue.As<T>();
        }

        public static Stream GetBinary(this IDictionary<string, MessageAttributeValue> dic, string key)
        {
            Validator.ThrowIfNull(dic, nameof(dic));
            return dic.GetValueOrDefault(key)?.BinaryValue;
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="string"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddString(this IDictionary<string, MessageAttributeValue> dic, string key, string value, string labelType = null)
        {
            Add(dic, key, value, labelType, AttributeType.String);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="byte"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, byte value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="sbyte"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, sbyte value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="short"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, short value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="ushort"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, ushort value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="int"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, int value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="uint"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, uint value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="long"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, long value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="ulong"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, ulong value, string labelType = null)
        {
            Add(dic, key, value.ToString(), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="float"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, float value, string labelType = null)
        {
            Add(dic, key, value.ToString(CultureInfo.InvariantCulture), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="decimal"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, decimal value, string labelType = null)
        {
            Add(dic, key, value.ToString(CultureInfo.InvariantCulture), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="double"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddNumber(this IDictionary<string, MessageAttributeValue> dic, string key, double value, string labelType = null)
        {
            Add(dic, key, value.ToString(CultureInfo.InvariantCulture), labelType);
        }

        /// <summary>
        /// Adds an element with the provided key, value and optional label type to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary to extend.</param>
        /// <param name="key">The <see cref="string"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="Stream"/> to use as the value of the element to add.</param>
        /// <param name="labelType">The optional type label to create custom data types.</param>
        public static void AddBinary(this IDictionary<string, MessageAttributeValue> dic, string key, Stream value, string labelType = null)
        {
            Validator.ThrowIfNull(dic, nameof(dic));
            Validator.ThrowIfNullOrWhitespace(key, nameof(key));
            var attribute = new MessageAttributeValue();
            var ms = new MemoryStream();
            value?.CopyTo(ms);
            attribute.DataType = labelType == null ? AttributeType.Binary.ToString() : "{0}.{1}".FormatWith(AttributeType.Binary, labelType);
            attribute.BinaryValue = ms;
            dic.Add(key, attribute);
        }

        private static void Add(IDictionary<string, MessageAttributeValue> dic, string key, string value, string labelType = null, AttributeType type = AttributeType.Number)
        {
            Validator.ThrowIfNull(dic, nameof(dic));
            Validator.ThrowIfNullOrWhitespace(key, nameof(key));
            var attribute = new MessageAttributeValue();
            attribute.DataType = labelType == null ? type.ToString() : "{0}.{1}".FormatWith(type, labelType);
            attribute.StringValue = value;
            dic.Add(key, attribute);
        }
    }
}