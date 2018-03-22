using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace WaterVendingMachine
{
    internal enum Status
    {
        Waiting,
        Pouring
    };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Contructor

        /// <summary>
        /// Default Constuctor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        #endregion

        #region Propertys

        public int Money { get; set; } = 0;

        private Status CurrentStatus { get; set; } = Status.Waiting;

        private string mainScreenText = "Здравствуйте!\nВнесите нужную сумму!";
        public string MainScreenText
        {
            get { return mainScreenText; }
            set
            {
                mainScreenText = value;
                RaisePropertyChanged("MainScreenText");
            }
        }

        #endregion

        private void OnPaperMoneyInput(object sender, MouseButtonEventArgs e)
        {
            MoneySelectWindow window = new MoneySelectWindow
            {
                FirstTextBlock = {Text = "100" },
                SecondTextBlock = {Text = "50" },
                ThirdTextBlock = {Text = "10"}
            };

            window.FirstTextBlock.MouseLeftButtonDown += MoneyTextClicled;
            window.SecondTextBlock.MouseLeftButtonDown += MoneyTextClicled;
            window.ThirdTextBlock.MouseLeftButtonDown += MoneyTextClicled;

            window.Left = GetMousePosition().X;
            window.Top = GetMousePosition().Y;
            window.ShowDialog();
        }

        private void MoneyTextClicled(object sender, MouseButtonEventArgs e)
        {
            Money += int.Parse((sender as TextBlock).Text);
            MainScreenText = "Баланс : " + Money;
        }

        private void OnCoinInput(object sender, MouseButtonEventArgs e)
        {
            MoneySelectWindow window = new MoneySelectWindow
            {
                FirstTextBlock = {Text = "10"},
                SecondTextBlock = {Text = "5"},
                ThirdTextBlock = {Text = "2"},
                ForthTextBlock = {Text = "1"}
            };

            window.FirstTextBlock.MouseLeftButtonDown += MoneyTextClicled;
            window.SecondTextBlock.MouseLeftButtonDown += MoneyTextClicled;
            window.ThirdTextBlock.MouseLeftButtonDown += MoneyTextClicled;
            window.ForthTextBlock.MouseLeftButtonDown += MoneyTextClicled;

            window.Left = GetMousePosition().X;
            window.Top = GetMousePosition().Y;
            window.ShowDialog();
        }

        #region Private Helpers

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(this);

            // Add the window position so its a "ToScreen"
            if (this.WindowState == WindowState.Maximized)
                return new Point(position.X, position.Y);
            else
                return new Point(position.X + this.Left, position.Y + this.Top);
        }

        #endregion

        private void GiveChange(object sender, RoutedEventArgs e)
        {
            if (CurrentStatus == Status.Waiting && Money > 0)
            {
                MainScreenText = "Сдача: " + Money;
            }
        }

        private void TakeChange(object sender, MouseButtonEventArgs e)
        {
            MainScreenText = "Здравствуйте!\nВнесите нужную сумму!";
            MessageBox.Show("Взята сдача " + Money + "руб.");
            Money = 0;
        }

        private void StartAndStopPounding(object sender, RoutedEventArgs e)
        {
            if (CurrentStatus == Status.Waiting && Money > 0)
            {
                CurrentStatus = Status.Pouring;

                while (Money > 0 && CurrentStatus == Status.Pouring)
                {
                    Minus();
                    Money -= 2;
                    MainScreenText = "Баланс : " + Money;
                }
            }

            if (CurrentStatus == Status.Pouring)
            {
                CurrentStatus = Status.Waiting;
            }
        }

        private async void Minus()
        {
            await Task.Delay(1000000);
        }

        private void RaisePropertyChanged(string propName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
