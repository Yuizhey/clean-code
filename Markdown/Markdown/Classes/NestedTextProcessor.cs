using System.Text;

namespace Markdown;

public class NestedTextProcessor
{
    private string text;

    public NestedTextProcessor(string nestedText)
    {
        text = nestedText;
    }
    public string GetNestedHtmlLine()
    {
        var result = ProcessNestedText(text);
        return result.ToString();
    }
    private string ProcessNestedText(string text)
    {
        var result = new StringBuilder();
        var currentText = new StringBuilder();
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
                // Проверяем окружение подчеркивания
                bool isPreviousCharDigit = (i > 0 && char.IsDigit(text[i - 1]));
                bool isNextCharDigit = (i + 1 < text.Length && char.IsDigit(text[i + 1]));

                if (isPreviousCharDigit || isNextCharDigit)
                {
                    currentText.Append(currentChar);
                    continue;
                }

                // Проверка для начала и закрытия курсивного выделения
                bool isStartOfItalic = (i + 1 < text.Length && !char.IsWhiteSpace(text[i + 1]) && char.IsLetter(text[i + 1]));
                bool isEndOfItalic = (i > 0 && !char.IsWhiteSpace(text[i - 1]) && char.IsLetter(text[i - 1]));

                if (isItalic) 
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
                else 
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
            else
            {
                currentText.Append(currentChar); 
            }
        }

        if (currentText.Length > 0)
        {
            result.Append(currentText);
        }
        
        return result.ToString();
    }
}