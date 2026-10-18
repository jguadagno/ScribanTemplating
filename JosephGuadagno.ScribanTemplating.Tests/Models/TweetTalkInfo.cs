namespace JosephGuadagno.ScribanTemplating.Tests.Models;

public class TweetTalkInfo
{
    /// <summary>
    /// The name of the talk
    /// </summary>
    public string Name { get; set; } = string.Empty;
        
    /// <summary>
    /// A Url for the talk on the conference website
    /// </summary>
    public string UrlForConferenceTalk { get; set; } = string.Empty;
        
    /// <summary>
    /// The Url for the talk on the talk deliverers website
    /// </summary>
    public string UrlForTalk { get; set; } = string.Empty;
        
    /// <summary>
    /// The start date and time of the talk
    /// </summary>
    public DateTimeOffset StartDateTime { get; set; }
        
    /// <summary>
    /// The end date and time of the talk
    /// </summary>
    public DateTimeOffset EndDateTime { get; set; }
        
    /// <summary>
    /// The room/channel/url for the talk
    /// </summary>
    public string TalkLocation { get; set; } = string.Empty;
        
    /// <summary>
    /// Comments for the talk
    /// </summary>
    public string Comments { get; set; } = string.Empty;
    
    /// <summary>
    /// The engagement this talk is associated with
    /// </summary>
    public TweetEngagementInfo EngagementInfo { get; set; } = new();
}