using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterMapExtension.Commands
{
    internal partial class CopyToClipboard : InvokableCommand
    {
        public override string Name => "Copy";
        public override IconInfo Icon => new("\uE8C8");

        /// <summary>
        /// The _text to copy to clipboard
        /// </summary>
        private readonly string _text = string.Empty;

        public CopyToClipboard(string text)
        {
            _text = text;
        }

        public override ICommandResult Invoke()
        {
            ClipboardHelper.SetText(_text);
            return CommandResult.ShowToast($"Copied {_text} to clipboard");
        }
    }
}
