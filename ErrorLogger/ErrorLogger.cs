using System;
using System.IO;
using System.Text;

namespace ErrorLogger
{
    public interface ILog
    {
        void Logerror(string error,string path);
    }
    public sealed class Logger : ILog
    {
        private Logger() { }
        private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());
        public static Logger GetInstance
        {
            get { return instance.Value; }
        }

        public   void Logerror(string error,string path)
        {
            try
            {
                var dtmCurrent = DateTime.Now;
                string strCurrentDate = string.Format("{0}-{1}-{2}.txt", dtmCurrent.Month.ToString().PadLeft(2, '0'), dtmCurrent.Day.ToString().PadLeft(2, '0'), dtmCurrent.Year);

                string fileName = string.Format("{0}_{1}", "Exception", strCurrentDate);
                string logFilePath = string.Format(@"{0}\{1}", path, fileName);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("");
                sb.AppendLine(DateTime.Now.ToString() + " Begin | ----------------------------------------------------------------- ");
                sb.AppendLine(error);
                sb.AppendLine(DateTime.Now.ToString() + "   End | ----------------------------------------------------------------- ");
               
                using (StreamWriter writer = new StreamWriter(logFilePath,true))
                {
                      writer.Write(sb.ToString());

                      writer.Flush();
                } 
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
            } 
        }
    }
}
