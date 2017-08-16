using System;
using System.Collections.Generic;
using System.Linq;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// This utility class is designed to make AWS batch related operations easier to work with.
    /// </summary>
    public static class BatchUtility
    {
        /// <summary>
        /// Creates a sequence of objects that is compatible with *BatchRequest classes.
        /// </summary>
        /// <typeparam name="TResult">The type of the entries for the request.</typeparam>
        /// <typeparam name="TInput">The type of the input items.</typeparam>
        /// <param name="items">The sequence of items that need to be converted.</param>
        /// <param name="converter">The function delegate that converts the specified <paramref name="items"/> to a sequence of entries for a request.</param>
        /// <param name="counter">The initial value of the by itereation incremented counter.</param>
        /// <returns>A sequence of entries for a request.</returns>
        public static IEnumerable<TResult> CreateBatchRequestEntries<TResult, TInput>(IEnumerable<TInput> items, Func<TInput, int, TResult> converter, int counter = 0) where TResult : class 
        {
            return items.Select(item =>
            {
                counter++;
                return converter(item, counter);
            });
        }
    }
}