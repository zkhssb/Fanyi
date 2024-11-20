using Fanyi.Abstractions;
using Fanyi.Services;
using RadLine;

namespace Fanyi.Commands;

public class SelectFromLanguageCommand(ITranslatorOptions options, LanguageSelectionResultService languageSelectionResultService) : LineEditorCommand
{
    public override void Execute(LineEditorContext context)
    {
        ((TranslatorOptions)options).From = LanguageSelection.SelectLanguage(true, "源语言");
        languageSelectionResultService.Refresh();
    }
}