﻿using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO.Abstract
{
    public abstract class PlaylistItemBaseDTO
    {
        public int Id { get; set; }
        public DateTime ETA { get; set; }
        public double Length { get; set; }
        public int PlaylistId { get; set; }

        public static PlaylistItem ToEntity(PlaylistItemBaseDTO dto)
        {
            var entity = new PlaylistItem()
            {
                Id = dto.Id,
                ETA = dto.ETA,
                Length = dto.Length,
                PlaylistId = dto.PlaylistId,
            };
            return entity;
        }
    }
}