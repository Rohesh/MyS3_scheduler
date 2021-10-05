using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyS3_scheduler
{
    public class ErrorLog
    {
        public void WriteToLog(string message)
        {

            var path = Path.Combine("Entity", "ErrorLog.txt");
            using (var mutex = new Mutex(false, "LARGEFILE"))
            {
                mutex.WaitOne();
                File.AppendAllText(path, " " + message + Environment.NewLine);
                mutex.ReleaseMutex();
            }
            

        }

    }
}



