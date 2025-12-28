// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CharacterMapExtension.CharMap;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;

namespace CharacterMapExtension;

internal sealed partial class CharacterMapExtensionPage : ListPage
{
    private readonly CharacterMapManager _characterMap;

    public CharacterMapExtensionPage(CharacterMapManager characterMap)
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "Character Map";
        Name = "Search";
        PlaceholderText = "Search Character...";
        _characterMap = characterMap;

    }

    public override IListItem[] GetItems()
    {
        List<ListItem> results = [];

        var searchResults = _characterMap.Search("");
        foreach (var item in searchResults)
        {
            var result = new ListItem()
            {
                Title = item.symbol.Symbol,
                Subtitle = item.symbol.Description,
            };
            results.Add(result);
        }

        return results.ToArray();
    }
}
