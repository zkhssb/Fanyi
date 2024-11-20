namespace Fanyi.Baidu;

public class BaiduTranslatorOptions
{
    public required bool Enable { get; init; }
    public required string ApiKey { get; init; }
    public required string SecretKey { get; init; }
    public required string[] TermIds { get; init; }
}