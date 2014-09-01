using System.Net;

namespace Richev.Nest.ApiWrapper.Net
{
    public interface INetGetter
    {
        /// <summary>
        /// Gets the text returned in the HTTP Response from the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="subsequentRequestsUrl">
        /// <para>The URL to be used for subsequent requests to the URL specified in the request.</para>
        /// <para>This is communicated to us via a 307 redirect from the Nest API. For more information see the appropriate section in https://developer.nest.com/documentation/data-rate-limits. </para>
        /// </param>
        string GetResponseText(WebRequest request, out string subsequentRequestsUrl);
    }
}
