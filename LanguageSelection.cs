using System.Globalization;
using Spectre.Console;

namespace Fanyi;

public class LanguageSelection
{
    private static readonly List<SpecificLanguage> Languages = LanguageMap.Maps
        .Select(x => new SpecificLanguage
        {
            Name = x.Key,
            Code = x.Value
        }).ToList();

    public static Language SelectLanguage(bool allowAuto, string title)
        => AnsiConsole.Prompt(
            new SelectionPrompt<Language>
                {
                    SearchEnabled = true
                }
                .Title($"选择你的 [green]{title}[/]")
                .PageSize(15)
                .SearchPlaceholderText("[grey](输入文本可搜索)[/]")
                .MoreChoicesText("[grey](↑↓以显示更多语言)[/]")
                .AddChoices(allowAuto ? [new AutoLanguage(), ..Languages] : Languages));

    public static Language Parse(string? input)
    {
        return input switch
        {
            null => new AutoLanguage(),
            "auto" => new AutoLanguage(),
            _ => Languages.FirstOrDefault(x => x.Code.Equals(input, StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException("找不到语言:" + input),
        };
    }
}