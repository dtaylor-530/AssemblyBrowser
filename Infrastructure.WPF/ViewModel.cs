using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AssemblyBrowserGUI.ViewModel
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
       protected IDialogService dialogService;
        private object? content = "Null";

        public ViewModel()
        {
           dialogService = new DefaultDialogService();
        }

        public object? Content
        {
           get => content;
           set{
              content = value;
              OnPropertyChanged();
           }
        }

        public ICollection<ViewModel> Collection { get; } = new ObservableCollection<ViewModel>();

        #region PropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
