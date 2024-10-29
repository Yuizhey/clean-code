using System.Text;
using Markdown;

public class HeaderMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<h1>";
    private string closingTag = "</h1>";
    public HeaderMarkdownElement(string line)
    {
        text = line.Substring(1);
    }
    public string GetHtmlLine()
    {
        var nestedText = ProcessNested(text);
        return $"{openingTag}{nestedText}{closingTag}\n";
    }
    private string ProcessNested(string text)
    {
        StringBuilder result = new StringBuilder();
        StringBuilder currentText = new StringBuilder();
        bool isBold = false; 
        bool isItalic = false; 

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            if (currentChar == '\\')
            {
                if (i + 1 < text.Length)
                {
                    char nextChar = text[i + 1];
                    if (nextChar == '_' || nextChar == '\\')
                    {
                        currentText.Append(nextChar);
                        i++;
                        continue;
                    }
                }

                currentText.Append(currentChar);
                continue;
            }

            if (currentChar == '_')
            {
                bool isStartOfItalic = (i + 1 < text.Length && !char.IsWhiteSpace(text[i + 1]) && char.IsLetter(text[i + 1]));
                bool isEndOfItalic = (i > 0 && !char.IsWhiteSpace(text[i - 1]) && char.IsLetter(text[i - 1]) || text[i - 2] == '\\');
                bool isPreviousCharDigit = (i > 0 && char.IsDigit(text[i - 1]));
                bool isNextCharDigit = (i + 1 < text.Length && char.IsDigit(text[i + 1]));

                if (isPreviousCharDigit || isNextCharDigit)
                {
                    currentText.Append(currentChar);
                    continue;
                }

                if (i + 1 < text.Length && text[i + 1] == '_')
                {
                    i++; 
                    if (isItalic)
                    {
                        if (currentChar == '_')
                            currentText.Append(currentChar);
                        currentText.Append(currentChar);
                        continue;
                    }
                    else
                    {
                        if (isBold) // Закрываем жирный текст
                        {
                            if (currentText.Length > 0)
                            {
                                result.Append(currentText);
                                currentText.Clear();
                            }
                            result.Append("</strong>");
                            isBold = false;
                        }
                        else // Открываем жирный текст
                        {
                            if (currentText.Length > 0)
                            {
                                result.Append(currentText);
                                currentText.Clear();
                            }
                            result.Append("<strong>");
                            isBold = true;
                        }
                    }
                }
                else 
                {
                    if (isItalic) // Закрываем курсивный текст
                    {
                        if (isEndOfItalic)
                        {
                            result.Append("<em>").Append(currentText.ToString().Substring(1)).Append("</em>");
                            currentText.Clear();
                            isItalic = false;
                        }
                        else
                        {
                            currentText.Append(currentChar);
                        }
                    }
                    else // Открываем курсивный текст
                    {
                        if (isStartOfItalic)
                        {
                            result.Append(currentText);
                            currentText.Clear();
                            currentText.Append('_');
                            isItalic = true;
                        }
                        else
                        {
                            currentText.Append(currentChar);
                        }
                    }
                }
            }
            else
            {
                currentText.Append(currentChar); 
            }
        }
        if (currentText.Length> 0)
        {
            result.Append(currentText.ToString());
        }

        return result.ToString();
    }
}
