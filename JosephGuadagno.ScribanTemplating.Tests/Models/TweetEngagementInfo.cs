namespace JosephGuadagno.ScribanTemplating.Tests.Models;

/// <summary>
/// Contains the information that is needed to tweet about an engagement
/// </summary>
public class TweetEngagementInfo
{
    /// <summary>
    /// The name of the engagement
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The Url for the engagement
    /// </summary>
    public string Url { get; set; } = string.Empty;
        
    /// <summary>
    /// The date and time the engagement starts
    /// </summary>
    public DateTimeOffset StartDateTime { get; set; }

    /// <summary>
    /// The date and time the engagement ends
    /// </summary>
    public DateTimeOffset EndDateTime { get; set; }

    /// <summary>
    /// The IANA Time Zone Identifier for the engagement
    /// </summary>
    public string TimeZoneId { get; set; } = string.Empty;

    /// <summary>
    /// Comments for the engagement
    /// </summary>
    /// <remarks>Could be a discount code for the engagement</remarks>
    public string Comments { get; set; } = string.Empty;
}