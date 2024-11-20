using System.Text.Json.Serialization;

namespace Fanyi.Baidu.Services;

public class BaiduTranslatorResponse
{
    [JsonPropertyName("result")]
    public required BaiduTranslatorResult Result { get; set; } 

    [JsonPropertyName("log_id")]
    public required long LogId { get; set; }
}