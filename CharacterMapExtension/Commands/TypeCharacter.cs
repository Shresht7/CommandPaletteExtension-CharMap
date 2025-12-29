using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CharacterMapExtension.Commands
{
    internal partial class TypeCharacter : InvokableCommand
    {
        private readonly string _char;
        private const int WAIT_TIME = 250;

        public TypeCharacter(string ch)
        {
            Name = "Place";
            _char = ch;
        }

        public override ICommandResult Invoke()
        {
            var _ = Task.Run(() =>
            {
                Thread.Sleep(WAIT_TIME);
                SendKeys.SendWait(_char);
            });

            return CommandResult.Hide();
        }
    }
}
