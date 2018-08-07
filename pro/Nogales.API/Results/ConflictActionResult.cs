using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Nogales.API.Results
{
    /// <summary>
    /// Returns when a duplicate entry in database
    /// Reason for HTTP Status Code : 409
    /// The request could not be completed due to a conflict with the current state of the resource. 
    /// This code is only allowed in situations where it is expected that the user might be able to resolve the conflict and resubmit the request. 
    /// The response body SHOULD include enough information for the user to recognize the source of the conflict. 
    /// Ideally, the response entity would include enough information for the user or user agent to fix the problem; 
    /// however, that might not be possible and is not required.
    
    public class ConflictActionResult : IHttpActionResult
    {
        public ConflictActionResult(string message, HttpRequestMessage request)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Message = message;
            Request = request;
        }

        public string Message { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict);

            // Put the message in the response body (text/plain content).
            response.Content = new StringContent(Message);
            response.RequestMessage = Request;

            return response;
        }

    }
}