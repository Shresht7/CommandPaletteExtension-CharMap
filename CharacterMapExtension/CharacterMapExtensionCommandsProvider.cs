// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CharacterMapExtension.CharMap;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CharacterMapExtension;

public partial class CharacterMapExtensionCommandsProvider : CommandProvider
{
    private readonly CharacterMapManager _characterMapManager;
    private readonly ICommandItem[] _commands;

    public CharacterMapExtensionCommandsProvider()
    {
        _characterMapManager = new CharacterMapManager();
        _commands = [
            new CommandItem(new CharacterMapExtensionPage(_characterMapManager)) { Title = DisplayName },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
