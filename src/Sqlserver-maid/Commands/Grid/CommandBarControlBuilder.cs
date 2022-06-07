#pragma warning disable IDE1006

using Microsoft.VisualStudio.CommandBars;
using Sqlserver.maid.Services.Extension;
using stdole;
using System.Drawing;
using IconConverter = Sqlserver.maid.Services.IconConverter;

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

    public static class CommandBarButtonBuilder
    {
        public static CommandBarButton AddIcon(this CommandBarButton commandBarButton, Icon icon)
        {
            commandBarButton.Picture = IconConverter.GetPictureDispFromImage(icon.ToBitmap()).As<StdPicture>();
            return commandBarButton;
        }
    }
}