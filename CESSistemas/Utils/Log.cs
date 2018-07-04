using System;
using System.Text;
using System.IO;

namespace Promig.Utils {
    class Log {

        public static StringBuilder log = new StringBuilder();
        public static void logException(Exception e){
            log.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            log.AppendLine();
            log.Append(e.Message);
            log.AppendLine();
            log.Append(e.StackTrace);
            log.AppendLine();
            if(e.InnerException != null){
                log.AppendLine("InnerException: " + e.InnerException.Message);
            }
            File.AppendAllText("LogError_Promig.txt", log.ToString());
            log.Clear();
        }

        public static void logMessage(string message){
            log.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            log.Append(" " + message);
            log.AppendLine();
            File.AppendAllText("LogMessage_Promig.txt", log.ToString());
            log.Clear();
        }
    }
}
