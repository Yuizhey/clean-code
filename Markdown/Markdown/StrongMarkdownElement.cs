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
        string processedText = ProcessNestedItalic(text); 
        return $"{openingTag}{processedText}{closingTag}";
    }

    private string ProcessNestedItalic(string text)
    {
        StringBuilder result = new StringBuilder();
        StringBuilder currentText = new StringBuilder();
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
                    i+=2;
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
                    // Подчеркивание окружено цифрами, оставляем как обычный символ
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
                        isItalic = true; // Включаем курсив
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
                currentText.Append(currentChar); // Добавляем текущий символ
            }
        }

        // Добавляем остатки текста в результат
        if (currentText.Length > 0)
        {
            if (isItalic)
            {
                // Помним, что курсив открыт, но добавляем текст без изменений
                result.Append(currentText);
            }
            else
            {
                result.Append(currentText); // Добавляем текущий текст
            }
        }

        // Если курсив еще открыт, добавляем его из результата
        if (isItalic)
        {
            return result.ToString(); // Если курсив не закрыт, возвращаем его как есть
        }

        // Если все корректно, завершаем как <strong>
        return result.ToString();
    }
}