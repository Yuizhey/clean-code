using System.Text;

namespace Markdown;

public class MarkdownProcessor
{
    private IMarkdownParser _parser;
    private IMarkdownRenderer _renderer;
    public MarkdownProcessor()
    {
        _parser = new MarkdownParser();
        _renderer = new MarkdownRenderer();
    }
    public string GetHtml(string markdownText)
    {
        var elements = _parser.Parse(markdownText);
        var html = _renderer.Render(elements);
        return html;
    }  
}