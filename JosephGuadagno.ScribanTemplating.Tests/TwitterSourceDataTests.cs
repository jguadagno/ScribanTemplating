namespace JosephGuadagno.ScribanTemplating.Tests;

public class TwitterSourceDataTests
{
    private readonly string _twitterTemplate = @"{{ 
# define needed functions
func trimHashtag(hashtag)
   ret ""#"" + hashtag | string.remove "" "" | string.remove "".""
end

twitterMaxSize = 280
finalText = """"

# tweetParts is used to construct the final text
tweetParts = {
    prefix: '',
    title: title,
    url: url,
    hashtags: ''
}

# compose the prefix
if was_scheduled
    tweetParts.prefix = 'ICYMI: '
else
    if is_new
        tweetParts.prefix = 'New '
    else
        tweetParts.prefix = 'Updated '
    end
end
case source
when 'SyndicationFeed' 
  tweetParts.prefix = tweetParts.prefix + 'Blog Post: '
when 'YouTube' 
  tweetParts.prefix = tweetParts.prefix + 'YouTube Video: '
else 
  tweetParts.prefix = tweetParts.prefix + 'New Content: '
end

# compose the hash tag list
if (hash_tags | array.size) <= 0
    tweetParts.hashtags = '#dotnet #csharp #dotnetcore'
else
    tweetParts.hashtags = hash_tags | array.join "" "" @trimHashtag
end

# See if we need to overwrite the title due to space limitations
finalTweetLength = tweetParts.prefix + tweetParts.title + "" "" + tweetParts.url + "" "" + tweetParts.hashtags | string.size
if finalTweetLength > twitterMaxSize
    finalText = ""TODO: Too big""
    lengthWithoutTitle = (tweetParts.prefix + tweetParts.url + tweetParts.hashtags | string.size) +2
    tweetParts.title = tweetParts.title | string.truncate (twitterMaxSize - lengthWithoutTitle)
end
finalText = tweetParts.prefix + tweetParts.title + "" "" + tweetParts.url + "" "" + tweetParts.hashtags
 
}}
{{-finalText}}";
    
    // DataSource New - Blog Post
    [Fact]
    public void Render_DataSourceBlogPostNewWithDataLessThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = true,
            WasScheduled = false,
            Source = "SyndicationFeed",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("New Blog Post: This is a test title https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);

    }

    [Fact]
    public void Render_DataSourceBlogPostNewWithDataGreaterThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = true,
            WasScheduled = false,
            Source = "SyndicationFeed",
            Title = "This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was not enough so let me add some more characters",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("New Blog Post: This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was not... https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);
    }
    
    [Fact]
    public void Render_DataSourceBlogPostNewWithNoHashTags_ShouldReturnRenderedTextWithDefaultHashTags()
    {
        // Arrange
        var defaultHashtags = "#dotnet #csharp #dotnetcore";

        var model = new Models.TweetSourceData
        {
            HashTags = new() ,
            IsNew = true,
            WasScheduled = false,
            Source = "SyndicationFeed",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("New Blog Post: This is a test title https://www.josephguadagno.net #dotnet #csharp #dotnetcore", renderedText);
        Assert.EndsWith(defaultHashtags, renderedText);
    }
    
    // DataSource Updated - Blog Post
    [Fact]
    public void Render_DataSourceBlogPostUpdatedWithDataLessThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = false,
            Source = "SyndicationFeed",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("Updated Blog Post: This is a test title https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);

    }

    [Fact]
    public void Render_DataSourceBlogPostUpdatedWithDataGreaterThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = false,
            Source = "SyndicationFeed",
            Title = "This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was not enough so let me add some more characters",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("Updated Blog Post: This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was... https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);
    }
    
    [Fact]
    public void Render_DataSourceBlogPostUpdatedWithNoHashTags_ShouldReturnRenderedTextWithDefaultHashTags()
    {
        // Arrange
        var defaultHashtags = "#dotnet #csharp #dotnetcore";

        var model = new Models.TweetSourceData
        {
            HashTags = new() ,
            IsNew = false,
            WasScheduled = false,
            Source = "SyndicationFeed",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("Updated Blog Post: This is a test title https://www.josephguadagno.net #dotnet #csharp #dotnetcore", renderedText);
        Assert.EndsWith(defaultHashtags, renderedText);
    }
    
    // DataSource New - Video
    [Fact]
    public void Render_DataSourceVideoNewWithDataLessThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = true,
            WasScheduled = false,
            Source = "YouTube",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("New YouTube Video: This is a test title https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);

    }

    [Fact]
    public void Render_DataSourceVideoNewWithDataGreaterThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = true,
            WasScheduled = false,
            Source = "YouTube",
            Title = "This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was not enough so let me add some more characters",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("New YouTube Video: This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was... https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);
    }
    
    [Fact]
    public void Render_DataSourceVideoNewWithNoHashTags_ShouldReturnRenderedTextWithDefaultHashTags()
    {
        // Arrange
        var defaultHashtags = "#dotnet #csharp #dotnetcore";

        var model = new Models.TweetSourceData
        {
            HashTags = new() ,
            IsNew = true,
            WasScheduled = false,
            Source = "YouTube",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("New YouTube Video: This is a test title https://www.josephguadagno.net #dotnet #csharp #dotnetcore", renderedText);
        Assert.EndsWith(defaultHashtags, renderedText);
    }
    
    // DataSource Updated - Video
    [Fact]
    public void Render_DataSourceVideoUpdatedWithDataLessThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = false,
            Source = "YouTube",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("Updated YouTube Video: This is a test title https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);

    }

    [Fact]
    public void Render_DataSourceVideoUpdatedWithDataGreaterThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = false,
            Source = "YouTube",
            Title = "This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was not enough so let me add some more characters",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("Updated YouTube Video: This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this... https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);
    }
    
    [Fact]
    public void Render_DataSourceVideoUpdatedWithNoHashTags_ShouldReturnRenderedTextWithDefaultHashTags()
    {
        // Arrange
        var defaultHashtags = "#dotnet #csharp #dotnetcore";

        var model = new Models.TweetSourceData
        {
            HashTags = new() ,
            IsNew = false,
            WasScheduled = false,
            Source = "YouTube",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("Updated YouTube Video: This is a test title https://www.josephguadagno.net #dotnet #csharp #dotnetcore", renderedText);
        Assert.EndsWith(defaultHashtags, renderedText);
    }
    
    // DataSource Scheduled - Blog Post
    [Fact]
    public void Render_DataSourceBlogPostScheduleWithDataLessThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = true,
            Source = "SyndicationFeed",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("ICYMI: Blog Post: This is a test title https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);

    }

    [Fact]
    public void Render_DataSourceBlogPostScheduledWithDataGreaterThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = true,
            Source = "SyndicationFeed",
            Title = "This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was not enough so let me add some more characters",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("ICYMI: Blog Post: This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was ... https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);
    }
    
    [Fact]
    public void Render_DataSourceBlogPostScheduledWithNoHashTags_ShouldReturnRenderedTextWithDefaultHashTags()
    {
        // Arrange
        var defaultHashtags = "#dotnet #csharp #dotnetcore";

        var model = new Models.TweetSourceData
        {
            HashTags = new() ,
            IsNew = false,
            WasScheduled = true,
            Source = "SyndicationFeed",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("ICYMI: Blog Post: This is a test title https://www.josephguadagno.net #dotnet #csharp #dotnetcore", renderedText);
        Assert.EndsWith(defaultHashtags, renderedText);
    }
    
    // DataSource Scheduled - Video
    [Fact]
    public void Render_DataSourceVideoScheduleWithDataLessThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = true,
            Source = "YouTube",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("ICYMI: YouTube Video: This is a test title https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);

    }

    [Fact]
    public void Render_DataSourceVideoScheduledWithDataGreaterThanMaxLength_ShouldReturnRenderedText()
    {
        // Arrange
        var model = new Models.TweetSourceData
        {
            HashTags = new List<string>() { "HashTag1", "HashTag2" },
            IsNew = false,
            WasScheduled = true,
            Source = "YouTube",
            Title = "This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this was not enough so let me add some more characters",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("ICYMI: YouTube Video: This is going to be a really long text field to validate that we are not sending too much to twitter.  I don't know how much more I need to type to get this work but this should be good. Apparently, this ... https://www.josephguadagno.net #HashTag1 #HashTag2", renderedText);
    }
    
    [Fact]
    public void Render_DataSourceVideoScheduledWithNoHashTags_ShouldReturnRenderedTextWithDefaultHashTags()
    {
        // Arrange
        var defaultHashtags = "#dotnet #csharp #dotnetcore";

        var model = new Models.TweetSourceData
        {
            HashTags = new() ,
            IsNew = false,
            WasScheduled = true,
            Source = "YouTube",
            Title = "This is a test title",
            Url = "https://www.josephguadagno.net"
        };
        var generator = new Generator();
        
        // Act
        var renderedText = generator.Render(_twitterTemplate, model);

        // Assert
        Assert.NotNull(renderedText);
        Assert.NotEmpty(renderedText);
        Assert.Equal("ICYMI: YouTube Video: This is a test title https://www.josephguadagno.net #dotnet #csharp #dotnetcore", renderedText);
        Assert.EndsWith(defaultHashtags, renderedText);
    }
}