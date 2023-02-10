using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Tracks.Exceptions
{
    public class TrackCorruptFileException : Exception
    {
        public TrackCorruptFileException()
        {
        }

        public TrackCorruptFileException(string? message) : base(message)
        {
        }

        public TrackCorruptFileException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TrackCorruptFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
