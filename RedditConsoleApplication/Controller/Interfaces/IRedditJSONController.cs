namespace RedditConsoleApplication.Controller.Interfaces;
/// <summary>
/// This Interface can be used to implement the response JSON object.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRedditJSONController<T>
{
    /// <summary>
    /// This method is invoked with the response of the JSON Object parameter 
    /// </summary>
    /// <param name="responseJSONObject"></param>
    public void OnHandleResponseJSONObject(T? responseJSONObject);
}
