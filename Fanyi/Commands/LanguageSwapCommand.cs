using Fanyi.Abstractions;
using Fanyi.Services;
using RadLine;

namespace Fanyi.Commands;

public class LanguageSwapCommand(ITranslatorOptions options, LanguageSelectionResultService languageSelectionResultService) : LineEditorCommand
{
    public override void Execute(LineEditorContext context)
    {
        var to = ((TranslatorOptions)options).To;
        var from = ((TranslatorOptions)options).From;
        ((TranslatorOptions)options).To = from;
        ((TranslatorOptions)options).From = to;
        languageSelectionResultService.Refresh();
    }
}