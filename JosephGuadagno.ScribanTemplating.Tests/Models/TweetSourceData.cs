namespace JosephGuadagno.ScribanTemplating.Tests.Models;

public class TweetSourceData
{
    public bool IsNew { get; set; } = true;
    public bool WasScheduled { get; set; } = false;
    public string Source { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<string> HashTags { get; set; } = new();
}