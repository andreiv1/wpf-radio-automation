using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic
{
    public static class DebugHelper
    {
        public static void WriteLine(object source, string message)
        {
            Debug.WriteLine($"[{source.GetType().Name}] {message}");
        }
    }
}
