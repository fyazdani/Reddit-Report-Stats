using RedditConsoleApplication.Controller.Interfaces;
using RedditConsoleApplication.Controller.Models;
using RedditConsoleApplication.Models;

namespace RedditConsoleApplication.Controller;

public class RedditController : IRedditController<Root>, IDisposable
{
    public IRedditControllerModel<Root> RedditCtrlModel { get; private set; } = new RedditControllerModel();

    public RedditController(string apiAuthToken, string userAgent, string baseUrl)
    {
        Uri uriResult;
        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            throw new NullReferenceException("baseUrl parameter is not valid URL");

        RedditCtrlModel.RClient = new RedditClient(apiAuthToken, userAgent, baseUrl);
        RedditCtrlModel.RedditJSONController = new RedditJSONController();
        //Subscribe to event for processing of the JSON object
        RedditCtrlModel.RClient.ResponseJSONObjectEvent += RedditCtrlModel.RedditJSONController.OnHandleResponseJSONObject;
    }

    public async Task GetPosts()
    {
        try
        {
            do
            {
                //process the request
                await RedditCtrlModel.RClient.ProcessRequest();

                // RateLimit_remaining is 0 lets wait to reset the rate limit by sleeping the thread for the seconds amount specified in X_RateLimit_Rest header of response
                if (RedditClient.X_RateLimit_Remaining == 0)
                    Thread.Sleep((int)RedditClient.X_RateLimit_Reset * 1000);

            } while ((!string.IsNullOrEmpty(RedditCtrlModel.RClient.AfterParam) && RedditClient.X_RateLimit_Remaining > 0));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error MSG: {ex.Message} Error Trace: {ex.StackTrace} ");
        }
    }


    public void Dispose()
    {
        //Unsubscribe to events
        RedditCtrlModel.RClient.ResponseJSONObjectEvent -= RedditCtrlModel.RedditJSONController.OnHandleResponseJSONObject;
        //when done dispose the RedditClient
        RedditCtrlModel.RClient.Dispose();
        GC.SuppressFinalize(this);
    }
}