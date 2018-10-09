using System.Collections.Generic;
using Simplecast.Client.Contracts;

namespace Simplecast.Client.Core.Helpers
{
    public static class FilterExtensions
    {
        public static IList<KeyValuePair<string, string>> ToQueryParams<TFilterModel>(this TFilterModel filter)
            where TFilterModel : class, IFilter
        {
            var propertyQueryStringBuilder = new PropertyQueryStringBuilder();

            var queryParameters = new List<KeyValuePair<string, string>>();

            propertyQueryStringBuilder.ProcessRequest(queryParameters, filter);

            return queryParameters;
        }
    }
}
