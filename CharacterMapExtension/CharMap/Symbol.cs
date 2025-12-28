using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CharacterMapExtension.CharMap;

internal class SymbolData: ISymbol
{
    [JsonPropertyName("symbol")]
    public string Symbol => string.Empty;

    [JsonPropertyName("description")]
    public string Description => string.Empty;

    [JsonPropertyName("category")]
    public string Category => string.Empty;

    [JsonPropertyName("unicode")]
    public string Unicode => string.Empty;

    [JsonPropertyName("decimal")]
    public string Dec => string.Empty;

    [JsonPropertyName("latex")]
    public string Latex => string.Empty;

    [JsonPropertyName("keywords")]
    public List<string> Keywords => [];
}
