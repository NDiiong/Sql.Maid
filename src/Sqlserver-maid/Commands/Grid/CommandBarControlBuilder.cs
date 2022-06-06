#pragma warning disable IDE1006

using Microsoft.VisualStudio.CommandBars;

namespace Sqlserver.maid.Commands.Grid
{
    public static class CommandBarControlBuilder
    {
        public static CommandBarControl Caption(this CommandBarControl commandBarControl, string caption)
        {
            commandBarControl.Caption = caption;
            return commandBarControl;
        }

        public static CommandBarControl Visible(this CommandBarControl commandBarControl, bool visible)
        {
            commandBarControl.Visible = commandBarControl.Enabled = visible;
            return commandBarControl;
        }

        public static CommandBarControl TooltipText(this CommandBarControl commandBarControl, string tooltipText)
        {
            commandBarControl.TooltipText = tooltipText;
            return commandBarControl;
        }
    }
}