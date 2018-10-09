using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core.Helpers;

namespace Simplecast.Client.Core
{
    public abstract class QueryStringBuilder
    {
        protected QueryStringBuilder Successor;

        public void SetSuccessor(QueryStringBuilder successor)
        {
            Ensure.ArgumentNotNull(successor, nameof(successor));

            Successor = successor;
        }

        public abstract void ProcessRequest<TFilterModel>(IList<KeyValuePair<string, string>> queryStringParams,
            TFilterModel filter) where TFilterModel : class, IFilter;
    }

    public class PropertyQueryStringBuilder : QueryStringBuilder
    {
        public override void ProcessRequest<TFilterModel>(IList<KeyValuePair<string, string>> queryStringParams, TFilterModel filter)
        {
            Ensure.ArgumentNotNull(filter, nameof(filter));

            var propertyInfos = filter.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(info => Attribute.IsDefined(info, typeof(QueryAttribute))).ToList();

            if (!propertyInfos.Any())
            {
                Successor?.ProcessRequest(queryStringParams, filter);
                return;
            }

            queryStringParams = queryStringParams ?? new List<KeyValuePair<string, string>>();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object value = propertyInfo.GetValue(filter);
                if (value == null)
                {
                    continue;
                }

                var customAttribute = propertyInfo.GetCustomAttribute<QueryAttribute>();
                var queryStringKey = customAttribute.QueryStringKey;

                queryStringParams.Add(new KeyValuePair<string, string>(queryStringKey, value.ToString()));
            }

            Successor?.ProcessRequest(queryStringParams, filter);
        }
    }
}
