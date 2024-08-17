namespace RedditConsoleApplication.Controller.Interfaces;

public interface IRedditControllerModel<T>
{
    public IRedditJSONController<T> RedditJSONController { get; set;}
    public IRedditClient<T> RClient { get; set;}
}
