﻿using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services.Extension;
using Sqlserver.maid.Services.Loging;
using Sqlserver.maid.Services.SqlPackage;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands.Excute
{
    public class WindowEventLogging
    {
        public static async Task InitializeAsync(SqlAsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var commandService = package.GetService<IMenuCommandService, OleMenuCommandService>();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

            var windowEvents = dte.Events.get_WindowEvents();
            windowEvents.WindowCreated += WindowEvents_WindowCreated;
            windowEvents.WindowClosing += WindowEvents_WindowClosing;
            windowEvents.WindowActivated += WindowEvents_WindowActivated;
            windowEvents.WindowMoved += WindowEvents_WindowMoved;
        }

        private static void WindowEvents_WindowMoved(Window Window, int Top, int Left, int Width, int Height)
        {
            Logger.Info("Call [WindowEvents_WindowMoved]");
        }

        private static void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            Logger.Info("Call [WindowEvents_WindowActivated]");
        }

        private static void WindowEvents_WindowClosing(Window Window)
        {
            Logger.Info("Call [WindowEvents_WindowClosing]");
        }

        private static void WindowEvents_WindowCreated(Window Window)
        {
            Logger.Info("Call [WindowEvents_WindowCreated]");
        }
    }
}