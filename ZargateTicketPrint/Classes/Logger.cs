using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace ZargateTicketPrint.Classes
{
    internal static class Logger
    {
        #region Severity enum

        public enum Severity
        {
            INFO = 0,
            WARNING = 1,
            ERROR = 2
        }

        #endregion

        private static readonly ObservableCollection<LogMessage> _uploadToSQLLog =
            new ObservableCollection<LogMessage>();

        private static readonly ObservableCollection<LogMessage> _completeLog = new ObservableCollection<LogMessage>();

        public static void Add(string message)
        {
            Add(message, Severity.INFO);
        }

        public static void Add(string message, Severity severity)
        {
            Action doAdd = () =>
                               {
                                   if (message == null) throw new ArgumentNullException("message");
                                   var logMessage = new LogMessage(message, severity, DateTime.Now);
                                   _completeLog.Add(logMessage);
                                   _uploadToSQLLog.Add(logMessage);
                               };
            Application.Current.Dispatcher.BeginInvoke(doAdd);
        }

        public static ObservableCollection<LogMessage> GetLog()
        {
            return _completeLog;
        }

        public static IList<string> GetErrors()
        {
            return getMessages(Severity.ERROR);
        }

        public static IList<string> GetInfos()
        {
            return getMessages(Severity.INFO);
        }

        public static IList<string> GetWarnings()
        {
            return getMessages(Severity.WARNING);
        }

        private static ObservableCollection<string> getMessages(Severity severity)
        {
            var messages = new ObservableCollection<string>();
            var completeLog = new ObservableCollection<LogMessage>(_completeLog);
            foreach (LogMessage logMessage in completeLog)
            {
                if (logMessage.Severity == severity)
                {
                    messages.Add(logMessage.Message);
                }
            }
            return messages;
        }

        public static IList<LogMessage> GetLogToUploadToSQL()
        {
            var uploadToSQLLog = new ObservableCollection<LogMessage>(_uploadToSQLLog);
            _uploadToSQLLog.Clear();
            return uploadToSQLLog;
        }
    }

    internal class LogMessage
    {
        private readonly string _message;
        private readonly Logger.Severity _severity;
        private readonly DateTime _timestamp;

        public LogMessage(string message, Logger.Severity severity, DateTime timestamp)
        {
            _timestamp = timestamp;
            _severity = severity;
            _message = message;
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        public Logger.Severity Severity
        {
            get { return _severity; }
        }

        public string Message
        {
            get { return _message; }
        }
    }
}