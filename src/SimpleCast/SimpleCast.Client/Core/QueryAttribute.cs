using System;

namespace Simplecast.Client.Core
{
    public class QueryAttribute : Attribute
    {
        public QueryAttribute(string queryStringKey)
        {
            QueryStringKey = queryStringKey;
        }

        public string QueryStringKey { get; }
    }
}
