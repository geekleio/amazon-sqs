namespace WBPA.Amazon.SimpleQueueService.Attributes
{
    /// <summary>
    /// Specifies how message attribute data types identify how the message attribute values are handled by Amazon SQS.
    /// </summary>
    public enum MessageAttributeType
    {
        /// <summary>
        /// Strings are Unicode with UTF-8 binary encoding.
        /// </summary>
        String,
        /// <summary>
        /// Numbers are positive or negative integers or floating point numbers. 
        /// </summary>
        Number,
        /// <summary>
        /// Binary type attributes can store any binary data, for example, compressed data, encrypted data, or images.
        /// </summary>
        Binary
    }
}