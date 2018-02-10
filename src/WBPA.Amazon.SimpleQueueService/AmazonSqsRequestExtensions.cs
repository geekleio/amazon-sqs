using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.SQS;
using Cuemon;
using WBPA.Amazon.Runtime;
using WBPA.Amazon.Runtime.Internal;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Extension methods for the <see cref="AmazonSQSRequest"/> object.
    /// </summary>
    public static class AmazonSqsRequestExtensions
    {
        /// <summary>
        /// Validates the size of the specified <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="TMarshaller">The type of the AWS marshaller.</typeparam>
        /// <typeparam name="T">The type of the request object.</typeparam>
        /// <param name="request">The <see cref="AmazonSQSRequest"/> to validate.</param>
        /// <param name="marshaller">The marshaller that will convert the <paramref name="request"/> object to an AWS HTTP request.</param>
        /// <returns>A reference to the specified <paramref name="request"/>.</returns>
        public static T ValidateRequestSize<TMarshaller, T>(this T request, TMarshaller marshaller = null)
            where T : AmazonSQSRequest
            where TMarshaller : class, IMarshaller<IRequest, AmazonWebServiceRequest>
        {
            Validator.ThrowIfNull(request, nameof(request));
            var marshalledSize = request.Marshall<TMarshaller>()?.GetApproximateMessageSize() ?? 0;
            Validator.ThrowIfGreaterThan(marshalledSize, AmazonSqsManager.MaximumRequestSize, nameof(request), "Request cannot exceed a size of {0} bytes. Actual size was {1}. Try to reduce the message size -or- check the number of attributes specified.".FormatWith(AmazonSqsManager.MaximumRequestSize, marshalledSize));
            return request;
        }
    }
}