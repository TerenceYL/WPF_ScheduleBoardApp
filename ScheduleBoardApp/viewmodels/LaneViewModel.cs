using System.Collections.ObjectModel;
using System.ComponentModel;
using ScheduleBoardApp.Models;

namespace ScheduleBoardApp.ViewModels
{
    /// <summary>
    /// 每條泳道（欄位）的 ViewModel
    /// </summary>
    public class LaneViewModel : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}