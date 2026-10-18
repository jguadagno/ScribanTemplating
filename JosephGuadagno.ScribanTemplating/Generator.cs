using Scriban;

namespace JosephGuadagno.ScribanTemplating;

public class Generator
{
    public string Render(string template, object? model = null)
    {
        var parsedTemplate = Template.Parse(template);
        return parsedTemplate.Render(model);
    }
    
    public string RenderFromLiquid(string template, object? model = null)
    {
        var parsedTemplate = Template.ParseLiquid(template);
        return parsedTemplate.Render(model);
    }
}