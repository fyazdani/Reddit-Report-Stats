namespace RedditConsoleApplication.Controller;

public static class RedditRequestParams
{
    public static readonly string BASEADDRESS = "https://oauth.reddit.com/";
    public static readonly string SUBREDDIT_PROGRAMMING = "/r/programming";
    public static readonly string SUBREDDIT_TECHNOLOGY = "/r/technology";
    public static readonly string QUERY_AFTER = "after";
    public static readonly string HEADER_X_RATELIMIT_USED = "x-ratelimit-used";
    public static readonly string HEADER_X_RATELIMIT_REMAINING = "x-ratelimit-remaining";
    public static readonly string HEADER_X_RATELIMIT_RESET = "x-ratelimit-reset";
    public static readonly string HEADER_USER_AGENT = "User-Agent";
    public static readonly string HEADER_AUTHORIZATION = "Authorization";
}
