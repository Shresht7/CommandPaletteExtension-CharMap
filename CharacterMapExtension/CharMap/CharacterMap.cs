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
    private readonly Dictionary<string, ISymbol> _characterMap = [];
    private readonly string _dataPath;

    public IReadOnlyDictionary<string, ISymbol> CharacterMap => _characterMap;

    public CharacterMapManager(string? customPath)
    {
        _dataPath = customPath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CharacterMap"
        );

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

    private void LoadFile(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };

            var characters = JsonSerializer.Deserialize<Dictionary<string, ISymbol>>(
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
}
