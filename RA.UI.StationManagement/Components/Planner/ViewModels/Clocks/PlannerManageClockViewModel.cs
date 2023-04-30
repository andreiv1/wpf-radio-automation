using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.Database.Models;
using RA.Database;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Clocks
{
    public partial class PlannerManageClockViewModel : DialogViewModelBase
    {
        [ObservableProperty]
        private String clockName = "";

        private int? clockId;
        private bool duplicate = false;
        public PlannerManageClockViewModel(IWindowService windowService) : base(windowService)
        {
            DialogName = "Add new clock";
        }

        public PlannerManageClockViewModel(IWindowService windowService, int clockId, bool duplicate = false) : base(windowService)
        {
            DialogName = "Edit clock";
            this.clockId = clockId;
            this.duplicate = duplicate;
            LoadClock();
        }

        private void LoadClock()
        {
            //if (!clockId.HasValue)
            //{
            //    return;
            //}
            //using (var db = new AppDbContext())
            //{
            //    ClockName = db.Clocks
            //        .Where(c => c.Id == clockId.Value)
            //        .Select(c => c.Name)
            //        .First();
            //}
            //if (dialogOperation == DialogOperation.Duplicate)
            //{
            //    ClockName += " (COPY)";
            //}
        }

        [RelayCommand]
        private void SaveClock()
        {
            //if (ClockName is null)
            //{
            //    return;
            //}
            //using (var db = new AppDbContext())
            //{
            //    Clock clock = new Clock();
            //    clock.Name = ClockName.Trim();
            //    if (clockId.HasValue && dialogOperation == DialogOperation.Edit)
            //    {
            //        clock.Id = clockId.Value;
            //        db.Clocks.Update(clock);
            //    }
            //    else
            //    {
            //        db.Clocks.Add(clock);
            //    }

            //    if (dialogOperation == DialogOperation.Duplicate)
            //    {
            //        DuplicateClockItems(db, clock);
            //    }

            //    db.SaveChangesAsync();
            //}
            base.CancelDialog();
        }

        private void DuplicateClockItems(AppDbContext db, Clock clock)
        {
            //if (!clockId.HasValue)
            //{
            //    return;
            //}

            //if (dialogOperation == DialogOperation.Duplicate)
            //{

            //    var clockItems = db.ClockItems.Where(ci => ci.ClockId == clockId.Value);

            //    var duplicates = new List<ClockItem>();
            //    foreach (var clockItem in clockItems)
            //    {
            //        duplicates.Add(new ClockItem
            //        {
            //            Clock = clock,
            //            OrderIndex = clockItem.OrderIndex,
            //            CategoryId = clockItem.CategoryId,
            //            TrackId = clockItem.TrackId,
            //        });
            //    }

            //    _ = db.ClockItems.AddRangeAsync(duplicates);

            //}
        }

        protected override bool CanFinishDialog()
        {
            return false;
        }
    }
}
