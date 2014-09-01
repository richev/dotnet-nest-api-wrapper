using System.IO;
using System.Net;
using log4net;
using Newtonsoft.Json;
using Richev.Nest.ApiWrapper.Models;

namespace Richev.Nest.ApiWrapper.Net
{
    public class NetGetter : INetGetter
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(NetGetter));

        public string GetResponseText(WebRequest request, out string subsequentRequestsUrl)
        {
            // Requests to the Nest API result in a 307 redirect. The Nest documentation states that subsequent
            // requests should go to this redirected URL, so we capture this and return it in the out
            // variable for this method.
            //
            // For more information see the relevant section in https://developer.nest.com/documentation/data-rate-limits.

            var responseText = string.Empty;

            PerformanceCounter counter = null;

            if (_log.IsInfoEnabled)
            {
                counter = new PerformanceCounter();
                counter.Start();
            }

            try
            {
                var response = GetResponseWithBetterErrors(request);

                // The underlying call to GetResponse will have handled any 307 redirect, and the URL we were redirected to
                // (which must be used for subsequent requests) will be in the ResponseUri.AbsoluteUri.
                subsequentRequestsUrl = response.ResponseUri.AbsoluteUri;

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseText = reader.ReadToEnd();
                        }
                    }
                    else if (_log.IsWarnEnabled)
                    {
                        _log.WarnFormat("Null response received from '{0}'.", request.RequestUri.AbsoluteUri);
                    }
                }
            }
            finally
            {
                if (_log.IsInfoEnabled && counter != null)
                {
                    counter.Stop();
                    _log.InfoFormat("Received response in {0:0.00} seconds from {1} '{2}'.", counter.Duration.TotalSeconds, request.Method, request.RequestUri);
                }
            }

            return responseText;
        }

        /// <summary>
        /// Calls the GetResponse method, but in the event of a WebException throws a new exception that contains the error message returned from the Nest API.
        /// </summary>
        private static WebResponse GetResponseWithBetterErrors(WebRequest request)
        {
            WebResponse response;

            try
            {
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                using (var exceptionResponse = (HttpWebResponse)ex.Response)
                {
                    if (exceptionResponse == null)
                    {
                        throw;
                    }

                    using (var responseStream = exceptionResponse.GetResponseStream())
                    using (var reader = new StreamReader(responseStream))
                    {
                        var errorJson = reader.ReadToEnd();

                        if (string.IsNullOrEmpty(errorJson))
                        {
                            throw;
                        }

                        try
                        {
                            var errorObj = JsonConvert.DeserializeObject<NestErrorResponseModel>(errorJson);
                            throw new WebException(string.Format("Nest said '{0}'", errorObj.Message), ex);
                        }
                        catch (JsonSerializationException)
                        {
                            throw new WebException(errorJson, ex);
                        }
                    }
                }
            }

            return response;
        }
    }
}