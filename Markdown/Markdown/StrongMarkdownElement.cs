using System.Text;

namespace Markdown;

public class StrongMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<strong>";
    private string closingTag = "</strong>";
    public StrongMarkdownElement(string line)
    {
        text = line.Substring(2,line.Length-4);
    }
    public string GetHtmlLine()
    { 
        string processedText = ProcessNestedText(text); 
        return $"{openingTag}{processedText}{closingTag}";
    }

    private string ProcessNestedText(string text)
    {
        var nestedTextProcessor = new NestedTextProcessor(text);
        var result = nestedTextProcessor.GetNestedHtmlLine();
        return result;
    }
}