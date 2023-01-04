using RA.Database.Models;
using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO.Abstract
{
    public abstract class ClockItemBaseDTO
    {
        public int Id { get; set; }
        public int OrderIndex { get; set; }
        public int ClockId { get; set; }
        public int? ClockItemEventId { get; set; }
        public int? EventOrderIndex { get; set; }

        public static ClockItemBase ToEntity(ClockItemBaseDTO dto)
        {
            if(dto is ClockItemTrackDTO itemTrack)
            {
                return ClockItemTrackDTO.ToEntity(itemTrack);
            }
            else if(dto is ClockItemCategoryDTO itemCategory)
            {
                return ClockItemCategoryDTO.ToEntity(itemCategory);
            }
            else if(dto is ClockItemEventDTO itemEvent)
            {
                return ClockItemEventDTO.ToEntity(itemEvent);
            }

            throw new InvalidOperationException("Unrecognised clock item type.");
        }

        public static ClockItemBaseDTO FromEntity(ClockItemBase entity)
        {
            if(entity is ClockItemCategory itemCategory)
            {
                return ClockItemCategoryDTO.FromEntity(itemCategory);
            }
            else if(entity is ClockItemEvent itemEvent)
            {
                return ClockItemEventDTO.FromEntity(itemEvent);
            }
            throw new Exception("TO DO other types");
        }
    }
}
