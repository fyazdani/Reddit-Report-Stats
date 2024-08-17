namespace RedditConsoleApplication.Controller.Models;

public class RedditStatsModel : IRedditStatsModel
{
    public string? author { get; set; }
    public string? title { get; set; }
    public int? ups { get; set; }
    public int? posts { get; set; }
    public double? upvote_ratio { get; set; }
    public DateTime created { get; set; }
    public int? num_comments { get; set; }
}