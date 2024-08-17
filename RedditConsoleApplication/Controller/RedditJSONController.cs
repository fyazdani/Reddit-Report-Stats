using RedditConsoleApplication.Controller.Interfaces;
using RedditConsoleApplication.Controller.Models;
using RedditConsoleApplication.Models;

namespace RedditConsoleApplication.Controller;

public class RedditJSONController : IRedditJSONController<Root>
{
    private static SortedList<int, IRedditStatsModel>? _sortedListOfUps;
    public static SortedList<int, IRedditStatsModel> SortedListOfUps
    {
        get
        {
            if (_sortedListOfUps == null)
                _sortedListOfUps = new SortedList<int, IRedditStatsModel>(new DuplicateKeyComparer<int>());
            return _sortedListOfUps;
        }
    }

    private static Dictionary<string, int>? _unsortedDicOfAuthors;
    public static Dictionary<string, int> UnsortedDicOfAuthors
    {
        get
        {
            if (_unsortedDicOfAuthors == null)
                _unsortedDicOfAuthors = new Dictionary<string, int>();
            return _unsortedDicOfAuthors;
        }
    }
    private static readonly object dataLockObj = new object();
    private static bool _dataAvailable;
    public static bool DataAvailable
    {
        get { return _dataAvailable; }
        set
        {
            lock (dataLockObj)
            {
                _dataAvailable = value;
            }
        }
    }

    private static readonly object authorLockObj = new object();
    private static readonly object sortedLockObj = new object();

    public void OnHandleResponseJSONObject(Root? responseJSONObject)
    {
        int sortedListOfUpsCount = SortedListOfUps.Count;
        int unsortedDicOfAuthors = UnsortedDicOfAuthors.Count;
        if (responseJSONObject?.data?.children?.Count > 0)
        {
            foreach (var item in responseJSONObject.data.children)
            {
                if (item.data != null)
                {
                    if (item.data.ups.HasValue)
                    {
                        RedditStatsModel redRedditStatsModel;
                        lock (sortedLockObj)
                        {
                            SortedListOfUps.Add(item.data.ups.Value, redRedditStatsModel = new RedditStatsModel()
                            {
                                title = item.data?.title,
                                author = item.data?.author,
                                ups = item.data?.ups,
                                upvote_ratio = item.data?.upvote_ratio,
                                num_comments = item.data.num_comments
                            });
                        };
                        if ((bool)item.data?.created.HasValue)
                            redRedditStatsModel.created = DateTime.UnixEpoch.AddSeconds((double)item.data?.created.Value);
                    }
                    if (item.data.author != null)
                    {
                        lock (authorLockObj)
                        {
                            if (UnsortedDicOfAuthors.ContainsKey(item.data?.author))
                                UnsortedDicOfAuthors[item.data.author] = UnsortedDicOfAuthors[item.data.author] + 1;
                            else
                                UnsortedDicOfAuthors.Add(item.data.author, 1);
                        }
                    }
                }
            }
        }
        //new dataAvailable if data changed
        if (sortedListOfUpsCount < SortedListOfUps.Count || unsortedDicOfAuthors < UnsortedDicOfAuthors.Count)
        {
            DataAvailable = true;
        }
    }
}
