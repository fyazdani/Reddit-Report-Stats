using RestSharp;

namespace RedditConsoleApplication.Controller.Interfaces;
/// <summary>
/// The base rest client interface.
/// </summary>
/// <typeparam name="T">JSON object model</typeparam>
public interface IRedditClient<T>
{
    /// <summary>
    /// Authentication Token Value
    /// </summary>
    public string ApiAuthTokenValue { get; }
    /// <summary>
    /// Http User Agent Value
    /// </summary>
    public string UserAgentValue { get; }
    /// <summary>
    /// Subreddit Next Page Value
    /// </summary>
    public string AfterParam { get; }
    /// <summary>
    ///  Approximate number of requests used in this period
    /// </summary>
    public static double X_RateLimit_Used { get; private set; }
    /// <summary>
    /// Approximate number of requests left to use
    /// </summary>
    public static double X_RateLimit_Remaining { get; private set; }
    /// <summary>
    /// Approximate number of seconds to end of period
    /// </summary>
    public static double X_RateLimit_Reset { get; private set; }
    /// <summary>
    /// Creating a Delegate method with parameter of the JSON object, which is received from the response when called rest service
    /// </summary>
    /// <param name="responseJSONObject"></param>
    public delegate void HandleResponseJSONObject(T? responseJSONObject);
    /// <summary>
    /// Invokes when the rest request was successfull by checking the rest response IsSuccessful
    /// </summary>
    public event HandleResponseJSONObject? ResponseJSONObjectEvent;
    public IRestClient restClient { get; }
    public RestRequest? RestRequest { get; set; }
    public void ConfigureRestCLientOptions(RestClientOptions options);
    public Task ProcessRequest();
    public void Dispose();
}
