using System;
using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.Util;

namespace WBPA.Amazon.SimpleQueueService
{
    /// <summary>
    /// Specifies options that is related to <see cref="Manager"/> operations.
    /// </summary>
    public class ManagerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ManagerOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Timeout"/></term>
        ///         <description>30 seconds.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseHttp"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="LogResponse"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="BufferSize"/></term>
        ///         <description><see cref="AWSSDKUtils.DefaultBufferSize"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SignatureMethod"/></term>
        ///         <description><see cref="SigningAlgorithm.HmacSHA256"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MaxErrorRetry"/></term>
        ///         <description><c>4</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ProgressUpdateInterval"/></term>
        ///         <description><see cref="AWSSDKUtils.DefaultProgressUpdateInterval"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ResignRetries"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="LogMetrics"/></term>
        ///         <description><see cref="LoggingConfig.LogMetrics"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="DisableLogging"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AllowAutoRedirect"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseDualStackEndpoint"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ThrottleRetries"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MaxConnectionsPerServer"/></term>
        ///         <description><c>64</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ProxyHost"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ProxyPort"/></term>
        ///         <description><c>0</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ProxyCredentials"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AuthenticationServiceName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AuthenticationRegion"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ManagerOptions()
        {
            Timeout = TimeSpan.FromSeconds(30);
            UseHttp = false;
            LogResponse = false;
            BufferSize = AWSSDKUtils.DefaultBufferSize;
            SignatureMethod = SigningAlgorithm.HmacSHA256;
            MaxErrorRetry = 4;
            ProgressUpdateInterval = AWSSDKUtils.DefaultProgressUpdateInterval;
            ResignRetries = false;
            LogMetrics = AWSConfigs.LoggingConfig.LogMetrics;
            DisableLogging = false;
            AllowAutoRedirect = true;
            UseDualStackEndpoint = false;
            ThrottleRetries = true;
            MaxConnectionsPerServer = 64;
        }

        internal RegionEndpoint RegionEndpoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the service response should be logged.
        /// </summary>
        /// <value><c>true</c> to have the service response logged; otherwise, <c>false</c>.</value>
        public bool LogResponse { get; set; }

        /// <summary>
        /// Gets or sets the size of the request buffer. Default is <see cref="AWSSDKUtils.DefaultBufferSize"/>.
        /// </summary>
        /// <value>The size of the request buffer.</value>
        public int BufferSize { get; set; }

        /// <summary>
        /// Gets or sets the interval at which progress update events are raised for upload operations. Default is <see cref="AWSSDKUtils.DefaultProgressUpdateInterval"/>.
        /// </summary>
        /// <value>The interval at which progress update events are raised for upload operations.</value>
        public long ProgressUpdateInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to resign requests on retry or not.
        /// </summary>
        /// <value><c>true</c> to resign requests on retry; otherwise, <c>false</c>.</value>
        public bool ResignRetries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request should follow redirection responses.
        /// </summary>
        /// <value><c>true</c> if the request should automatically follow redirection responses from the Internet resource; otherwise, <c>false</c>.</value>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to log metrics for service calls.
        /// </summary>
        /// <value><c>true</c> if metrics for service calls should be logged; otherwise, <c>false</c>.</value>
        public bool LogMetrics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether logging for this client should be enabled.
        /// </summary>
        /// <value><c>true</c> if logging for this client must be disabled; otherwise, <c>false</c>.</value>
        public bool DisableLogging { get; set; }

        /// <summary>
        /// Gets or sets the timeout of web requests. Default is 30 seconds.
        /// </summary>
        /// <value>The timeout of web requests.</value>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a service should use dual stack (ipv6 enabled) endpoint for the configured region.
        /// </summary>
        /// <value><c>true</c> if a service should use dual stack endpoint for the configured region; otherwise, <c>false</c>.</value>
        public bool UseDualStackEndpoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the retry throttling feature should be enabled.
        /// </summary>
        /// <value><c>true</c> if the retry throttling feature should be enabled; otherwise, <c>false</c>.</value>
        public bool ThrottleRetries { get; set; }

        /// <summary>
        /// Gets or sets the credentials to use with a proxy.
        /// </summary>
        /// <value>The proxy credentials to use with a proxy.</value>
        public ICredentials ProxyCredentials { get; set; }

        /// <summary>
        /// Gets or sets the host of a proxy.
        /// </summary>
        /// <value>The host of a proxy.</value>
        public string ProxyHost { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed numbers of transient faults.
        /// </summary>
        /// <value>The maximum allowed numbers of transient faults.</value>
        public int MaxErrorRetry { get; set; }

        /// <summary>
        /// Gets or sets the short-form name of the authentication service being called.
        /// </summary>
        /// <value>The short-form name of the authentication service.</value>
        public string AuthenticationServiceName { get; set; }

        /// <summary>
        /// Gets or sets the region of the authentication service being called.
        /// </summary>
        /// <value>The region of the authentication service being called.</value>
        public string AuthenticationRegion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client should attempt to use HTTP protocol.
        /// </summary>
        /// <value><c>true</c> if the client should attempt to use HTTP protocol; otherwise, <c>false</c>.</value>
        public bool UseHttp { get; set; }

        /// <summary>
        /// Gets or sets the port number of a proxy.
        /// </summary>
        /// <value>The port number of a proxy.</value>
        public int ProxyPort { get; set; }

        /// <summary>
        /// Gets or sets the signature method of request signing.
        /// </summary>
        /// <value>The signature method of request signing.</value>
        public SigningAlgorithm SignatureMethod { get; set; }

        /// <summary>
        /// Gets or sets the maximum connections per server.
        /// </summary>
        /// <value>The maximum connections per server.</value>
        public int MaxConnectionsPerServer { get; set; }
    }
}