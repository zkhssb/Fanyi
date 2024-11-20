namespace Fanyi.ChatGPT;

public class ChatGPTTranslatorOptions
{
    public bool Enable { get; set; }
    public bool UseProxy { get; set; }
    public string? ProxyUrl { get; set; }
    public string? ApiKey { get; set; }
    public string? Model { get; set; }
}