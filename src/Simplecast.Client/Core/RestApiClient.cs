﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Simplecast.Client.Contracts;
using Simplecast.Client.Core.Extensions;
using Simplecast.Client.Core.Responses;

namespace Simplecast.Client.Core
{
    public class RestApiClient : IRestApiClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public RestApiClient(HttpClient httpClient, ApiOptions apiOptions)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(apiOptions.BaseUrl);
            _apiKey = apiOptions.ApiKey;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver {NamingStrategy = new SnakeCaseNamingStrategy()}
            };
        }

        public async Task<ApiResponse<TModel>> GetApiResponseAsync<TModel>(string path, 
                                                                           IList<KeyValuePair<string, string>> queryParams = null, 
                                                                           IDictionary<string, string> headerParams = null) 
            where TModel : class, new()
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            using (HttpResponseMessage httpResponseMessage = await CallAsync(HttpMethod.Get, path, queryParams, headerParams))
            {
                string stringContent = await httpResponseMessage.Content.ReadAsStringAsync();

                var apiResponse = new ApiResponse<TModel>
                {
                    HttpStatusCode = httpResponseMessage.StatusCode,
                    Headers = httpResponseMessage.Headers.ToDictionary(pair => pair.Key, pair => pair.Value.First()),
                    UrlPath = path
                };
                    
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    bool converted = stringContent.TryDeserializeObject(out ErrorResponse errorResponse, _jsonSerializerSettings);
                    apiResponse.Error = true;
                    apiResponse.Message = converted ? errorResponse.Error : stringContent;

                    return apiResponse;
                }

                var model = JsonConvert.DeserializeObject<TModel>(stringContent, _jsonSerializerSettings);
                apiResponse.Model = model;

                return apiResponse;
            }
        }

        public async Task<TModel> GetAsync<TModel>(string path, 
                                                   IList<KeyValuePair<string, string>> queryParams = null,
                                                   IDictionary<string, string> headerParams = null)
            where TModel : class, new()
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            string stringContent = await GetStringContentAsync(path, queryParams, headerParams);

            return JsonConvert.DeserializeObject<TModel>(stringContent, _jsonSerializerSettings);
        }

        public async Task<string> GetStringContentAsync(string path,
                                                        IList<KeyValuePair<string, string>> queryParams = null,
                                                        IDictionary<string, string> headerParams = null)
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            using (HttpResponseMessage httpResponseMessage = await CallAsync(HttpMethod.Get, path, queryParams, headerParams))
            {
                string stringContent = await httpResponseMessage.Content.ReadAsStringAsync();

                return stringContent;
            }
        }

        public async Task<HttpResponseMessage> CallAsync(HttpMethod httpMethod, string path,
                                                         IList<KeyValuePair<string, string>> queryParams = null,
                                                         IDictionary<string, string> headerParams = null)
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            using (HttpRequestMessage httpRequestMessage = PrepareRequestMessage(path, httpMethod, queryParams, headerParams))
            {
                HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequestMessage);

                return httpResponse;
            }
        }

        public HttpRequestMessage PrepareRequestMessage(string path, 
                                                        HttpMethod httpMethod,
                                                        IList<KeyValuePair<string, string>> queryParams = null,
                                                        IDictionary<string, string> headerParams = null)
        {
            Ensure.ArgumentNotNullOrEmptyString(path, nameof(path));

            if (queryParams == null)
            {
                queryParams = new List<KeyValuePair<string, string>>();
            }

            queryParams.Add(new KeyValuePair<string, string>("api_key", _apiKey));

            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

            foreach (KeyValuePair<string, string> queryParam in queryParams)
            {
                query[queryParam.Key] = queryParam.Value;
            }

            path = $"{path}?{query}";

            var requestMessage = new HttpRequestMessage(httpMethod, path);

            if (headerParams == null || !headerParams.Any())
            {
                return requestMessage;
            }

            foreach (KeyValuePair<string, string> headerParam in headerParams)
            {
                requestMessage.Headers.Add(headerParam.Key, headerParam.Value);
            }

            return requestMessage;
        }
    }
}