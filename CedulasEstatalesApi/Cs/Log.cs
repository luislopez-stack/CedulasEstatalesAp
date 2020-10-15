using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace CedulasEstatalesApi
{
    public static class Log
    {
        public static void WriteInfo(string mensaje) {
            Trace.TraceInformation(mensaje);
        }

        public static void WriteWarning(string mensaje)
        {
            Trace.TraceWarning(mensaje);
        }

        public static void WriteError(string mensaje)
        {
            Trace.TraceError(mensaje);
        }

    }
}