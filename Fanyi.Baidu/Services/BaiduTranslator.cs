using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fanyi.Abstractions;
using Fanyi.Services;
using Microsoft.Extensions.Options;

namespace Fanyi.Baidu.Services;

public class BaiduTranslator(IOptions<BaiduTranslatorOptions> options, ITranslatorOptions translatorOptions)
    : ITranslator, IDisposable
{
    /// <summary>
    /// 百度翻译服务,客户端
    /// 文档: https://ai.baidu.com/ai-doc/MT/4kqryjku9
    /// </summary>
    private readonly HttpClient _client = new()
    {
        BaseAddress = new Uri("https://aip.baidubce.com/"),
        Timeout = TimeSpan.FromSeconds(30)
    };

    private async Task<string> GetAccessTokenAsync()
    {
        var url =
            $"/oauth/2.0/token?client_id={options.Value.ApiKey}&client_secret={options.Value.SecretKey}&grant_type=client_credentials";

        var response = await _client.PostAsync(url, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"获取百度翻译 AccessToken 失败，状态码: {response.StatusCode}");
        }

        var responseJson = await response.Content.ReadFromJsonAsync<JsonObject>();
        if (responseJson is null || !responseJson.TryGetPropertyValue("access_token", out var accessToken))
        {
            throw new Exception("百度翻译 AccessToken 响应格式无效");
        }

        return accessToken!.ToString();
    }

    public async Task<string> TranslateAsync(string text)
    {
        var accessToken = await GetAccessTokenAsync();
        var from = "auto";
        var to = IsAscii(text) ? "zh" : "en";
        if (translatorOptions.GetFrom() is SpecificLanguage sourceLanguage)
        {
            from = sourceLanguage.Code;
        }
        if (translatorOptions.GetTo() is SpecificLanguage targetLanguage)
        {
            to = targetLanguage.Code;
        }
        
        
        var request = new BaiduTranslatorRequest
        {
            From = from,
            To = to,
            Text = text,
            TermIds = string.Join(',', options.Value.TermIds ?? [])
        };

        var response = await _client.PostAsJsonAsync(
            $"/rpc/2.0/mt/texttrans/v1?access_token={accessToken}",
            request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"百度翻译接口请求失败，状态码: {response.StatusCode}");
        }

        var responseJson = await response.Content.ReadFromJsonAsync<JsonObject>();
        if (responseJson is null || responseJson.TryGetPropertyValue("error_code", out var errorCode))
        {
            throw new Exception($"百度翻译接口请求失败: {responseJson?["error_msg"]}");
        }

        var translationResult = responseJson.Deserialize<BaiduTranslatorResponse>();
        return translationResult?.Result.ToString() ?? throw new Exception("百度翻译接口响应无效");
    }

    public bool IsEnable()
        => options.Value.Enable;

    public string GetName()
        => "百度翻译";

    private static bool IsAscii(string text) => text.All(c => c <= 127);

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}