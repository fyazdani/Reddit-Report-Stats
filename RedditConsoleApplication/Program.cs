
using System.Timers;
using Microsoft.Extensions.Configuration;
using RedditConsoleApplication.Controller;
using RedditConsoleApplication.Controller.Interfaces;
using RedditConsoleApplication.Models;

namespace RedditConsoleApplication;
internal class Program
{
    private static string _apiAuthToken = "";
    private static string _userAgent = "";
    private static IRedditController<Root>? redditController = null;
    private static System.Timers.Timer _timer;
    private static void Main(string[] args)
    {
        try
        {
            //Get Configuration and create an object with Inerface for configuration
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            // .AddUserSecrets<Program>(); // You can add sectrets token int his way import nuget package

            IConfiguration config = builder.Build();
            _apiAuthToken = config.GetValue<string>("AppConfiguration:UserAccount:ApiAuthToken");
            _userAgent = config.GetValue<string>("AppConfiguration:UserAccount:UserAgent");

            redditController = new RedditController(_apiAuthToken, _userAgent, RedditRequestParams.BASEADDRESS + RedditRequestParams.SUBREDDIT_PROGRAMMING);
            var task = Task.Factory.StartNew(async () =>
            {
                    await redditController.GetPosts();
            });
            //Timer to report stats to console every X seconds
            _timer = new System.Timers.Timer(15000);
            _timer.Elapsed += ReportStatsToConsoles;
            _timer.AutoReset = true;
            _timer.Start();

            Console.WriteLine("Reddit Console Application is running. Waiting for stats...");
            Console.ReadLine();
            //Stoping the timer
            _timer.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error MSG: {ex.Message} Error Trace: {ex.StackTrace} ");
        }
        finally
        {
            //Dispose
            redditController.Dispose();
            //Disposing the timer
            _timer.Dispose();
        }
    }

    private static void ReportStatsToConsoles(object? sender, ElapsedEventArgs e)
    {
        try
        {
            if (RedditJSONController.DataAvailable)
            {
                ConsoleController.ReportListOfPostsWithMostUpVotesToConsole(RedditJSONController.SortedListOfUps);
                ConsoleController.ReportListOfUsersWithMostPostToConsole(RedditJSONController.UnsortedDicOfAuthors);
                RedditJSONController.DataAvailable = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error MSG: {ex.Message} Error Trace: {ex.StackTrace} ");
        }
    }
}