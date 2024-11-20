using Fanyi.Abstractions;
using Fanyi.Services;
using RadLine;

namespace Fanyi.Commands;

public class SelectToLanguageCommand(ITranslatorOptions options, LanguageSelectionResultService languageSelectionResultService) : LineEditorCommand
{
    public override void Execute(LineEditorContext context)
    {
        ((TranslatorOptions)options).To = LanguageSelection.SelectLanguage(true, "目标语言");
        languageSelectionResultService.Refresh();
    }
}