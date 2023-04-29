using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic.Exceptions
{
    public class TrackUnsupportedFormatException : Exception
    {
        public TrackUnsupportedFormatException()
        {
        }

        public TrackUnsupportedFormatException(string? message) : base(message)
        {
        }

        public TrackUnsupportedFormatException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TrackUnsupportedFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
