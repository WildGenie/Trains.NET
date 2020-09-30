﻿using System.Collections.Generic;
using System.Linq;
using Trains.NET.Engine;

namespace Trains.NET.Rendering.UI
{
    public class CommandsPanel : ButtonPanelBase
    {
        private readonly List<Button> _buttons;

        protected override bool Collapsed { get; set; } = true;
        protected override string? Title => "Commands";
        protected override int Top => 250;

        public CommandsPanel(IEnumerable<ICommand> commands)
        {
            _buttons = commands.Select(c => new Button(c.Name, c, () => false, () => c.Execute())).ToList();
        }

        protected override IEnumerable<Button> GetButtons()
            => _buttons;
    }
}
