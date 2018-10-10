using System;
using System.Net;

namespace Simplecast.Client.Core.Exceptions
{
    public class UnsuccessfulResponseException : Exception
    {
        public UnsuccessfulResponseException(string message, string urlPath, HttpStatusCode code) 
            : base(message)
        {
            UrlPath = urlPath;
            Code = code;
        }

        public string UrlPath { get; }

        public HttpStatusCode Code { get; }
    }
}