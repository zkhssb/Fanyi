using System.Text.Json.Serialization;

namespace Fanyi.Services;

public class BaiduTranslatorRequest
{
    [JsonPropertyName("from")]
    public required string From { get; set; } 
    [JsonPropertyName("to")]
    public required string To { get; set; }
    [JsonPropertyName("q")]
    public required string Text { get; set; }
    [JsonPropertyName("termIds")]
    public required string TermIds { get; set; }
}