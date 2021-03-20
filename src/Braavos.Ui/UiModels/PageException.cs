using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braavos.Ui
{
    public class PageException
    {
        public string Message { get; set; }
        public string Exception { get; set; }

        public PageException() { }

        public PageException(string message, Exception exception)
        {
            Message = message;
            Exception = exception.Message + Environment.NewLine + exception.StackTrace;
        }
    }
}
