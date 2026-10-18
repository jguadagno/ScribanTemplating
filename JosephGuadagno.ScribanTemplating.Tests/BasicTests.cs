namespace JosephGuadagno.ScribanTemplating.Tests;

public class BasicTests
{
    [Fact]
    private void Render_WithTextAndNoParameters_ShouldGenerateText()
    {
        // Arrange
        var template = "Hello World";
        var generator = new Generator();
        
        // Act
        var result = generator.Render(template);
        
        // Assert
        Assert.Equal(template, result);
    }
    
    [Fact]
    private void Render_WithTextAndParameters_ShouldGenerateText()
    {
        // Arrange
        var template = "Hello World {{ name }}";
        var name = "Joseph";
        var generator = new Generator();
        
        // Act
        var result = generator.Render(template, new {Name = name});
        
        // Assert
        Assert.Equal($"Hello World {name}", result);
    }
}