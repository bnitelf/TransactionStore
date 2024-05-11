using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionStore.Utils
{
    public class DebugUtil
    {
        public static string ModelClassToString(object obj)
        {
            StringBuilder sb = new StringBuilder();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                sb.AppendLine(string.Format("{0}={1}", name, value));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get Exception Message (only message is return) recursive to all child InnerException.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isInnerException"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception ex, bool isInnerException = false)
        {
            if (ex == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            if (!isInnerException)
            {
                sb.AppendLine("Exception: " + ex.Message);
            }
            else
            {
                sb.AppendLine("InnerException: " + ex.Message);
            }

            if (ex.InnerException == null)
            {
                sb.AppendLine("InnerException: null");
            }
            else
            {
                sb.Append(GetExceptionMessage(ex.InnerException, true));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get Full Exception Message (ex. Exception type, HResult, Message, Stacktrace) 
        /// Recursive to all child InnerException.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isInnerException"></param>
        /// <returns></returns>
        public static string GetFullExceptionMessage(Exception ex, bool isInnerException = false)
        {
            if (ex == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            if (!isInnerException)
            {
                sb.AppendLine("Exception: " + ex.GetType().Name);
            }
            else
            {
                sb.AppendLine("---> InnerExeption: " + ex.GetType().Name);
            }

            sb.AppendLine("HResult: " + ex.HResult);
            sb.AppendLine("Message: " + ex.Message);
            sb.AppendLine("StackTrace:" + ex.StackTrace);
            if (ex.InnerException == null)
            {
                sb.AppendLine("---> InnerExeption: null");
            }
            else
            {
                sb.AppendLine(GetFullExceptionMessage(ex.InnerException, true));
            }

            return sb.ToString();
        }
    }
}
