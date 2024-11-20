# 配置文件

配置文件在程序运行目录下的`appsettings.json`

```json lines
{
  "Translator": {
    "OpenAi": {
      "Enable": false,          // 总开关
      "UseProxy": false,        // 是否使用代理
      "ProxyUrl": "",           // 使用的代理地址(带v1路径)
      "ApiKey": "",             // 你的api key
      "Model": "gpt-3.5-turbo"  // 使用的模型
    },
    "Baidu": {
      "Enable": false,          // 总开关
      "ApiKey": "",             // 你的api key
      "SecretKey": "",          // 你的secret key
      "TermIds": []             // 术语库(文本数组,百度限制最多10个)
    }
  }
}
```