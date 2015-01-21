using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DoodleEmpires.Core
{
    public static class Logger
    {
        static StreamWriter m_memoryStream;

        static Logger()
        {
            m_memoryStream = new StreamWriter("logs.txt");
        }

        public static void Close()
        {
            m_memoryStream.Close();
            m_memoryStream.Dispose();
        }

        public static void LogMessage(string message, params object[] arguments)
        {
            if (!string.IsNullOrWhiteSpace(message))
                m_memoryStream.WriteLine(string.Format(message, arguments));
        }

        internal static void LogException(Exception e)
        {
            LogMessage("Exception has occured: ", e.Message);
            LogMessage(e.StackTrace);
        }
    }
}
