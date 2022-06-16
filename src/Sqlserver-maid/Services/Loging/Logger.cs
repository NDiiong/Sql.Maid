﻿using System;
using System.IO;
using System.Text;

namespace Sqlserver.maid.Services.Loging
{
    public class Logger
    {
        private static StreamWriter _writer { get; set; }

        public static void Debug(string message)
            => Log(LogVerbosity.Debug, message);

        public static void Info(string message)
            => Log(LogVerbosity.Info, message);

        public static void Warn(string message)
            => Log(LogVerbosity.Warn, message);

        public static void Error(string message)
            => Log(LogVerbosity.Error, message);

        public static void Error(Exception exception)
            => Log(LogVerbosity.Error, exception.GetFullMessage());

        public static void Error(Exception exception, string message)
        {
            Error(message);
            Error(exception);
        }

        public static void Special(string message)
            => Log(LogVerbosity.Motd, message);

        private enum LogVerbosity
        {
            Debug,
            Info,
            Warn,
            Error,
            Motd
        }

        private static void Log(LogVerbosity verbosity, string message)
        {
            if (_writer == null)
                Initialize();

            var data = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss:ff} | {verbosity.ToString().ToUpper()} | {message}";
            lock (_writer)
            {
                _writer.WriteLine(data);
                _writer.Flush();
            }
        }

        private static void Initialize()
        {
            var localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sqlserver-maid");

            var location = Path.Combine(localPath, "logs");
            Directory.CreateDirectory(location);

            var path = Path.Combine(location, "log.txt");
            _writer = new StreamWriter(path);
        }
    }

    public static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception exception)
        {
            var builder = new StringBuilder();
            Exception realerror = exception;
            builder.AppendLine(exception.Message);
            while (realerror.InnerException != null)
            {
                builder.AppendLine(realerror.InnerException.Message);
                realerror = realerror.InnerException;
            }
            return builder.ToString();
        }
    }
}