using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic.Exceptions
{
    public class TrackFileReadException : Exception
    {
        public TrackFileReadException()
        {
        }

        public TrackFileReadException(string? message) : base(message)
        {
        }

        public TrackFileReadException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TrackFileReadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
