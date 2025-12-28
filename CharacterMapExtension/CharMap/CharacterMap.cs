using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CharacterMapExtension.CharMap;

internal class CharacterMapManager
{
    private readonly Dictionary<string, ISymbol> _characterMap = new Dictionary<string, ISymbol>();
    private readonly string _dataPath;

    public IReadOnlyDictionary<string, ISymbol> CharacterMap => _characterMap;

    public CharacterMapManager()
    {
        _dataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CharacterMap"
        );

        LoadCharacterMapData();
    }

    public CharacterMapManager(string customPath)
    {
        _dataPath = customPath;
        LoadCharacterMapData();
    }

    private void LoadCharacterMapData()
    {
        try
        {
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
                Debug.WriteLine($"Created directory: {_dataPath}");
                return;
            }

            var jsonFiles = Directory.GetFiles(_dataPath, "*.json");
            Debug.WriteLine($"Found {jsonFiles.Length} JSON files in {_dataPath}");

            foreach (var file in jsonFiles)
            {
                LoadFile(file);
            }

            Debug.WriteLine($"Loaded {_characterMap.Count} total character map json files");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading character data: {ex.Message}");
        }
    }

    private static JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    private void LoadFile(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            var characters = JsonSerializer.Deserialize<Dictionary<string, SymbolData>>(
                json,
                jsonOptions
            );

            if (characters == null)
            {
                Debug.WriteLine($"No characters found in {filePath}");
                return;
            }

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var loadedCount = 0;

            foreach (var kvp in characters)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                if (_characterMap.TryAdd(key, value))
                {
                    loadedCount++;
                }
            }

            Debug.WriteLine($"Loaded {loadedCount} characters from {fileName}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading file {filePath}: {ex.Message}");
        }
    }

    public void Reload()
    {
        _characterMap.Clear();
        LoadCharacterMapData();
    }

    public IEnumerable<(ISymbol symbol, int score)> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return _characterMap.Values.Select(c => (c, score: 0));
        }

        var searchTerm = query.Trim().ToLower();
        var results = new List<(ISymbol symbol, int score)>();

        foreach (var symbol in _characterMap.Values)
        {
            var score = CalculateScore(searchTerm, symbol);
            if (score > 0)
            {
                results.Add((symbol, score));
            }
        }

        return results.OrderByDescending(r => r.score);
    }

    private static int CalculateScore(string query, ISymbol symbol)
    {
        int maxScore = 0;

        // Symbol Exact Match (highest priority)
        if (symbol.Symbol?.Equals(query, StringComparison.OrdinalIgnoreCase) == true)
        {
            return 1000;
        }

        // Description
        if (!string.IsNullOrEmpty(symbol.Description))
        {
            var descScore = FuzzyStringMatcher.ScoreFuzzy(query, symbol.Description);
            maxScore = Math.Max(maxScore, descScore);
        }

        // LaTeX Command
        if (!string.IsNullOrEmpty(symbol.Latex))
        {
            var latexQuery = query.StartsWith("\\") ? query : "\\" + query;

            var latexScore1 = FuzzyStringMatcher.ScoreFuzzy(latexQuery, symbol.Latex);
                maxScore = Math.Max(maxScore, latexScore1 * 10);

            var latexScore2 = FuzzyStringMatcher.ScoreFuzzy(query, symbol.Latex);
                maxScore = Math.Max(maxScore, latexScore2);

            // Keywords
            if (symbol.Keywords != null)
            {
                foreach (var keyword in symbol.Keywords)
                {
                    var keywordScore = FuzzyStringMatcher.ScoreFuzzy(query, keyword);
                        maxScore = Math.Max(maxScore, keywordScore);
                }
            }

            // Category
            if (!string.IsNullOrEmpty(symbol.Category))
            {
                var categoryScore = FuzzyStringMatcher.ScoreFuzzy(query, symbol.Category);
                maxScore = Math.Max(maxScore, categoryScore / 2);
            }
        }

        return maxScore;
    }
}
