using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL.Interfaces;
using RA.Database.Models;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Settings.ViewModels.Security
{
    public partial class SettingsManageGroupViewModel : DialogViewModelBase
    {
        private readonly IMessageBoxService messageBoxService;
        private readonly IUserGroupsService userGroupsService;

        [ObservableProperty]
        private int? groupId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private string groupName = string.Empty;

        public ObservableCollection<UserRuleType> GroupRules { get; set; } = new();
        public List<UserRuleType> ExistingRules => Enum.GetValues(typeof(UserRuleType)).Cast<UserRuleType>().ToList();

        [ObservableProperty]
        private UserRuleType? selectedExistingRule;

        [ObservableProperty]
        private UserRuleType? selectedGroupRule;

        public SettingsManageGroupViewModel(IWindowService windowService,
                                            IMessageBoxService messageBoxService,
                                            IUserGroupsService userGroupsService) : base(windowService)
        {
            this.messageBoxService = messageBoxService;
            this.userGroupsService = userGroupsService;
        }

        public SettingsManageGroupViewModel(IWindowService windowService,
                                            IMessageBoxService messageBoxService,
                                            IUserGroupsService userGroupsService,
                                            int groupId) 
            : this(windowService, messageBoxService, userGroupsService)
        {
            _ = LoadGroup(groupId);
        }

        private async Task LoadGroup(int groupId)
        {
            var GroupDTO = await userGroupsService.GetGroup(groupId);
            if (GroupDTO != null)
            {
                GroupId = GroupDTO.Id;
                GroupName = GroupDTO.Name;
                GroupDTO.Rules?.ForEach((r) => GroupRules.Add(r));
            }

        }

        private async Task AddGroup()
        {
            UserGroupDTO dto = new UserGroupDTO()
            {
                Name = GroupName,
                Rules = GroupRules.ToList(),
            };
            await userGroupsService.AddGroup(dto);
        }

        private async Task UpdateGroup()
        {
            UserGroupDTO dto = new UserGroupDTO()
            {
                Id = GroupId,
                Name = GroupName,
                Rules = GroupRules.ToList(),
            };

            await userGroupsService.UpdateGroup(dto);
        }

        protected override void FinishDialog()
        {
            if(GroupId.HasValue)
            {
                _ = UpdateGroup();
            } else
            {
                _ = AddGroup();
            }
            base.FinishDialog();
        }
        protected override bool CanFinishDialog()
        {
            return !string.IsNullOrEmpty(GroupName) && GroupRules.Count > 0;
        }

        [RelayCommand]
        private void AddRule()
        {
            if(SelectedExistingRule != null)
            {
                if(!GroupRules.Contains(SelectedExistingRule.Value))
                {
                    GroupRules.Add(SelectedExistingRule.Value);
                    SelectedExistingRule = null;
                    FinishDialogCommand.NotifyCanExecuteChanged();
                }
            }
        }

        [RelayCommand]
        private void RemoveRule()
        {
            if(SelectedGroupRule != null)
            {
                GroupRules.Remove(SelectedGroupRule.Value);
                SelectedGroupRule = null;
                FinishDialogCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
