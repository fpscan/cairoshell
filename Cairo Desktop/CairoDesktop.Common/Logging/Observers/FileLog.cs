﻿using System;
using System.IO;

namespace CairoDesktop.Common.Logging.Observers
{
    /// <summary>
    /// Writes log events to a local file.
    /// </summary>
    /// <remarks>
    /// GoF Design Pattern: Observer.
    /// The Observer Design Pattern allows this class to attach itself to an
    /// the logger and 'listen' to certain events and be notified of the event. 
    /// </remarks>
    public class FileLog : DisposableLogBase
    {
        private readonly FileInfo _fileInfo;
        private TextWriter _textWriter;

        /// <summary>
        /// Constructor of ObserverLogToFile.
        /// </summary>
        /// <param name="fileName">File log to.</param>
        public FileLog(string fileName)
        {
            _fileInfo = new FileInfo(fileName);
        }

        public void Open()
        {
            if (!Directory.Exists(_fileInfo.DirectoryName))
                Directory.CreateDirectory(_fileInfo.DirectoryName);

            var stream = File.AppendText(_fileInfo.FullName);
            stream.AutoFlush = true;

            _textWriter = TextWriter.Synchronized(stream);
        }

        protected override void DisposeOfManagedResources()
        {
            base.DisposeOfManagedResources();

            try
            {
                _textWriter.Flush();
                _textWriter.Close();
                _textWriter.Dispose();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Write a log request to a file.
        /// </summary>
        /// <param name="sender">Sender of the log request.</param>
        /// <param name="e">Parameters of the log request.</param>
        public override void Log(object sender, LogEventArgs e)
        {
            string message = string.Format("[{0}] {1}: {2}", e.Date, e.SeverityString, e.Message);

            try
            {
                _textWriter.WriteLine(message);
                _textWriter.Flush();
            }
            catch (Exception)
            {
                /* do nothing */
            }
        }
    }
}