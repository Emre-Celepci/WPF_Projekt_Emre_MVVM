using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Projekt_Emre.Commands;
using WPF_Projekt_Emre.pages;

namespace WPF_Projekt_Emre.ViewModels
{
    public class PlayerCountViewModel : BaseViewModel
    {
        private readonly Frame _mainFrame;
        private int _selectedPlayerCount;

        public int SelectedPlayerCount
        {
            get { return _selectedPlayerCount; }
            set
            {
                _selectedPlayerCount = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }

        public PlayerCountViewModel(Frame mainFrame)
        {
            _mainFrame = mainFrame;

            BackCommand = new RelayCommand(_ =>
            {
                _mainFrame.Navigate(new StartPage(_mainFrame));
            });

            NextCommand = new RelayCommand(_ =>
            {
                if (SelectedPlayerCount < 2 || SelectedPlayerCount > 4)
                {
                    MessageBox.Show("Bitte wähle 2 bis 4 Spieler aus.");
                    return;
                }

                _mainFrame.Navigate(new PlayerConfigPage(_mainFrame, SelectedPlayerCount));
            });
        }
    }
}