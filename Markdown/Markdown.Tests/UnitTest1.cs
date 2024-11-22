using System.Net.Mime;

namespace Markdown.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("#Заголовок с _курсивом_", "<h1>Заголовок с <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок с __жирным__", "<h1>Заголовок с <strong>жирным</strong></h1>\n")]
    [TestCase("#Заголовок с __жирным__ и _курсивом_",
        "<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок с __жирным _жирным_ текстом__",
        "<h1>Заголовок с <strong>жирным <em>жирным</em> текстом</strong></h1>\n")]
    [TestCase("#Заголовок с _жирным __жирным__ текстом_",
        "<h1>Заголовок с <em>жирным __жирным__ текстом</em></h1>\n")]
    [TestCase("#This is a \\_simple\\_ text.", "<h1>This is a _simple_ text.</h1>\n")]
    [TestCase("#This is a \\\\_simple_ text.", "<h1>This is a \\<em>simple</em> text.</h1>\n")]
    [TestCase("#This is a sim\\ple text.", "<h1>This is a sim\\ple text.</h1>\n")]
    [TestCase("#This is a \\_simple __strong__ \\_ text.", "<h1>This is a _simple <strong>strong</strong> _ text.</h1>\n")]
    [TestCase("#This is a \\__simple __strong__ \\__ text.", "<h1>This is a _<em>simple __strong__ _</em> text.</h1>\n")]
    [TestCase("#This is a number _12_3 and should not be italic.", "<h1>This is a number _12_3 and should not be italic.</h1>\n")]
    [TestCase("#Text _ italics_ here.", "<h1>Text _ italics_ here.</h1>\n")]
    [TestCase("#This _italic _ text jumps.", "<h1>This _italic _ text jumps.</h1>\n")]
    public void HeaderMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString(string text,string expectedHtml)
    {
        // Arrange
        var element = new HeaderMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [Test]
    public void ItalicMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString()
    {
        // Arrange
        var text = "_My string_";
        var expectedHtml = "<em>My string</em>\n";
        var element = new ItalicMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [TestCase("__This is a simple text.__", "<strong>This is a simple text.</strong>\n")]
    [TestCase("__This is _italic_ text.__", "<strong>This is <em>italic</em> text.</strong>\n")]
    [TestCase("__This is a number _12_3 and should not be italic.__", "<strong>This is a number _12_3 and should not be italic.</strong>\n")]
    [TestCase("___start_ and _middle_ and _end_.__", "<strong><em>start</em> and <em>middle</em> and <em>end</em>.</strong>\n")]
    [TestCase("__This _is_ a simple _test_.__", "<strong>This <em>is</em> a simple <em>test</em>.</strong>\n")]
    [TestCase("__This is _italic text.__", "<strong>This is _italic text.</strong>\n")]
    [TestCase("__This is _italic _nested_ text_.__", "<strong>This is <em>italic _nested</em> text_.</strong>\n")]
    [TestCase("__Text _ italics_ here.__", "<strong>Text _ italics_ here.</strong>\n")]
    [TestCase("__This _italic _ text jumps.__", "<strong>This _italic _ text jumps.</strong>\n")]
    [TestCase("__This is a \\_simple\\_ text.__", "<strong>This is a _simple_ text.</strong>\n")]
    [TestCase("__This is a \\\\_simple_ text.__", "<strong>This is a \\<em>simple</em> text.</strong>\n")]
    [TestCase("__This is a sim\\ple text.__", "<strong>This is a sim\\ple text.</strong>\n")]
    public void StrongMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString(string text,string expectedHtml)
    {
        // Arrange
        var element = new StrongMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [Test]
    public void LinkMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString()
    {
        var element = new LinkMarkdownElement("[google](https://www.google.ru/?hl=ru)");
        var htmlLine = element.GetHtmlLine();
        var expected = "<a href='https://www.google.ru/?hl=ru'>google</a>";
        Assert.AreEqual(expected,htmlLine);
    }

    [Test]
    public void ParagraphMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString()
    {
        // Arrange
        var text = "My string";
        var expectedHtml = "<p>My string</p>\n";
        var element = new ParagraphMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }
    
    [TestCase("#My string","<h1>My string</h1>\n")]
    [TestCase("_My String_","<em>My String</em>\n")]
    [TestCase("__My string__","<strong>My string</strong>\n")]
    [TestCase("My string","<p>My string</p>\n")]
    [TestCase("#My string\n_My string_\n__My string__",
        "<h1>My string</h1>\n<em>My string</em>\n<strong>My string</strong>\n")]
    [TestCase("#Заголовок с _курсивом_", "<h1>Заголовок с <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок с __жирным__", "<h1>Заголовок с <strong>жирным</strong></h1>\n")]
    [TestCase("#Заголовок с __жирным__ и _курсивом_",
        "<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок __с _разными_ символами__",
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>\n")]
    public void MarkdownProcessor_GetHtml_ShouldReturnCorrectHtml(string markdownText, string expectedHtml)
    {
        // Arrange
        var parser = new MarkdownParser();
        var renderer = new MarkdownRenderer();
        var markdownProcessor = new MarkdownProcessor(parser, renderer);
        // Act
        var html = markdownProcessor.GetHtml(markdownText);
        // Assert
        Assert.AreEqual(expectedHtml,html);
    }
}