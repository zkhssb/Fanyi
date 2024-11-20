using System.Text.Json.Serialization;

namespace Fanyi.Baidu.Services;

public class TransResult
{
    [JsonPropertyName("src")]
    public string Src { get; set; }

    [JsonPropertyName("dst")]
    public string Dst { get; set; }
}