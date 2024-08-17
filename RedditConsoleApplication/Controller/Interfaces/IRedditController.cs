namespace RedditConsoleApplication.Controller.Interfaces;

/// <summary>
/// T is the model of the serialized response JSON object.
/// </summary>
public interface IRedditController<T>
{
       public IRedditControllerModel<T> RedditCtrlModel { get; }

       public Task GetPosts();

       public void Dispose();
}
