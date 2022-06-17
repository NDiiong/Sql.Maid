﻿using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services.Extension;
using Sqlserver.maid.Services.Loging;
using Sqlserver.maid.Services.SqlPackage;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.IO;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands.Excute
{
    internal sealed class QueryHistoryCommand
    {
        private static readonly string _location;
        private static readonly ConcurrentQueue<QueryItem> _itemsQueue = new ConcurrentQueue<QueryItem>();
        public static CommandEvents QueryExecuteEvent { get; private set; }

        static QueryHistoryCommand()
        {
            var localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sqlserver-maid");
            _location = Path.Combine(localPath, "histories");
            Directory.CreateDirectory(_location);
        }

        public static async Task InitializeAsync(SqlAsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var commandService = package.GetService<IMenuCommandService, OleMenuCommandService>();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

            var command = dte.Commands.Item("Query.Execute");
            QueryExecuteEvent = dte.Events.get_CommandEvents(command.Guid, command.ID);
            QueryExecuteEvent.BeforeExecute += (string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault) => BeforeExecute(dte);
            QueryExecuteEvent.AfterExecute += CommandEvents_AfterExecute;
        }

        private static void CommandEvents_AfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            Logger.Info("CommandEvents_AfterExecute");
        }

        private static void BeforeExecute(DTE2 dte)
        {
            string queryText = GetQueryText(dte);
            if (string.IsNullOrWhiteSpace(queryText))
                return;

            _itemsQueue.Enqueue(new QueryItem
            {
                Query = queryText,
                ExecutionDate = DateTime.Now,
            });

            Task.Delay(1000).ContinueWith((t) => SaveHistories());
        }

        private static void SaveHistories()
        {
            lock (_itemsQueue)
            {
                while (_itemsQueue.TryDequeue(out var item))
                {
                    var path = Path.Combine(_location, item.ExecutionDate.ToString("dd.MM.yyyy.hh.mm.ss.fff") + Path.GetRandomFileName());
                    File.AppendAllText(path + ".txt", $"{item.ExecutionDate}" + Environment.NewLine + item.Query);
                }
            }
        }

        private static string GetQueryText(DTE2 dte)
        {
            Document document = dte.ActiveDocument;
            if (document == null)
                return null;

            var textDocument = (TextDocument)document.Object("TextDocument");
            string queryText = textDocument.Selection.Text;

            if (string.IsNullOrEmpty(queryText))
            {
                EditPoint startPoint = textDocument.StartPoint.CreateEditPoint();
                queryText = startPoint.GetText(textDocument.EndPoint);
            }

            return queryText;
        }

        private class QueryItem
        {
            public string Query { get; set; }
            public DateTime ExecutionDate { get; set; }
        }
    }
}