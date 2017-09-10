#region

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

#endregion

namespace Advitex.ExceptionAnalizer.Utils
{
    public static class Logger
    {
        static Logger()
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            // ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException
            var hasListener = Trace.Listeners.Cast<TraceListener>().Any(l => l.Name == ExceptionAnalyzer);

            if (!hasListener)
                Trace.Listeners.Add(new TextWriterTraceListener(LogPath, ExceptionAnalyzer));
            // ReSharper restore NotDocumentedExceptionHighlighting
            // ReSharper restore NotDocumentedExceptionHighlighting.ArgumentNullException
        }

        public static void LogError(Exception ex)
        {
            Trace.TraceError("");
            Trace.TraceError("----------------------------------------------------------------------");
            Trace.TraceError(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            Trace.TraceError(ex.ToString());
            Trace.Flush();
        }

        #region Private members

        private const string LogPath = "C:\\Program Files\\JetBrains\\ReSharper\\v7.1\\ExceptionAnalyzer.log";
        private const string ExceptionAnalyzer = "ExceptionAnalyzer";

        #endregion
    }
}