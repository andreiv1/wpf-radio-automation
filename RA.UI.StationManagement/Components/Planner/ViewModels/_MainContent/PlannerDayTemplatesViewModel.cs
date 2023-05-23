using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using RA.UI.StationManagement.Components.Planner.ViewModels.Templates;
using RA.UI.StationManagement.Components.Planner.ViewModels.Templates.Models;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerDayTemplatesViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly ITemplatesService templatesService;
        private readonly IDispatcherService dispatcherService;

        public ObservableCollection<TemplateDTO> Templates { get; set; } = new();
        public ObservableCollection<TemplateClockItemModel> ClocksForSelectedTemplate { get; set; } = new();

        [ObservableProperty]
        private TemplateDTO? selectedTemplate = null;

        partial void OnSelectedTemplateChanged(TemplateDTO? value)
        {
            _ = LoadClocksForSelectedTemplate();
        }

        [ObservableProperty]
        private string searchQuery = "";

        private const int searchDelayMilliseconds = 500; // Set an appropriate delay time

        private CancellationTokenSource? searchQueryToken;
        partial void OnSearchQueryChanged(string value)
        {
            if (searchQueryToken != null)
            {
                searchQueryToken.Cancel();
            }

            searchQueryToken = new CancellationTokenSource();
            var cancellationToken = searchQueryToken.Token;
            Task.Delay(searchDelayMilliseconds, cancellationToken).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && !cancellationToken.IsCancellationRequested)
                {
                    DebugHelper.WriteLine(this, $"Performing search query: {value}");
                    _ = LoadTemplates(value);
                }
            });
        }

        public PlannerDayTemplatesViewModel(IWindowService windowService,
                                            IMessageBoxService messageBoxService,
                                            ITemplatesService templatesService,
                                            IDispatcherService dispatcherService)
        {
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.templatesService = templatesService;
            this.dispatcherService = dispatcherService;

            _ = LoadTemplates();
        }

        #region Data fetching
        private async Task LoadTemplates(string? query = null)
        {
            var templates = await templatesService.GetTemplatesAsync(query);
            if (string.IsNullOrEmpty(query)) SearchQuery = string.Empty;
            Templates.Clear();
            foreach (var template in templates)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    Templates.Add(template);
                });
            }
        }

        private async Task LoadClocksForSelectedTemplate()
        {
            if (SelectedTemplate == null) return; 
            var items = await templatesService.GetTemplatesForClockAsync(SelectedTemplate.Id);
            ClocksForSelectedTemplate.Clear();
            foreach (var item in items)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    ClocksForSelectedTemplate.Add(TemplateClockItemModel.FromDto(item));
                });
            }
        }

        #endregion

        public async Task AddClockToTemplate(int clockId, TimeSpan start, int span)
        {
            if (SelectedTemplate == null) return;
            var clockTemplate = new ClockTemplateDTO()
            {
                ClockId = clockId,
                StartTime = start,
                ClockSpan = span,
                TemplateId = SelectedTemplate.Id,
            };
            if (ClocksForSelectedTemplate.Where(c => c.StartTime.TimeOfDay == start).Any()) return;
                await templatesService.AddClockToTemplate(clockTemplate);
            _ = LoadClocksForSelectedTemplate();
        }

        public async Task UpdateClockToTemplate(int clockId, DateTime oldStart, DateTime newStart, DateTime newEnd)
        {
            if (SelectedTemplate == null) return;
            if (IsOverlapping(oldStart, newStart, newEnd, clockId))
            {
                messageBoxService.ShowWarning("The new time overlaps with an existing clock. Please resize it without overlapping existing clock(s).");
                _ = LoadClocksForSelectedTemplate();
                return;
            }
            var clockTemplate = new ClockTemplateDTO()
            {
                ClockId = clockId,
                StartTime = newStart.TimeOfDay,
                ClockSpan = (int)newEnd.Subtract(newStart).TotalHours,
                TemplateId = SelectedTemplate.Id,
            };
            await templatesService.UpdateClockInTemplate(oldStart.TimeOfDay, clockTemplate);
            _ = LoadClocksForSelectedTemplate();
        }

        private bool IsOverlapping(DateTime oldStart, DateTime newStart, DateTime newEnd, int clockIdToExclude)
        {
            foreach (var clock in ClocksForSelectedTemplate)
            {
                if (clock.ClockId == clockIdToExclude && clock.StartTime == oldStart)
                {
                    // Ignore the clock that's being updated
                    continue;
                }

                if ((clock.StartTime < newEnd && clock.EndTime > newStart) || (clock.EndTime > newStart && clock.EndTime < newEnd))
                {
                    // Overlapping found
                    return true;
                }
            }

            // No overlapping found
            return false;
        }

        #region Commands
        [RelayCommand]
        private void AddTemplateDialog()
        {
            var result = windowService.ShowDialog<PlannerManageTemplateViewModel>();
            TemplateModel template = result.ManagedTemplate;
            templatesService.AddTemplate(TemplateModel.ToDto(template));

            _ = LoadTemplates();
        }

        [RelayCommand]
        private void EditTemplateDialog()
        {
            if (SelectedTemplate == null) return;
            var result = windowService.ShowDialog<PlannerManageTemplateViewModel>(SelectedTemplate.Id);
            TemplateModel template = result.ManagedTemplate;
            templatesService.UpdateTemplate(TemplateModel.ToDto(template));
            _ = LoadTemplates();
        }

        [RelayCommand]
        private void DeleteTemplateDialog()
        {
            //throw new NotImplementedException();
        }

        [RelayCommand]
        private void DuplicateTemplateDialog()
        {
            //throw new NotImplementedException();
        }

        [RelayCommand]
        private void RefreshTemplates()
        {
            _ = LoadTemplates();
            SearchQuery = string.Empty;
            SelectedTemplate = null;
            ClocksForSelectedTemplate.Clear();
        }

        [RelayCommand]
        private async void InsertClock(SchedulerContextMenuInfo schedulerContextMenuInfo)
        {
            var vm = windowService.ShowDialog<PlannerTemplateSelectClockViewModel>();
            if(vm.SelectedClock == null || schedulerContextMenuInfo.DateTime == null) return;
            await AddClockToTemplate(vm.SelectedClock.Id, schedulerContextMenuInfo.DateTime.Value.TimeOfDay, 1);
        }

        [RelayCommand]
        private void EditClock(SchedulerContextMenuInfo schedulerContextMenuInfo)
        {

        }

        [RelayCommand]
        private async void DeleteClock(SchedulerContextMenuInfo schedulerContextMenuInfo)
        {
            var model = schedulerContextMenuInfo.Appointment.Data as TemplateClockItemModel;
            if (model == null || SelectedTemplate == null) return;
            await templatesService.DeleteClockInTemplate(SelectedTemplate.Id, model.ClockId, model.StartTime.TimeOfDay);
            await LoadClocksForSelectedTemplate();
        }
        #endregion
    }
}
