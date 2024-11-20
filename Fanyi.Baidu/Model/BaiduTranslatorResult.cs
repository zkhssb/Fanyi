using System.Text.Json.Serialization;

namespace Fanyi.Baidu.Services;

public class BaiduTranslatorResult
{
    [JsonPropertyName("trans_result")] public required IEnumerable<TransResult> TransResult { get; set; }

    [JsonPropertyName("from")] public required string From { get; set; }

    [JsonPropertyName("to")] public required string To { get; set; }

    public override string ToString()
        => string.Join("", TransResult.Select(x => x.Dst).ToArray());
}