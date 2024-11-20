namespace Fanyi.Abstractions;

public interface ITranslatorOptions
{
    public Language? GetTo();
    public Language? GetFrom();
}