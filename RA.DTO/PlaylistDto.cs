﻿using RA.Database.Models;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class PlaylistDTO
    {
        public int Id { get; set; }

        public DateTime AirDate { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }

        public List<PlaylistItemBaseDTO>? Items { get; set; }

        public static PlaylistDTO FromEntity(Playlist entity)
        {
            //TODO: get items
            var dto = new PlaylistDTO { 
                Id = entity.Id, 
                AirDate = entity.AirDate, 
                DateAdded = entity.DateAdded, 
                DateModified = entity.DateModified 
            };

            return dto;
        }

        public static Playlist ToEntity(PlaylistDTO dto)
        {
            //TODO: add items
            var entity = new Playlist
            {
                Id = dto.Id,
                AirDate = dto.AirDate,
                DateAdded = dto.DateAdded,
                PlaylistItems = dto.Items.Select(x =>
                {
                    if (x.GetType() == typeof(PlaylistItemTrackDTO))
                    {
                        return PlaylistItemTrackDTO.ToEntity(x as PlaylistItemTrackDTO);
                    }
                    return null;
                }).ToList()
            };

            return entity;
        }
    }
}
