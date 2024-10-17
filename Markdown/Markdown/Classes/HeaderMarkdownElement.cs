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
        var nestedText = ProcessNestedTags(text);
        return $"{openingTag}{nestedText}{closingTag}";
    }
    private string ProcessNestedTags(string text)
    {
        StringBuilder result = new StringBuilder();
        StringBuilder currentText = new StringBuilder();
        bool isBold = false; // Жирный текст
        bool isItalic = false; // Курсивный текст

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            if (currentChar == '_')
            {
                // Проверка на двойное подчеркивание для жирного текста
                if (i + 1 < text.Length && text[i + 1] == '_')
                {
                    i++; // Пропускаем двойное подчеркивание

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
                else // Обработка курсивного текста
                {
                    if (isItalic) // Закрываем курсивный текст
                    {
                        if (currentText.Length > 0)
                        {
                            result.Append(currentText);
                            currentText.Clear();
                        }
                        result.Append("</em>");
                        isItalic = false;
                    }
                    else // Открываем курсивный текст
                    {
                        if (currentText.Length > 0)
                        {
                            result.Append(currentText);
                            currentText.Clear();
                        }
                        result.Append("<em>");
                        isItalic = true;
                    }
                }
            }
            else
            {
                currentText.Append(currentChar); // Добавляем символ к текущему тексту
            }
        }

        return result.ToString();
    }
}
