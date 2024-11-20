using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Fanyi;

public class AppOptions
{
    [Option('c', "content", Required = false, HelpText = "要翻译的文本")]
    public string? Text { get; set; }

    [Option('t', "to", Required = false, HelpText = "目标语言")]
    public string? To { get; set; }

    [Option('f', "from", Required = false, HelpText = "要翻译的语言")]
    public string? From { get; set; }
}