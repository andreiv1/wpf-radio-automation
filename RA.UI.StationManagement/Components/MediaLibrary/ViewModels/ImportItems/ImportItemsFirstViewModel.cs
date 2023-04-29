using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Dto;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RA.UI.Core.Services.Interfaces;
//using RA.UI.StationManagement.Dialogs.ViewModels;
using RA.Logic;
using RA.UI.Core.Services;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
using RA.DAL;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.ImportItems
{
    public partial class ImportItemsFirstViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IFolderBrowserDialogService folderBrowserDialog;
        private readonly ICategoriesService categoriesService;

        public ImportItemsModel Model { get; set; }

        public ImportItemsFirstViewModel()
        {
        }

        public ImportItemsFirstViewModel(IWindowService windowService, IFolderBrowserDialogService folderBrowserDialog, 
            ICategoriesService categoriesService)
        {
            this.windowService = windowService;
            this.folderBrowserDialog = folderBrowserDialog;
            this.categoriesService = categoriesService;
        }

     
        [RelayCommand]
        private void PickFolder()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                Model.FolderPath = folderBrowserDialog.SelectedPath;
            }
        }

        [RelayCommand]
        private async void PickCategory()
        {
            var dialog = windowService.ShowDialog<CategorySelectViewModel>();
            var selectedCategory = dialog.SelectedCategory;
            if(selectedCategory != null)
            {
                DebugHelper.WriteLine(this, $"Picked category: {selectedCategory.CategoryId}, {selectedCategory.Name}");
                Model.SelectedCategory = await categoriesService.GetCategoryHierarchy(selectedCategory.CategoryId);
            }
        }
    }
}
