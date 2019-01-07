using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FaceRecognition.GUILayer
{
    public class LogHelper
    {
        public static void LogException(IEnumerable<string> errors)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"********** (Error Log: {DateTime.Now}) **********");
            foreach(var error in errors)
            {
                sb.AppendLine(error);
            }
            sb.AppendLine("********************************************************");
            sb.AppendLine("\r\n\r\n");
            try
            {
                
                using (StreamWriter writer = new StreamWriter(Global.LogFileName,true))
                {
                    writer.WriteLine(sb.ToString());
                }
            }
            catch { }
        }
    }
}
