using RedditConsoleApplication.Controller.Models;

namespace RedditConsoleApplication;

public static class ConsoleController
{
    public static void ReportListOfPostsWithMostUpVotesToConsole(SortedList<int, IRedditStatsModel> sortedListOfUps)
    {
        Console.WriteLine("\n**************** List top 5 of posts with most up votes in descending order.. ****************\n");
        foreach (var item in sortedListOfUps.Take(5).ToList())
        {
            Console.WriteLine("______________________________");
            Console.WriteLine($" Title : {item.Value.title}");
            Console.WriteLine($" Author : {item.Value.author} | # Votes : {item.Value.ups.Value} | Vote Ratio : {item.Value.upvote_ratio.Value} | # Comments : {item.Value.num_comments.Value} | Created : {item.Value.created.ToString("MM/dd/yy HH:mm")}");
            Console.WriteLine("______________________________\n");
        }
    }
    public static void ReportListOfUsersWithMostPostToConsole(Dictionary<string, int> unsortedDicOfAuthors){
        //Sorting the authors in descending order to write out in the console
        Console.WriteLine("\n**************** List top 5 of users with most posts in descending order..\n");
        foreach (var item in unsortedDicOfAuthors.OrderByDescending(o => o.Value).Take(5).ToList())
        {
            Console.WriteLine("______________________________");
            Console.WriteLine($" User : {item.Key}");
            Console.WriteLine($" Posts : {item.Value}");
            Console.WriteLine("______________________________\n");
        }
    }
}
