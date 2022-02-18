﻿using System;
using System.Windows.Forms;

namespace Sqlserver.maid.Toolkit.Services
{
    internal class ClipboardService : IClipboardService
    {
        public string GetFromClipboard()
        {
            try
            {
                if (Clipboard.ContainsText())
                    return Clipboard.GetText(TextDataFormat.UnicodeText);
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }
    }
}
