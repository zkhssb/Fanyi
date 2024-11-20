using System.Globalization;

namespace Fanyi;

public class SpecificLanguage : Language
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public override string ToString()
        => $"{Code} ({Name})";
}