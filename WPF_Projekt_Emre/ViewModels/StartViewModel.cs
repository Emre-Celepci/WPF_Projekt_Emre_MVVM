using System.Windows.Controls;
using System.Windows.Input;
using WPF_Projekt_Emre.Commands;
using WPF_Projekt_Emre.pages;

namespace WPF_Projekt_Emre.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private readonly Frame _mainFrame;

        public ICommand StartGameCommand { get; }
        public ICommand ShowHighscoreCommand { get; }

        public StartViewModel(Frame mainFrame)
        {
            _mainFrame = mainFrame;

            StartGameCommand = new RelayCommand(_ =>
            {
                _mainFrame.Navigate(new PlayerCountPage(_mainFrame));
            });

            ShowHighscoreCommand = new RelayCommand(_ =>
            {
                _mainFrame.Navigate(new HighscorePage(_mainFrame));
            });
        }
    }
}