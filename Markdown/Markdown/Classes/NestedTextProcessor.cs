using System.Text;

namespace Markdown;

public class NestedTextProcessor
{
    private string text;

    public NestedTextProcessor(string nestedText)
    {
        text = nestedText;
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
                if (i + 1 < text.Length && text[i + 1]=='_')
                {
                    currentText.Append('_');
                    i++;
                    continue;
                }
                else if (i + 1 < text.Length && text[i + 1]=='\\')
                {
                    currentText.Append('\\');
                    i++;
                    continue;
                }
                else
                {
                    currentText.Append('\\');
                    continue;
                }
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
                bool isStartOfItalic = (i + 1 < text.Length && !char.IsWhiteSpace(text[i + 1]) && char.IsLetterOrDigit(text[i + 1]));
                bool isEndOfItalic = (i > 0 && !char.IsWhiteSpace(text[i - 1]) && char.IsLetterOrDigit(text[i - 1]));

                if (isItalic) // Если курсив включен
                {
                    // Закрываем курсив только если подчеркивание закончилось корректно
                    if (isEndOfItalic)
                    {
                        // Добавляем текущий текст в курсив
                        result.Append("<em>").Append(currentText.ToString().Substring(1)).Append("</em>");
                        currentText.Clear();
                        isItalic = false; // Завершаем курсив
                    }
                    else
                    {
                        // Не подходит для закрытия, поэтому добавляем подчеркивание
                        currentText.Append(currentChar);
                    }
                }
                else // Если курсив еще не включен
                {
                    // Проверяем, можно ли начать курсив
                    if (isStartOfItalic)
                    {
                        // Добавляем текст перед курсивным выделением
                        result.Append(currentText);
                        currentText.Clear();
                        currentText.Append('_');
                        isItalic = true; 
                    }
                    else
                    {
                        // Если не подходит для открытия, добавляем подчеркивание как обычный символ
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

    public string GetNestedHtmlLine()
    {
        var result = ProcessNestedText(text);
        return result.ToString();
    }
}