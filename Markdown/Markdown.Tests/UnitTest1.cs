using System.Net.Mime;

namespace Markdown.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("#Заголовок с _курсивом_","<h1>Заголовок с <em>курсивом</em></h1>")]
    [TestCase("#Заголовок с __жирным__","<h1>Заголовок с <strong>жирным</strong></h1>")]
    [TestCase("#Заголовок с __жирным__ и _курсивом_",
        "<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em></h1>")]
    [TestCase("#Заголовок с __жирным _жирным_ текстом__",
        "<h1>Заголовок с <strong>жирным <em>жирным</em> текстом</strong></h1>")]
    [TestCase("#Заголовок с _жирным __жирным__ текстом_",
        "<h1>Заголовок с <em>жирным <strong>жирным</strong> текстом</em></h1>")]
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
        var expectedHtml = "<em>My string</em>";
        var element = new ItalicMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [TestCase("__This is a simple text.__", "<strong>This is a simple text.</strong>")]
    [TestCase("__This is _italic_ text.__", "<strong>This is <em>italic</em> text.</strong>")]
    [TestCase("__This is a number _12_3 and should not be italic.__", "<strong>This is a number _12_3 and should not be italic.</strong>")]
    [TestCase("___start_ and _middle_ and _end_.__", "<strong><em>start</em> and <em>middle</em> and <em>end</em>.</strong>")]
    [TestCase("__This _is_ a simple _test_.__", "<strong>This <em>is</em> a simple <em>test</em>.</strong>")]
    [TestCase("__This is _italic text.__", "<strong>This is _italic text.</strong>")]
    [TestCase("__This is _italic _nested_ text_.__", "<strong>This is <em>italic _nested</em> text_.</strong>")]
    [TestCase("__Text _ italics_ here.__", "<strong>Text _ italics_ here.</strong>")]
    [TestCase("__This _italic _ text jumps.__", "<strong>This _italic _ text jumps.</strong>")]
    [TestCase("__This is a \\_simple\\_ text.__", "<strong>This is a _simple_ text.</strong>")]
    [TestCase("__This is a \\\\_simple_ text.__", "<strong>This is a \\<em>simple</em> text.</strong>")]
    [TestCase("__This is a sim\\ple text.__", "<strong>This is a sim\\ple text.</strong>")]
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
        var expectedHtml = "<p>My string</p>";
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
    [TestCase("#Заголовок с _курсивом_","<h1>Заголовок с <em>курсивом</em></h1>")]
    [TestCase("#Заголовок с __жирным__","<h1>Заголовок с <strong>жирным</strong></h1>")]
    [TestCase("#Заголовок с __жирным__ и _курсивом_",
        "<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em></h1>")]
    [TestCase("#Заголовок __с _разными_ символами__",
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    public void MarkdownProcessor_GetHtml_ShouldReturnCorrectHtml(string markdownText, string expectedHtml)
    {
        // Arrange
        var markdownProcessor = new MarkdownProcessor();
        // Act
        var html = markdownProcessor.GetHtml(markdownText);
        // Assert
        Assert.AreEqual(expectedHtml,html);
    }
}