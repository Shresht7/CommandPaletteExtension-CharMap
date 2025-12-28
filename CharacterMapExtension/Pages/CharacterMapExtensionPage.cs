// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CharacterMapExtension.CharMap;
using CharacterMapExtension.Commands;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using Windows.System;

namespace CharacterMapExtension;

internal sealed partial class CharacterMapExtensionPage : ListPage
{
    private readonly CharacterMapManager _characterMap;

    public CharacterMapExtensionPage(CharacterMapManager characterMap)
    {
        Title = "Character Map";
        Name = "Character Map";
        PlaceholderText = "Search Character...";
        Icon = new IconInfo("\u0394");
        _characterMap = characterMap;
    }

    public override IListItem[] GetItems()
    {
        List<ListItem> results = new();
        var searchResults = _characterMap.Search("");
        foreach (var item in searchResults)
        {
            results.Add(CreateListItem(item));
        }
        return results.ToArray();
    }

    private static ListItem CreateListItem((ISymbol symbol, int score) item)
    {
        List<string> _subtitle = [];
        if (!string.IsNullOrEmpty(item.symbol.Unicode))
        {
            _subtitle.Add($"Unicode: {item.symbol.Unicode.ToUpper()}");
        }
        if (!string.IsNullOrEmpty(item.symbol.Dec))
        {
            _subtitle.Add($"Unicode: {item.symbol.Dec}");
        }
        if (!string.IsNullOrEmpty(item.symbol.Latex))
        {
            _subtitle.Add($"Unicode: {item.symbol.Latex}");
        }
        string subtitle = string.Join("  |  ", _subtitle);

        Tag[] tags = [
            new Tag() { Text = item.symbol.Category, ToolTip = "Category" },
            ];

        var result = new ListItem()
        {
            Title = item.symbol.Description,
            Subtitle = subtitle,
            Tags = tags,
            Icon = new IconInfo(item.symbol.Symbol),
            Command = new CopyToClipboard(item.symbol.Symbol),
            MoreCommands = [
                new CommandContextItem(new CopyToClipboard(item.symbol.Unicode)) {
                    Title = "Copy Unicode",
                    RequestedShortcut = new KeyChord() { Vkey = (int)VirtualKey.U, Modifiers = VirtualKeyModifiers.Control }
                },
                new CommandContextItem(new CopyToClipboard(item.symbol.Dec)) {
                    Title = "Copy Decimal",
                    RequestedShortcut = new KeyChord() { Vkey = (int)VirtualKey.D, Modifiers = VirtualKeyModifiers.Control }
                },
                new CommandContextItem(new CopyToClipboard(item.symbol.Latex)) {
                    Title = "Copy LaTeX",
                    RequestedShortcut = new KeyChord() { Vkey = (int)VirtualKey.L, Modifiers = VirtualKeyModifiers.Control }
                },
            ]
        };

        return result;
    }
}
