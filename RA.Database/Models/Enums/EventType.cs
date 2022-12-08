using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models.Enums
{
    public enum EventType
    {
        /// <summary>
        /// A non-playable label or note
        /// </summary>
        Marker,
        /// <summary>
        /// A fixed break is a break that is scheduled to occur at a specific time during the music playing process, 
        /// and its start time is fixed. The fixed break will interrupt the current playing item at the scheduled time
        /// </summary>
        FixedBreak,
        /// <summary>
        /// A dynamic break is a break that can occur at any point during the music playing process, and its start time is not fixed
        /// </summary>
        DynamicBreak,


    }
}
