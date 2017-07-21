using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SenseHatTelemeter.ViewModel
{
    public class TelemetryViewModel: INotifyPropertyChanged
    {
        #region Public Properties

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                OnPropertyChanged();

                IsStartTelemetryButtonEnabled = value;
                OnPropertyChanged("IsStartTelemetryButtonEnabled");

                IsStopTelemetryButtonEnabled = !value;
                OnPropertyChanged("IsStopTelemetryButtonEnabled");
            }
        }

        public bool IsTelemetryActive
        {
            get { return isTelemetryActive; }
            set
            {
                isTelemetryActive = value;
                OnPropertyChanged();

                IsStartTelemetryButtonEnabled = !value && IsConnected ;
                OnPropertyChanged("IsStartTelemetryButtonEnabled");

                IsStopTelemetryButtonEnabled = !value  && IsConnected ; 
                OnPropertyChanged("IsStopTelemetryButtonEnabled");
            }
        }

        public float Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnPropertyChanged();
            }
        }

        public float Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                OnPropertyChanged();
            }
        }

        public bool IsStartTelemetryButtonEnabled { get; private set; }
        public bool IsStopTelemetryButtonEnabled { get; private set; }

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields
        private float temperature;
        private float humidity;
        private bool isConnected;
        private bool isTelemetryActive;
        #endregion

        #region Methods
            private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
