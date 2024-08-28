#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SampleBrowser.Maui.Scheduler.SfScheduler
{

    /// <summary>
    /// The data binding View Model.
    /// </summary>
    public class SchedulerDataBindingViewModel : INotifyPropertyChanged
    {
        #region Fields

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        /// <summary>
        /// Event handler.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDataBindingViewModel" /> class.
        /// </summary>
        public SchedulerDataBindingViewModel()
        {
            this.IntializeDragDropAppoitments();
        }

        private ObservableCollection<Meeting>? _schedulerDragEvents;
        public ObservableCollection<Meeting>? SchedulerDragEvents
        {
            get => _schedulerDragEvents;
            set
            {
                _schedulerDragEvents = value;
                OnPropertyChanged(nameof(SchedulerDragEvents));
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Method to generate drag and drop appointments.  
        /// </summary>
        private void IntializeDragDropAppoitments()
        {
            this.SchedulerDragEvents = new ObservableCollection<Meeting>();
        }

        #endregion
    }
}
