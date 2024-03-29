﻿using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models
{
    public partial class ClockModel : ObservableValidator
    {
        public int Id { get; set; }

        private string name = "";

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        public void Validate()
        {
            this.ValidateAllProperties();
        }


        public static ClockModel FromDto(ClockDTO dto)
        {
            return new ClockModel()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static ClockDTO ToDto(ClockModel model)
        {
            return new ClockDTO()
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
