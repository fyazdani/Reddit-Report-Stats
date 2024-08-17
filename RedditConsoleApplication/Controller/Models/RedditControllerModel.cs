using RedditConsoleApplication.Controller.Interfaces;
using RedditConsoleApplication.Models;

namespace RedditConsoleApplication.Controller.Models;

public class RedditControllerModel : IRedditControllerModel<Root>
{
    public IRedditJSONController<Root> RedditJSONController { get; set; }

    public IRedditClient<Root> RClient { get; set; }
}
