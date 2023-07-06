using RA.DAL.Interfaces;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Settings.ViewModels.Security
{
    public partial class SettingsManageUserViewModel : DialogViewModelBase
    {
        private readonly int? userId;
        private readonly IUserGroupsService userGroupsService;

        public ObservableCollection<UserGroupDTO> Groups { get; private set; } = new();
        public SettingsManageUserViewModel(IWindowService windowService, IUserGroupsService userGroupsService) : base(windowService)
        {
            this.userGroupsService = userGroupsService;
            _ = LoadGroups();
        }
        public SettingsManageUserViewModel(IWindowService windowService, 
                                          IUserGroupsService userGroupsService, 
                                          int userId) 
            : this(windowService, userGroupsService)
        {
            this.userId = userId;
        }

        private async Task LoadGroups()
        {
            Groups.Clear();
            foreach (var g in await userGroupsService.GetGroups())
            {
                Groups.Add(g);
            }
        }
        protected override bool CanFinishDialog()
        {
            return false;
        }
    }
}
