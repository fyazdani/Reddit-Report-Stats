
using RedditConsoleApplication.Controller.Interfaces;
using RedditConsoleApplication.Models;
using RestSharp;

namespace RedditConsoleApplication.Controller;

public class RedditClient : IRedditClient<Root>, IDisposable
{
    public string ApiAuthTokenValue { get; private set; }
    public string UserAgentValue { get; private set; }
    /// <summary>
    /// Subreddit Next Page Value
    /// </summary>
    public string AfterParam { get; private set; }
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
    /// Creating a Delegate method with parameter of the object received in response which is being used when the Handle
    /// </summary>
    /// <param name="responseJSONObject"></param>
    public delegate void HandleResponseJSONObject(Root? responseJSONObject);
    /// <summary>
    /// Invokes when the rest request was successfull by checking the rest response IsSuccessful
    /// </summary>
    public event IRedditClient<Root>.HandleResponseJSONObject? ResponseJSONObjectEvent;
    public IRestClient restClient { get; private set;}
    public RestRequest? RestRequest { get; set; }

    public RedditClient(string apiAuthToken, string userAgent, string BaseUrl)
    {
        //Validate the headers
        this.ApiAuthTokenValue = $"bearer {apiAuthToken}";
        this.UserAgentValue = userAgent;
        this.RestRequest = new RestRequest(BaseUrl);
        this.RestRequest.AddHeader(RedditRequestParams.HEADER_USER_AGENT, this.UserAgentValue);
        this.RestRequest.AddHeader(RedditRequestParams.HEADER_AUTHORIZATION, this.ApiAuthTokenValue);
        //setting UseClientFactory to true to make single instance of the HttpClient as it will be used mutiple times to request
        this.restClient = new RestClient(ConfigureRestCLientOptions, useClientFactory: true);
    }

    public void ConfigureRestCLientOptions(RestClientOptions options)
    {
        options.BaseUrl = new Uri(RedditRequestParams.BASEADDRESS);
        options.UserAgent = this.UserAgentValue;
    }

    public async Task ProcessRequest()
    {
        try
        {
            if (!string.IsNullOrEmpty(this.AfterParam))
            {
                Parameter param = Parameter.CreateParameter(RedditRequestParams.QUERY_AFTER, this.AfterParam, ParameterType.QueryString);
                if (!this.RestRequest.Parameters.Contains(param))
                    this.RestRequest.AddParameter(param);
                else
                    this.RestRequest.AddOrUpdateParameter(param);
            }
            var restResp = await this.restClient.ExecuteGetAsync<Root>(RestRequest);
            if (restResp.IsSuccessful)
            {
                _ = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        //Invoke the delegate event for JSON object processing
                        this.ResponseJSONObjectEvent?.Invoke(restResp.Data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error MSG: {ex.Message} Error Trace: {ex.StackTrace} ");
                    }
                });
            }
            else
            {
                Console.WriteLine("Error MSG " + restResp.ErrorMessage);
                Console.WriteLine("Error Trace " + restResp.ErrorException);
            }

            // Getting header limits
            if (restResp.Headers?.Count > 0)
            {
                double tmp;
                if (double.TryParse(restResp.GetHeaderValue(RedditRequestParams.HEADER_X_RATELIMIT_USED), out tmp))
                    X_RateLimit_Used = tmp;
                if (double.TryParse(restResp.GetHeaderValue(RedditRequestParams.HEADER_X_RATELIMIT_REMAINING), out tmp))
                    X_RateLimit_Remaining = tmp;
                if (double.TryParse(restResp.GetHeaderValue(RedditRequestParams.HEADER_X_RATELIMIT_RESET), out tmp))
                    X_RateLimit_Reset = tmp;
            }
            // Getting next query params
            if (!string.IsNullOrWhiteSpace(restResp.Data?.data?.after))
                this.AfterParam = restResp.Data?.data?.after;
            else
                this.AfterParam = "";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error MSG: {ex.Message} Error Trace: {ex.StackTrace} ");
        }
    }

    public void Dispose()
    {
        this.restClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
