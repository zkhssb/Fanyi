using System.Text;
using CommandLine;
using Fanyi;
using Fanyi.Abstractions;
using Fanyi.Baidu.Services;
using Fanyi.Baidu;
using Fanyi.ChatGPT;
using Fanyi.ChatGPT.Services;
using Fanyi.Commands;
using Fanyi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RadLine;
using Spectre.Console;

Console.OutputEncoding = Encoding.UTF8;
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var options = Parser.Default.ParseArguments<AppOptions>(args).Value;
var collection = new ServiceCollection()
    .AddSingleton<LanguageSelectionResultService>()
    .AddSingleton<LanguageSwapCommand>()
    .AddSingleton<SelectFromLanguageCommand>()
    .AddSingleton<SelectToLanguageCommand>()
    .AddSingleton<ITranslatorOptions, TranslatorOptions>(_ => new TranslatorOptions
    {
        From = LanguageSelection.Parse(options.From),
        To = LanguageSelection.Parse(options.To)
    })
    .Configure<BaiduTranslatorOptions>(configuration.GetSection("Translator:Baidu"))
    .AddTransient<ITranslator, BaiduTranslator>()
    .Configure<ChatGPTTranslatorOptions>(configuration.GetSection("Translator:OpenAi"))
    .AddTransient<ITranslator, ChatGPTTranslator>();

var provider = collection.BuildServiceProvider();

var translatorOptions = (TranslatorOptions)provider.GetRequiredService<ITranslatorOptions>();
var languageSelectionResultService = provider.GetRequiredService<LanguageSelectionResultService>();
var lineEditor = new LineEditor
{
    MultiLine = true,
    Prompt = new LineEditorPrompt("[gray]:[/]")
};

lineEditor.KeyBindings.Add(ConsoleKey.LeftArrow, ConsoleModifiers.Shift,
    () => provider.GetRequiredService<SelectFromLanguageCommand>());
lineEditor.KeyBindings.Add(ConsoleKey.RightArrow, ConsoleModifiers.Shift,
    () => provider.GetRequiredService<SelectToLanguageCommand>());
lineEditor.KeyBindings.Add(ConsoleKey.UpArrow, ConsoleModifiers.Shift,
    () => provider.GetRequiredService<LanguageSwapCommand>());
lineEditor.KeyBindings.Add(ConsoleKey.DownArrow, ConsoleModifiers.Shift,
    () => provider.GetRequiredService<LanguageSwapCommand>());

if (!string.IsNullOrWhiteSpace(options.Text))
{
        var enabledTranslators = provider.GetServices<ITranslator>().Where(x => x.IsEnable()).ToArray();
    if (enabledTranslators.Length == 0)
    {
        AnsiConsole.MarkupLine("[red]当前没有任何启用的翻译器! 请到[/][yellow]appsettings.json[/][red]配置启用一个翻译器.[/]");
        return;
    }
    await StartTranslate(options.Text, enabledTranslators);
    return;
}

while (true)
{
    languageSelectionResultService.WriteLine();
    var text = await lineEditor.ReadLine(CancellationToken.None);
    if (string.IsNullOrWhiteSpace(text)) continue;

    Console.WriteLine();

    var enabledTranslators = provider.GetServices<ITranslator>().Where(x => x.IsEnable()).ToArray();
    if (enabledTranslators.Length == 0)
    {
        AnsiConsole.MarkupLine("[red]当前没有任何启用的翻译器! 请到[/][yellow]appsettings.json[/][red]配置启用一个翻译器.[/]");
        continue;
    }
    await StartTranslate(text, enabledTranslators);
}


async Task StartTranslate(string text, IEnumerable<ITranslator> translators)
{
    await AnsiConsole.Status()
        .Spinner(Spinner.Known.Dots)
        .StartAsync("开始翻译...", async ctx =>
        {
            foreach (var translator in translators)
            {
                ctx.Status($"正在使用[lightgoldenrod3]{translator.GetName()}[/]翻译你的文本...");
                try
                {
                    var result = await translator.TranslateAsync(text);
                    AnsiConsole.MarkupLine($"[gray]>>[/] [lightgoldenrod3]{translator.GetName()}[/] 翻译结果");
                    ctx.Status("_");
                    await Task.Delay(TimeSpan.FromSeconds(0.1)); // 给控制台一点刷新的时间,如无此延迟将可能导致状态文本刷新失败
                    foreach (var line in result.Split(Environment.NewLine))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(0.01));
                        AnsiConsole.WriteLine(line);
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine(
                        $"[gray]>>[/] [red]翻译器[/] [orange1]{translator.GetName()}[/] 翻译失败: [red]{ex.Message}[/]");
                }
            }

            Console.WriteLine();
        });
}

string GetLanguageName(Language? language)
{
    return language switch
    {
        null => "auto",
        AutoLanguage => "auto",
        SpecificLanguage specificLanguage => specificLanguage.Code,
        _ => throw new ArgumentOutOfRangeException()
    };
}

string BuildPrompt()
    =>
        $"[[[lightskyblue1]{GetLanguageName(translatorOptions.GetFrom())}[/][yellow]=>[/][lightskyblue1]{GetLanguageName(translatorOptions.GetTo())}[/]]]";