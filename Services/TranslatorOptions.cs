using Fanyi.Abstractions;

namespace Fanyi.Services;

public class TranslatorOptions:ITranslatorOptions
{
    public Language? From { get; set; }
    public Language? To { get; set; }

    public Language? GetTo()
        => To;
    public Language? GetFrom()
        => From;
}