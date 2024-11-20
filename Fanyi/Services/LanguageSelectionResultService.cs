using System.IO.Pipes;
using Fanyi.Abstractions;
using Spectre.Console;

namespace Fanyi.Services;

public class LanguageSelectionResultService(ITranslatorOptions options)
{
    private readonly TranslatorOptions _translatorOptions = (TranslatorOptions)options;
    private int _line;

    public void WriteLine()
    {
        (_, _line) = Console.GetCursorPosition();
        AnsiConsole.Write("\u001b[0K\u001b[1G");
        AnsiConsole.MarkupLine(BuildPrompt());
    }

    private void ClearLine()
    {
        AnsiConsole.Write($"\u001b[{_line};0H");
        AnsiConsole.Write("\u001b[0K");
    }

    public void Refresh()
    {
        ClearLine();
        AnsiConsole.Markup(BuildPrompt());
        AnsiConsole.WriteLine();
    }

    private static string GetLanguageName(Language? language)
    {
        return language switch
        {
            null => "auto",
            AutoLanguage => "auto",
            SpecificLanguage specificLanguage => specificLanguage.Code,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private string BuildPrompt()
        =>
            $"[[[lightskyblue1]{GetLanguageName(_translatorOptions.GetFrom())}[/][yellow]=>[/][lightskyblue1]{GetLanguageName(_translatorOptions.GetTo())}[/]]]";
}