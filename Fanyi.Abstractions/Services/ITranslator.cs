namespace Fanyi.Abstractions;

public interface ITranslator
{
    public Task<string> TranslateAsync(string text);
    public bool IsEnable();
    public string GetName();
}