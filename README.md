# Simplecast Client Library .NET

Simplecast .NET Client Library is a client library targeting .NET Standard 2.0 and .NET 4.6.1 that provides an easy way to interact with the Simplecast API (https://api.simplecast.com/)

All requests must be authenticated using your API key, available in your Simplecast account settings. You have the option of authenticating one of three different ways: using HTTP Basic Auth, passing it as a request parameter, or setting an HTTP header. Your API key is available in your [Simplecast account settings](https://api.simplecast.com/user/edit).

## Supported Platforms

* .NET 4.6.1 (Desktop / Server)
* [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

## Features
* Dependency injection friendly (can also be used standalone, see below)
* Supports async and sync (via extension method, see below) calls.

## Continuous integration


## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
    - [Standalone Initialization](#standalone-initialization)
    - [Microsoft.Extensions.DependencyInjection Initialization](#microsoftextensionsdependencyinjection-initialization)
    - [Call Endpoints](#call-endpoints)
    - [Synchronous Wrapper](#synchronous-wrapper)
3. [License](#license)

## Installation

Following commands can be used to install Simplecast.Client, run the following command in the Package Manager Console

```
Install-Package Simplecast.Client
```

Or use `dotnet cli`

```
dotnet Simplecast.Client
```
## Usage

Simplecast.Client can be used with any DI library, or it can be used standalone.

### Standalone Initialization

If you do not want to use any DI framework, you have to instantiate `SimplecastClientStandalone` as follows.

```csharp
ApiOptions apiOptions = new ApiOptions("<your token>", "https://api.simplecast.com/v1/");
var apiClientContext = SimplecastClientStandalone.Create(apiOptions);
IEpisodeClient episodeClient = apiClientContext.EpisodeClient;
IPlayerEmbedClient playerEmbedClient = apiClientContext.PlayerEmbedClient;
IPodcastClient podcastClient = apiClientContext.PodcastClient;
IStatisticClient statisticClient = apiClientContext.StatisticClient;
```

`apiClientContext` contains all necessary clients.

### Microsoft.Extensions.DependencyInjection Initialization

First, you need to install `Microsoft.Extensions.DependencyInjection` and `Microsoft.Extensions.Http` NuGet package as follows

```
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Http
```

By installing `Microsoft.Extensions.Http` you will be able to use [`HttpClientFactory`](https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore).In the words of the ASP.NET Team it is “an opinionated factory for creating HttpClient instances” and is a new feature comes with the release of ASP.NET Core 2.1. 

If you don't want to use `HttpClientFactory`, you must register `HttpClient` yourself with the container.

Register necessary dependencies to `ServiceCollection` as follows

```csharp
var apiOptions = new ApiOptions("<your token>", "https://api.simplecast.com/v1/");

var services = new ServiceCollection();
services.AddSingleton(apiOptions);
services.AddHttpClient<IRestApiClient, RestApiClient>();
services.AddTransient<IPodcastClient, PodcastClient>();
services.AddTransient<IEpisodeClient, EpisodeClient>();
services.AddTransient<IPlayerEmbedClient, PlayerEmbedClient>();
services.AddTransient<IStatisticClient, StatisticClient>();

ServiceProvider buildServiceProvider = services.BuildServiceProvider();

var podcastClient = buildServiceProvider.GetRequiredService<IPodcastClient>();
var episodeClient = buildServiceProvider.GetRequiredService<IEpisodeClient>();
var playerEmbedClient = buildServiceProvider.GetRequiredService<IPlayerEmbedClient>();
var statisticClient = buildServiceProvider.GetRequiredService<IStatisticClient>();
```

### Calling Endpoints

There are two ways to call an endpoint. The only difference is the return types. The methods that end with ResponseAsync returns `ApiResponse<TModel>` which contains model itself, HTTP status codes, error message and response headers.

```csharp
ApiResponse<List<Episode>> episodesResponse = await episodeClient.GetEpisodesResponseAsync(podcastId);

if(episodesResponse.Error)
{
HttpStatusCode statusCode = episodesResponse.HttpStatusCode;
string errorMessage = episodesResponse.Message;
IDictionary<string, string> headers = episodesResponse.Headers;
string urlPath = episodesResponse.UrlPath;
  // Handle http error
}

List<Episode> episodes = episodesResponse.Model;
```

The methods that end with Async returns model itself without additional HTTP response information. But in the case of HTTP error, you need to handle exceptions.

```csharp
List<Episode> episodes = await episodeClient.GetEpisodesAsync(podcastId);
```

### Synchronous Wrapper

For synchronous calls, Task extension method `RunSync` can be used. 

```csharp
List<Episode> episodes= episodeClient.GetEpisodesAsync(podcastId).RunSync();
```
But there is a possibility that this extension method can't cover all cases. See Stackoverflow [article](https://stackoverflow.com/a/25097498/1577827)

## License
Licensed under MIT, see [LICENSE](LICENSE) for the full text.
