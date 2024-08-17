namespace RedditConsoleApplication.Models;

    public class Data
    {
        public string? after { get; set; }
        public List<Child>? children { get; set; }
        public int? dist { get; set; }
        public string? author { get; set; }
        public string? title { get; set; }
        public int? ups { get; set; }
        public string? before { get; set; }
        public string? id { get; set; }
        public double? upvote_ratio { get; set; }
        public double? created { get; set; }
        public int? num_comments { get; set; }
    }
